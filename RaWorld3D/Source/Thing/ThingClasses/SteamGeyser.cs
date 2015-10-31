using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



class SteamGeyser : Thing
{
	//Components
	private IntermittentSteamSprayer steamSprayer;

	//Working vars
	public Building harvester = null;	//set externally


	public override void SpawnSetup()
	{
		base.SpawnSetup();

		steamSprayer = new IntermittentSteamSprayer(this);
	}

	public override void Tick()
	{
		if( harvester == null )
			steamSprayer.SteamSprayerTick();
	}
}



public class IntermittentSteamSprayer
{
	//Housekeeping
	private Thing parent;

		//Working vars
	int ticksUntilSpray = MinTicksBetweenSprays;
	int sprayTicksLeft = 0;

	//Constants
	private const int MinTicksBetweenSprays = 500;
	private const int MaxTicksBetweenSprays = 2000;
	private const int MinSprayDuration = 200;
	private const int MaxSprayDuration = 500;
	private const float SprayThickness = 0.6f;

	public IntermittentSteamSprayer(Thing parent)
	{
		this.parent = parent;
	}

	public void SteamSprayerTick()
	{

		if( sprayTicksLeft > 0 )
		{
			sprayTicksLeft--;

			//Do spray effect
			if( Random.value < SprayThickness )					
				MoteMaker.ThrowAirPuffUp( parent.TrueCenter(), AltitudeLayer.Projectile );	

			//Done spraying
			if( sprayTicksLeft <= 0 )
				ticksUntilSpray = Random.Range( MinTicksBetweenSprays, MaxTicksBetweenSprays+1 );
		}
		else
		{
			ticksUntilSpray--;

			if( ticksUntilSpray <= 0 )
			{
				//Start spray
				sprayTicksLeft = Random.Range( MinSprayDuration, MaxSprayDuration+1 );
			}
		}
	}
}

