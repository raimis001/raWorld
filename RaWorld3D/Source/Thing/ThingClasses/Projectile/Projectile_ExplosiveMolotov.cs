using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Projectile_ExplosiveMolotov : Projectile_Explosive
{
	protected override void Explode()
	{
		//First place burnable fuel on the ground
		for( int i=0; i<5; i++ )
		{
			IntVec3 sq = Position + GenRadial.RadialPattern[i];


			LiquidFuel existingFuel = Find.ThingGrid.ThingAt<LiquidFuel>(sq);
			if( existingFuel != null )
				existingFuel.Refill();
			else
				GenSpawn.Spawn( ThingDef.Named("Puddle_Fuel"), sq );
		}

		base.Explode();
	}

}



public class LiquidFuel : Thing
{
	//Working vars
	private int spawnTick;

	//Constants
	private const int DryOutTime = 1500;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.LookValue(ref spawnTick, "spawnTick");
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		spawnTick = Find.TickManager.tickCount;
	}

	public void Refill()
	{
		spawnTick = Find.TickManager.tickCount;
	}

	public override void Tick()
	{
		if( spawnTick + DryOutTime < Find.TickManager.tickCount )
			Destroy();
	}

}