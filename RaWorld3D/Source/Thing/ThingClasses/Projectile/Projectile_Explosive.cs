using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class Projectile_Explosive : Projectile
{
	private int ticksToDetonation = 0;

	public override void ExposeData()
	{
		base.ExposeData();

		Scribe_Values.LookValue(ref ticksToDetonation, "ticksToDetonation");
	}


	public override void Tick()
	{
		base.Tick();
		
		if( ticksToDetonation > 0 )
		{
			ticksToDetonation--;
			
			if( ticksToDetonation <= 0 )
				Explode();
		}
	}
	
	protected override void Impact(Thing HitThing)
	{
		if( def.projectile.explosionDelay == 0 )
		{
			Explode();
			return;
		}
		else
		{
			landed = true;
			ticksToDetonation = def.projectile.explosionDelay;
		}
	}	
	
	protected virtual void Explode()
	{
		Destroy();
		Explosion.DoExplosion( Position, 
								def.projectile.explosionRadius, 
								def.projectile.damageDef, 
								launcher );
	}
}
