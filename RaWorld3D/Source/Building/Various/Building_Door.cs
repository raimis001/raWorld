using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Sound;


public class Building_Door : Building
{
	//Housekeeping
	public CompPowerTrader	powerComp;

	//Working vars
	public bool 			isOpen = false;
	protected int 			ticksUntilClose = 0;
	protected int 			visualTicksOpen = 0;

	//Constants
	private const int		AutomaticCloseDelayTicks = 60;
	private const int		VisualOpenTicksMax = 10;
	private const float		VisualDoorOffsetStart = 0.25f;
	private const float		VisualDoorOffsetEnd = 0.75f;


	//Properties	
	public bool CloseBlocked
	{
		get
		{
			foreach( Thing t in Find.ThingGrid.ThingsAt(Position) )
			{
				//Don't close on items or pawns
				if(    t.def.category == EntityCategory.Pawn
					|| t.def.category == EntityCategory.Item )
					return true;
			}

			return false;
		}
	}
	public bool DoorPowerOn
	{
		get
		{
			return powerComp != null && powerComp.PowerOn;
		}
	}




	public override void SpawnSetup()
	{
		base.SpawnSetup();

		powerComp = GetComp<CompPowerTrader>();

		//Doors default to having a metal tile underneath
		//GenSpawn.Spawn("Floor_MetalTile", Position);
	}

	public override void Tick()
	{
		base.Tick ();

		if( !isOpen )
		{
			//Visual - slide door closed
			if( visualTicksOpen > 0 )
				visualTicksOpen--;
		}

		if( isOpen )
		{
			//Visual - slide door open
			if( visualTicksOpen < VisualOpenTicksMax )
				visualTicksOpen++;

			//Count down to closing
			if( Find.ThingGrid.SquareContains( Position, EntityType.Pawn ) )
				ticksUntilClose = AutomaticCloseDelayTicks;
			else
			{
				ticksUntilClose--;

				//If the power is on, close automatically
				if( DoorPowerOn && ticksUntilClose <= 0 )
					DoorTryClose();
			}
		}
	}

	public void Notify_PawnApproaching( Pawn p )
	{
		//Open automatically before pawn arrives
		if( WillOpenFor(p) && DoorPowerOn )
			DoorOpen();
	}

	public bool WillOpenFor( Pawn p )
	{
		return AI.GenAI.MachinesLike(Faction, p);
	}
	
	public override bool BlocksPawn( Pawn p )
	{
		if( isOpen )
			return false;
		else
			return !WillOpenFor(p);
	}

	protected void DoorOpen()
	{
		isOpen = true;
		ticksUntilClose = AutomaticCloseDelayTicks;

		if( DoorPowerOn )
			def.building.doorOpenSoundPowered.Play(Position);
		else
			def.building.doorOpenSoundManual.Play(Position);
	}


	protected void DoorTryClose()
	{
		if( CloseBlocked )
			return;

		isOpen = false;

		if( DoorPowerOn )
			def.building.doorCloseSoundPowered.Play(Position);
		else
			def.building.doorCloseSoundManual.Play(Position);
	}

		
	public void StartManualOpenBy( Pawn opener )
	{
		DoorOpen();
	}

	public void StartManualCloseBy( Pawn closer )
	{
		DoorTryClose();
	}

	public override void Draw()
	{
		//Note: It's a bit odd that I'm changing game variables in Draw
		//      but this is the easiest way to make this always look right even if
		//      conditions change while the game is paused.
		rotation = DoorRotationAt(Position);

		//Draw the two moving doors
		float pctOpen = (float)visualTicksOpen / (float)VisualOpenTicksMax;			
		float doorOffset = VisualDoorOffsetStart + (VisualDoorOffsetEnd-VisualDoorOffsetStart)*pctOpen;	
		for( int i=0; i<2; i++ )
		{
			Vector3 doorPos =  DrawPos;
			doorPos.y = Altitudes.AltitudeFor(AltitudeLayer.DoorMoveable);
			
			Vector3 offsetNormal = new Vector3();
			if( i == 0 )
				offsetNormal = new Vector3(0,0,1);
			else
				offsetNormal = new Vector3(0,0,-1);
			
			IntRot openDir = rotation;
			openDir.Rotate(RotationDirection.Clockwise);
			
			offsetNormal  = openDir.AsQuat * offsetNormal;
				
			doorPos += offsetNormal * doorOffset;
		
			Vector3 Scaler = new Vector3( 0.5f, 0.5f, 0.5f);
			Matrix4x4 Matrix = new Matrix4x4();
			Matrix.SetTRS(doorPos, openDir.AsQuat, Scaler);
			Graphics.DrawMesh(MeshPool.plane10, Matrix, def.drawMat, 0 );
			
		}
			
		Comps_Draw();
	}


	private static int AlignQualityAgainst( IntVec3 sq )
	{
		if( !sq.InBounds() )
			return 0;

		//We align against anything unwalkthroughable and against blueprints for unwalkthroughable things
		if( !sq.Walkable() )
			return 9;
			

		List<Thing> things = Find.ThingGrid.ThingsListAt(sq);
		for(int i=0; i<things.Count; i++ )
		{
			Thing t = things[i];

			if( t.def.eType == EntityType.Door )
				return 1;

			Thing blue = t as Blueprint;
			if( blue != null )
			{
				if( blue.def.entityDefToBuild.passability == Traversability.Impassable )
					return 9;
				if( blue.def.eType == EntityType.Door )
					return 1;
			}
		}
			
		return 0;		
	}


	public static IntRot DoorRotationAt(IntVec3 loc)
	{
		int horVotes = 0;
		int verVotes = 0;

		horVotes += AlignQualityAgainst( loc + IntVec3.east );
		horVotes += AlignQualityAgainst( loc + IntVec3.west );
		verVotes += AlignQualityAgainst( loc + IntVec3.north );
		verVotes += AlignQualityAgainst( loc + IntVec3.south );

		if( horVotes >= verVotes )
			return IntRot.north;
		else
			return IntRot.east;
	}	
}


