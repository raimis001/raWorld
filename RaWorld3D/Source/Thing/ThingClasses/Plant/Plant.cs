using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Sound;
using AI;


public enum PlantLifeStage
{
	Sowing,
	Growing,
	Mature,
}



public class Plant : Thing, Edible
{
	//Components
	private PlantReproducer		reproducer;

	//Working vars
	public float 				growthPercent = 0.05f; //Start in growing phase by default, set to 0 if sowing
	private int					age = 0;
	private int					ticksSinceLit = 0;

	//Fast vars
	private List<int>			posIndexList = new List<int>();
	private Color32[]			workingColors = new Color32[4];

	//Constants and content
	public const float			BaseGrowthPercent = 0.05f;
	private const float			RotDamagePerTick = 1f/200f;
	private const int			MinFoodFromFoodYieldingPlants = 2;
	private const float			MaxAirPressureForDOT = 0.6f;
	private const float			SuffocationMaxDOTPerTick = 1 / 100f;
	private static readonly		Material MatSowing =  MaterialPool.MatFrom("Things/Plant/Plant_Sowing");
	private const float			GridPosRandomnessFactor = 0.30f;
	protected static readonly	SoundDef SoundHarvestReady = SoundDef.Named("HarvestReady");
	private const float			MinGrowthToEat = 0.8f;
	private const int			TicksWithoutLightBeforeRot = 50000; //2.5 days

	//Properties
	public bool HarvestableNow{get{return def.plant.Harvestable && growthPercent > MinGrowthToEat;}}
	public bool EdibleNow{get{return growthPercent > MinGrowthToEat;}}
	public bool Rotting
	{
		get
		{
			if( ticksSinceLit > TicksWithoutLightBeforeRot )
				return true;

			return def.plant.LimitedLifespan && age > def.plant.lifeSpan;
		}
	}
	private string GrowthPercentString
	{
		get
		{
			float adjGrowthPercent = (growthPercent*100);
			if( adjGrowthPercent > 100f )
				adjGrowthPercent = 100.1f;
			return adjGrowthPercent.ToString("##0");
		}
	}
	public override string LabelMouseover
	{
		get
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(def.label);
			sb.Append(" (" + "PercentGrowth".Translate(GrowthPercentString));
			
			if( Rotting )
				sb.Append(", " + "DyingLower".Translate() );
			
			sb.Append(")");

			return sb.ToString();
		}
	}
	private bool HasEnoughLightToGrow
	{
		get
		{
			return Find.GlowGrid.PsychGlowAt(Position) >= def.plant.minGlowToGrow;
		}
	}
	public override Material DrawMat
	{
		get
		{
			if( LifeStage == PlantLifeStage.Sowing )
				return MatSowing;

			return base.DrawMat;
		}
	}
	private float LocalFertility
	{
		get
		{
			return Find.FertilityGrid.FertilityAt(Position);
		}
	}
	public PlantLifeStage LifeStage
	{
		get
		{
			if( growthPercent < 0.001f )
				return PlantLifeStage.Sowing;

			if( growthPercent > 0.999f )
				return PlantLifeStage.Mature;

			return PlantLifeStage.Growing;
		}
	}



	public override void SpawnSetup()
	{
		base.SpawnSetup();

		if( reproducer == null && def.plant.seedEmitAveragePer20kTicks > 0 )
		{
			reproducer = new PlantReproducer(this);
		}


		//At mapgen, we set your age randomly
		//Note: this can lead to inconsistent states e.g. 10% growth and rotting
		if( Find.Map.generating  && def.plant.LimitedLifespan )
			age = Random.Range( 0, (def.plant.lifeSpan + 3000)); 


		//Store all the position indices
		for( int i=0; i<def.plant.maxMeshCount; i++ )
			posIndexList.Add(i);
		posIndexList.Shuffle();
	}
	
	
	public override void ExposeData()
	{
		base.ExposeData();
		
		Scribe_Values.LookValue(ref growthPercent, 	"growthPercent");
		Scribe_Values.LookValue(ref age, 			"age", 0);
		Scribe_Values.LookValue(ref ticksSinceLit,	"ticksSinceLit", 0 );
	}

	public float Eaten(float nutritionWanted, Pawn eater)
	{
		PlantCollected();
		return def.food.nutrition; //Does not take growth into account
	}


	public void PlantCollected()
	{
		if( def.plant.destroyOnHarvest )
		{
			Destroy();
		}
		else
		{
			growthPercent = 0.08f;
			Find.MapDrawer.MapChanged(Position, MapChangeType.Things);
		}
	}


	public override void TickRare()
	{
		bool hasLight = HasEnoughLightToGrow;

		//Record light starvation
		if( !hasLight )
			ticksSinceLit += DateHandler.TickRareInterval;
		else
			ticksSinceLit = 0;


		//Grow
		if( LifeStage == PlantLifeStage.Growing && hasLight )
		{
			float fertilityFactor = (LocalFertility*def.plant.fertilityFactorGrowthRate)
									+ (1f-def.plant.fertilityFactorGrowthRate);

			float timeOfDayFactor = 1f;
			if( DateHandler.CurDayPercent < 0.2f || DateHandler.CurDayPercent > 0.8f )
				timeOfDayFactor *= 0.5f;


			growthPercent += fertilityFactor
							* timeOfDayFactor
							* DateHandler.TickRareInterval
							* (def.plant.growthPer20kTicks / 20000f);

			if( LifeStage == PlantLifeStage.Mature )
			{
				//Newly matured
				if( !def.plant.GrowsWild )
					SoundHarvestReady.Play(Position);
			}
		}		

		//Age
		age += DateHandler.TickRareInterval;

		//Rot
		if( Rotting && def.plant.LimitedLifespan )
		{
			int rotDamAmount = Mathf.CeilToInt(RotDamagePerTick * DateHandler.TickRareInterval);
			TakeDamage( new DamageInfo( DamageTypeDefOf.Rotting, rotDamAmount, null ));	
		}
		
		//Reproduce
		if( !destroyed && reproducer != null )
			reproducer.PlantReproducerTickRare();
	}
	

	public int FoodYieldNow()
	{
		if( !HarvestableNow )
			return 0;

		if( def.plant.maxFoodYield <= 1 )
			return Mathf.RoundToInt(def.plant.maxFoodYield);

		float yieldFloat = def.plant.maxFoodYield;;
		yieldFloat *= ((float)health / (float)def.maxHealth);
		yieldFloat *= growthPercent;

		int yieldInt = Gen.RandomRoundToInt(yieldFloat);		
				
		//Food-yielding plants always give you a certain minimum food
		if( yieldInt < MinFoodFromFoodYieldingPlants )
			yieldInt = Mathf.Min( MinFoodFromFoodYieldingPlants, Gen.RandomRoundToInt(def.plant.maxFoodYield));	
			
		return yieldInt;
	}


	public override void PrintOnto( SectionLayer layer )
	{
		Profiler.BeginSample("Plant.EmitMeshPieces " + this);

		Vector3 trueCenter = this.TrueCenter();

		Profiler.BeginSample("Meshes");

		Random.seed = Position.GetHashCode();//So our random generator makes the same numbers every time

		//Determine random local position variance
		float positionVariance;
		if( def.plant.maxMeshCount == 1 )
			positionVariance = 0.05f;
		else
			positionVariance = 0.50f;

		//Determine how many meshes to print
		int meshCount = Mathf.CeilToInt( growthPercent * def.plant.maxMeshCount );
		if( meshCount < 1 )
			meshCount = 1;

		//Grid width is the square root of max mesh count
		int gridWidth = 1;
		switch( def.plant.maxMeshCount )
		{
			case 1: gridWidth = 1; break;
			case 4: gridWidth = 2; break;
			case 9: gridWidth = 3; break;
			case 16: gridWidth = 4; break;
			case 25: gridWidth = 5; break;
			default: Log.Error(def + " must have plant.MaxMeshCount that is a perfect square."); break;
		}
		float gridSpacing = 1f/gridWidth; //This works out to give half-spacings around the edges

		//Shuffle up the position indices and place meshes at them
		//We do this to get even mesh placement by placing them roughly on a grid
		Vector3 adjustedCenter = Vector3.zero;
		Vector2 planeSize = Vector2.zero;
		int meshesYielded = 0;
		int posCount = posIndexList.Count;
		for(int i=0; i<posCount; i++ )
		{		
			int posIndex = posIndexList[i];

			//Determine plane size
			float size = def.plant.visualSizeRange.LerpThroughRange(growthPercent);

			//Determine center position
			if( def.plant.maxMeshCount == 1 )
			{
				adjustedCenter = trueCenter + new Vector3(Random.Range(-positionVariance, positionVariance),
															 0,
															 Random.Range(-positionVariance, positionVariance) );

				//Clamp bottom of plant to square bottom
				//So tall plants grow upward
				float squareBottom = Mathf.Floor(trueCenter.z);
				if( (adjustedCenter.z - (size/2f)) <  squareBottom )
					adjustedCenter.z = squareBottom + (size/2f);
			}
			else
			{
				adjustedCenter = Position.ToVector3(); //unshifted
				adjustedCenter.y = def.altitude;//Set altitude

				//Place this mesh at its randomized position on the submesh grid
				adjustedCenter.x += 0.5f * gridSpacing;
				adjustedCenter.z += 0.5f * gridSpacing;
				int xInd = posIndex / gridWidth;
				int zInd = posIndex % gridWidth;
				adjustedCenter.x += xInd * gridSpacing;
				adjustedCenter.z += zInd * gridSpacing;
				
				//Add a random offset
				float gridPosRandomness = gridSpacing * GridPosRandomnessFactor;
				adjustedCenter += new Vector3(Random.Range(-gridPosRandomness, gridPosRandomness),
											  0,
											  Random.Range(-gridPosRandomness, gridPosRandomness) );
			}

			//Randomize horizontal flip
			bool flipped = Random.value < 0.5f;		

			//Randomize material
			Material mat = def.folderDrawMats.RandomListElement();

			//Set wind exposure value at each vertex by setting vertex color
			workingColors[1].a = workingColors[2].a = (byte)(255 * def.plant.topWindExposure);
			workingColors[0].a = workingColors[3].a = 0;

			if( def.overdraw )
				size += 2f;
			planeSize = new Vector2( size,size );
			Printer_Plane.PrintPlane( layer, 
									  adjustedCenter, 
									  planeSize,	
									  mat, 
									  flipUv: flipped, 
									  colors:  workingColors );


			meshesYielded++;
			if( meshesYielded >= meshCount )
				break;
		}

		Profiler.EndSample();

		Profiler.BeginSample("Shadow");

		if( def.sunShadowInfo != null)
		{
			//Brutal shadow positioning hack
			float			shadowOffsetFactor = 0.85f;
			if( planeSize.y < 1 )
				shadowOffsetFactor = 0.6f; //for bushes
			else
				shadowOffsetFactor = 0.81f;	//for cacti

			Vector3 sunShadowLoc = adjustedCenter;
			sunShadowLoc.z -= (planeSize.y/2f) * shadowOffsetFactor;
			sunShadowLoc.y -= Altitudes.AltInc;

			Printer_Shadow.PrintShadow( layer, sunShadowLoc, def.sunShadowInfo );
		}

		Profiler.EndSample();

		Profiler.EndSample();
	}



	
	public override string GetInspectString()
	{
		StringBuilder sb = new StringBuilder();

		sb.Append(base.GetInspectString());
		sb.AppendLine();
		
		sb.AppendLine( "PercentGrowth".Translate(GrowthPercentString));

		if( LifeStage == PlantLifeStage.Sowing )
		{
		}
		else if( LifeStage == PlantLifeStage.Growing )
		{
			if( !HasEnoughLightToGrow )		
				sb.AppendLine("NotGrowingNow".Translate(def.plant.minGlowToGrow.HumanName().ToLower()) );
			else
				sb.AppendLine("Growing".Translate());
			

			int growTicksLeft = (int)((1f-growthPercent) * (20000f/def.plant.growthPer20kTicks));

            if( def.plant.Harvestable )
			    sb.AppendLine("HarvestableIn".Translate( growTicksLeft.TicksInDaysString() ));
            else
                sb.AppendLine("FullyGrownIn".Translate( growTicksLeft.TicksInDaysString() ));


		}
		else if( LifeStage == PlantLifeStage.Mature )
		{
			if( def.plant.Harvestable )
				sb.AppendLine("ReadyToHarvest".Translate() );
			else
				sb.AppendLine("Mature".Translate() );
		}
		
		return sb.ToString();
	}
	
	public void CropBlighted()
	{
		if( Random.value < 0.85f )
			Destroy();
	}

	public override void Destroy()
	{
		base.Destroy();

		Find.DesignationManager.RemoveAllDesignationsOn(this);
	}

}
