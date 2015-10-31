using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;




public class Building : ThingWithComponents
{
	//Working vars - leavings
	public LeavingMode		destroyMode = LeavingMode.Deconstructed;

	//Working vars - power transmission
	public PowerNet			powerNet = null; //Never saved or cleared
	public Building			connectedToTransmitter = null;
	public List<Building>	connectees = null;
	


	//Properties
	public virtual bool TransmitsPower { get { return def.transmitsPower; } }
	public PowerNet		ConnectedToNet
	{
		get
		{
			if( connectedToTransmitter == null )
				return null;

			return PowerNetGrid.TransmittedPowerNetAt( connectedToTransmitter.Position );
		}
	}

	public override Mesh DrawMesh
	{
		get
		{
			IntVec2 drawSize = def.size;

			if( def.overdraw )
			{
				drawSize.x += 2;
				drawSize.z += 2;
			}

			return MeshPool.gridPlanes[ drawSize.x, drawSize.z ];
		}
	}
	public IntVec3 InteractionSquare
	{
		get
		{
			return InteractionSquareWhenAt(def, Position, rotation);
		}
	}



	
	public override void SpawnSetup()
	{
		base.SpawnSetup();
		
		Find.ListerBuildings.Add( this );	
		
		if (TransmitsPower)
			PowerNetManager.Notify_TransmitterSpawned(this);

		if( def.ConnectToPower )
			PowerNetManager.Notify_ConnectorSpawned(this);

		//Remake terrain meshes with new underwall under me
		if( def.coversFloor )
			Find.MapDrawer.MapChanged(Position, MapChangeType.Terrain);

		if (TransmitsPower || def.ConnectToPower)
			Find.MapDrawer.MapChanged(Position, MapChangeType.PowerGrid, true, false);

		foreach( IntVec3 sq in GenAdj.SquaresOccupiedBy(this) )
		{
			Find.MapDrawer.MapChanged( sq, MapChangeType.Buildings );
			Find.GlowGrid.MarkGlowGridDirty(sq);
		}

		if( def.category == EntityCategory.Building )
			Find.BuildingGrid.Register(this);

		if( Faction == Faction.OfColony )
		{
			if( def.building != null && def.building.spawnedConceptLearnOpportunity != null )
			{
				Find.ConceptTracker.TeachOpportunity( def.building.spawnedConceptLearnOpportunity, OpportunityType.GoodToKnow );
			}
		}
	}

	public override void DeSpawn()
	{
		base.DeSpawn();

		if( def.category == EntityCategory.Building )
			Find.BuildingGrid.DeRegister(this);

		foreach( IntVec3 sq in GenAdj.SquaresOccupiedBy(this) )
		{
			MapChangeType changeType = MapChangeType.Buildings;

			if( def.coversFloor ) //Because floor covering buildings affect how the terrain looks
				changeType |= MapChangeType.Terrain;

			Find.Map.mapDrawer.MapChanged( sq, changeType );

			Find.GlowGrid.MarkGlowGridDirty(sq);
		}
	}

	public override void Killed( DamageInfo d )
	{
		destroyMode = LeavingMode.Killed;
	
		base.Killed(d);
	}

	public override void Destroy()
	{
		base.Destroy();
		
		GenLeaving.DoLeavingsFor(this, destroyMode);

        if( def.MakeFog )
            Find.FogGrid.Notify_FogBlockerDestroyed(Position);

		if( def.holdsRoof )
			RoofCollapseChecker.Notify_RoofHolderDestroyed(this);

		if( def.leaveTerrain != null && Map.Initialized )
		{
			foreach( IntVec3 loc in GenAdj.SquaresOccupiedBy(this) )
				Find.TerrainGrid.SetTerrain(loc, def.leaveTerrain);
		}

		Find.ListerBuildings.Remove(this);		
		
		if( TransmitsPower || def.ConnectToPower )
		{
			if (TransmitsPower)
				PowerNetManager.Notify_TransmitterDespawned(this);

			if( def.ConnectToPower )
				PowerNetManager.Notify_ConnectorDespawned(this);

			Find.MapDrawer.MapChanged(Position, MapChangeType.PowerGrid, true, false);
		}
	}


	public override void Draw()
	{
		if( health < def.maxHealth && def.useStandardHealth )
			OverlayDrawer.DrawOverlay(this, OverlayTypes.Damaged);

		//If we've already added to the map mesh don't bother with drawing our base mesh
		if( def.drawerType == DrawerType.RealtimeOnly )
			base.Draw();
		
		Comps_Draw();
	}

		
	public override void PrintOnto( SectionLayer layer )
	{
		//Yield power wires connecting me to my transmitter
		if( connectedToTransmitter != null )
			PowerNetGraphics.PrintWirePieceConnecting( layer, this, connectedToTransmitter, false);

		base.PrintOnto(layer);
	}

	public void PrintForPowerGrid( SectionLayer layer )
	{
		//Transmitter stuff
		if (TransmitsPower)
		{
			foreach( IntVec3 sq in GenAdj.SquaresOccupiedBy(this) )
			{
				//Transmission lines
				LinkDrawers.transmitterOverlay.PrintOnto(layer, this, sq);
			}
		}


		//The connector base (small blue circle)
		if( def.ConnectToPower )
		{
			PowerNetGraphics.PrintOverlayConnectorBaseFor( layer, this );
		}

		//The connector wire
		if( connectedToTransmitter != null )
		{
			PowerNetGraphics.PrintWirePieceConnecting( layer, this, connectedToTransmitter, true );
		}
	}


	public static IntVec3 InteractionSquareWhenAt( EntityDef tDef, IntVec3 loc, IntRot rot )
	{	
		IntVec3 rotatedOffset = tDef.interactionSquareOffset.RotatedBy(rot);		
		return loc + rotatedOffset;
	}



	
}




