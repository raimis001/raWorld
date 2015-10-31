using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Sound;







public class Fire : AttachableThing, SizeReporter
{
	//Working vars - gameplay
	public float			fireSize = MinFireSize; //1 is a medium-sized campfire
	private int				ticksSinceSpark; //Unsaved, unimportant
	private int				ticksSinceDamage; //Unsafed, unimportant
	

	//Working vars - damage
	private Material		curDrawFrame = MatsSimple.BadMaterial;
	private int				ticksUntilFrameChange = 0;
	private Vector2			curDrawOffset = Vector2.zero;

	//Working vars - audiovisual
	private int				ticksUntilSmoke = 0;
	private Sustainer		sustainer = null;


	//Constants and content
	private static readonly SoundDef BurningSoundDef = SoundDef.Named("FireBurning");

	public const float		MinFireSize = 0.1f;
	private const float		MinSizeForSpark = 1f;
	private const float		TicksBetweenSparksBase = 150; //Halves for every fire size
	private const float		TicksBetweenSparksReductionPerFireSize = 40;
	private const float		MinTicksBetweenSparks = 75;
	private const float		MinFireSizeToEmitSpark = 1f;
	private const float		MaxFireSize = 1.75f;

	private const float		SquareIgniteChancePerTickPerSize = 0.01f;
	private const float		MinSizeForSquareIgnite = 0.6f;

	private const float		FireBaseGrowthPerTick = 0.0005f;

	private const float		BaseTicksBetweenDamage = 95;
	private const float		TicksBetweenDamageReductionPerFireSize = 40;
	private const float		MinTicksBetweenDamage = 15;

	private static readonly IntRange SmokeIntervalRange = new IntRange(70,107);
	private const int		SmokeIntervalRandomAddon = 10;

	private static readonly IntRange FrameChangeIntervalRange = new IntRange(10,20);

	private const float		BaseSkyExtinguishChance = 0.04f;
	private const int		BaseSkyExtinguishDamage = 2;


	//Properties
	public override string Label
	{
		get
		{
			if( parent != null )
				return "FireOn".Translate( parent.Label);	
			else
				return "Fire".Translate();
		}
	}
	public override string InfoStringAddon
	{
		get
		{
			return "Burning".Translate() + " (" + "FireSizeLower".Translate( fireSize.ToString("###0.0") ) + ")";	
		}
	}
	private float TicksBeforeSpark
	{
		get
		{
			if( fireSize < MinSizeForSpark )
				return 999999;

			float ticks = TicksBetweenSparksBase - (fireSize-1)*TicksBetweenSparksReductionPerFireSize;

			if( ticks < MinTicksBetweenSparks )
				ticks = MinTicksBetweenSparks;

			return ticks;
		}
	}
	private float TicksBeforeDamage
	{
		get
		{
			float ticks = BaseTicksBetweenDamage - fireSize * TicksBetweenDamageReductionPerFireSize;

			if(ticks < MinTicksBetweenDamage )
				ticks = MinTicksBetweenDamage;

			return ticks;
		}
	}
	public override Material DrawMat
	{
		get
		{
			return curDrawFrame;
		}
	}
	public override Vector3 DrawPos
	{
		get
		{
			return base.DrawPos + new Vector3(curDrawOffset.x,0,curDrawOffset.y) * fireSize;
		}
	}









	public override void ExposeData()
	{
		base.ExposeData();

		Scribe_Values.LookValue(ref fireSize, "fireSize");
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		RecalcPathsOnAndAroundMe();
		Find.ConceptTracker.TeachOpportunity(ConceptDefOf.HomeRegion, this, OpportunityType.Important );


		SoundInfo info = SoundInfo.InWorld(this, MaintenanceType.PerTick);
		sustainer = SustainerAggregatorUtility.AggregateOrSpawnSustainerFor(this, BurningSoundDef, info);
	}

	public float CurrentSize()
	{
		return fireSize;
	}

	public override void Destroy()
	{
		sustainer.externalParams.SizeAggregator.RemoveReporter(this);

		base.Destroy();
		RecalcPathsOnAndAroundMe();
	}

	private void RecalcPathsOnAndAroundMe()
	{
		foreach( IntVec3 offset in GenAdj.AdjacentSquaresAndInside )
		{
			IntVec3 sq = Position + offset;

			if( !sq.InBounds() )
				continue;

			Find.PathGrid.RecalculatePathCostAt(sq);
		}
	}

	
	public override void Tick()
	{
		sustainer.Maintain();

		//Do micro sparks
		if( fireSize > 0.7f )
		{
			if( Random.value < fireSize * 0.01f )
			{
				MoteMaker.ThrowMicroSparks(DrawPos);
			}
		}

		//Do smoke and glow
		ticksUntilSmoke--;
		if( ticksUntilSmoke <= 0 )
		{
			MoteMaker.ThrowSmoke( DrawPos, fireSize );

			if( parent == null )//No fire glow for moving things (it trails them)
			{
				Vector3 glowLoc = DrawPos + fireSize*( new Vector3(Random.value-0.5f,0,Random.value-0.5f)   );
				MoteMaker.ThrowFireGlow( glowLoc, fireSize );
			}

			float firePct = fireSize / 2;
			if( firePct > 1 ) firePct = 1;
			firePct = 1f-firePct;
			ticksUntilSmoke = SmokeIntervalRange.Lerped(firePct) + (int)(SmokeIntervalRandomAddon*Random.value);
		}





		ticksUntilFrameChange--;
		if( ticksUntilFrameChange <= 0 )
		{
			ticksUntilFrameChange = FrameChangeIntervalRange.RandomInRange;

			//Choose a new draw frame
			Material oldFrame = curDrawFrame;
			while( curDrawFrame == oldFrame )
			{
				curDrawFrame = def.folderDrawMats.RandomListElement();
			}

			//Apply random offset (it adds some life)
			const float MaxOff = 0.05f;
			curDrawOffset = new Vector3( Random.Range(-MaxOff,MaxOff),0,Random.Range(-MaxOff,MaxOff) );
		}


		//Static fires: Ignite pawns in my square
		if( parent == null )
		{
			if( fireSize > MinSizeForSquareIgnite )
			{
				float igniteChance = fireSize * SquareIgniteChancePerTickPerSize;
				foreach( Thing t in Find.ThingGrid.ThingsAt(Position).ToList() )
				{
					if( Random.value < igniteChance
						&& t.CanEverAttachFire() )
						t.TryIgnite(fireSize /2f);
				}
			}
		}


		//Perhaps I should unify the below with square ignition

		//Damage whatever I'm burning
		ticksSinceDamage++;
		if( ticksSinceDamage >= TicksBeforeDamage )
		{
			Thing damTarget = null;
			if( parent != null )
			{
				damTarget = parent;
			}
			else
			{
				//Burn random flammable thing in square
				List<Thing> burnables = Find.ThingGrid.ThingsAt(Position)
										.Where( t=>t.def.Flammable )
										.ToList();

				if( burnables.Count > 0 )
					damTarget = burnables.RandomListElement();

				//Destroy if nothing to burn in square
				if( damTarget == null )
				{
					Destroy();
					return;
				}
			}

			//If it's a mobile burner, we only ignite it when we are above minimum square ignite size
			if( !damTarget.CanEverAttachFire() || fireSize > MinSizeForSquareIgnite )
			{
				damTarget.TakeDamage( new DamageInfo( DamageTypeDefOf.Flame, 1, this ) );
			}


			ticksSinceDamage = 0;
		}
		

		//Emit sparks
		ticksSinceSpark++;
		if( ticksSinceSpark  >= TicksBeforeSpark )
		{
			ThrowSpark();
			ticksSinceSpark = 0;
		}

		//Try to grow the fire
		fireSize += FireBaseGrowthPerTick;

		if( fireSize > MaxFireSize )
			fireSize = MaxFireSize;

		//Extinguish from sky (rain etc)
		if( Find.WeatherManager.RainRate > 0.01f )
		{
			Thing building = Position.GetBuilding();
			bool roofHolderIsHere = building != null && building.def.holdsRoof;
			
 			if( roofHolderIsHere || !Find.RoofGrid.Roofed(Position) )
			{
				if( Random.value < BaseSkyExtinguishChance )
				{
					TakeDamage( new DamageInfo(DamageTypeDefOf.Extinguish, BaseSkyExtinguishDamage, null) );
				}
			}
		}
	}



	public override void Draw()
	{
		float drawSize = fireSize / 1.2f;
				
		if( drawSize > 1.2f )
			drawSize = 1.2f;

		Vector3 scaler = new Vector3( drawSize, 1, drawSize);
		Matrix4x4 matrix = new Matrix4x4();
		matrix.SetTRS( DrawPos, rotation.AsQuat, scaler);
				
		Graphics.DrawMesh(MeshPool.plane10, matrix, DrawMat, 0);
	}


	protected override void ApplyDamage(DamageInfo d)
	{
		if( d.Def == DamageTypeDefOf.Extinguish )
		{
			//One damage reduces fireSize by 0.01f
			fireSize -= d.Amount / 100f;

			if( fireSize <= MinFireSize )
			{
				Destroy();
				return;
			}
		}
	}
	
	//Spread randomly to one Thing in this or an adjacent square
	protected void ThrowSpark()
	{
		IntVec3 targLoc = Position;
		if( Random.value < 0.8f )
			targLoc = Position + GenRadial.ManualRadialPattern[ Random.Range (1,8+1) ];	//Spark adjacent
		else
			targLoc = Position + GenRadial.ManualRadialPattern[ Random.Range (10,20+1) ];	//Spark distant
		
		
		Spark sp = (Spark)GenSpawn.Spawn( ThingDef.Named("Spark"), Position );
		sp.Launch( this, new TargetPack(targLoc) );
	}

}
