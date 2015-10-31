using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



namespace AI{

public class JobDriver_Wait : JobDriver
{
	//Constants
	private const int TargetSearchInterval = 4;


	public JobDriver_Wait(Pawn pawn) : base(pawn){}


	protected override IEnumerable<Toil> MakeNewToils()
	{
		Toil wait = new Toil();
		wait.initAction = ()=>
		{
			Find.PawnDestinationManager.ReserveDestinationFor(pawn, pawn.Position);

			pawn.pather.StopDead();
		};
		wait.tickAction = ()=>
		{
			if( (Find.TickManager.tickCount + pawn.thingIDNumber) % TargetSearchInterval != 0 )
				return;

			if( pawn.story == null || !pawn.story.WorkTagIsDisabled(WorkTags.Violent) )
			{
				//Melee attack adjacent enemy pawns
				//Barring that, put out fires
				foreach( IntVec3 neigh in GenAdj.AdjacentSquares8WayAndInside(pawn.Position) )
				{
					Fire foundFire = null;
					foreach(Thing t in Find.ThingGrid.ThingsAt(neigh) )
					{
						Pawn p = t as Pawn;
						if( p != null && pawn.HostileTo(p) && !p.Incapacitated )
						{
							pawn.natives.TryMeleeAttack(p);
							return;
						}

						//Note: It checks our position first, so we keep our first found fire
						//This way, we prioritize a fire we're standing on
						Fire f = t as Fire;
						if( f != null && foundFire == null)
							foundFire = f;
					}

					if( foundFire != null )
						pawn.natives.TryBeatFire( foundFire );
				}

				//Shoot at the closest enemy in range
				if( pawn.equipment != null && pawn.equipment.primary != null )
				{
					//We increase the range because we can hit targets slightly outside range by shooting at their ShootableSquares
					//We could just put the range at int.MaxValue but this is slightly more optimized so whatever
					Thing curTarg = pawn.ClosestReachableEnemyTarget
						(validator:			null, 
						maxDistance:		pawn.equipment.primary.verb.verbProps.range,
						needsLOStoDynamic:	true,
						needsLOStoStatic:	true );
			
					if( curTarg != null )
					{
						pawn.equipment.TryStartAttack( curTarg );
						return;
					}
				};
			}


		};
		wait.defaultCompleteMode = ToilCompleteMode.Never;

		yield return wait;
	}
}}



