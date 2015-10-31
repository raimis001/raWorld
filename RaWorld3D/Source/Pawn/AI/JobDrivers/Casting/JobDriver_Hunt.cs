using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AI{
public class JobDriver_Hunt : JobDriver
{
	//Constants
	private const TargetIndex VictimInd = TargetIndex.A;
	private const TargetIndex CorpseInd = TargetIndex.A;
	private const TargetIndex StoreSquareInd = TargetIndex.B;



	//Ctor
	public JobDriver_Hunt(Pawn pawn) : base(pawn){}


	protected override IEnumerable<Toil> MakeNewToils()
	{
		Toil startCollectCorpse = Toils_General.Wait(10);

		yield return Toils_Reserve.ReserveTarget(VictimInd, ReservationType.Total );

		yield return Toils_Combat.SetJobToUseToBestAttackVerb();

		Toil gotoCastPos = Toils_Combat.GotoCastPosition( VictimInd )
									   .JumpIfDespawned( VictimInd, startCollectCorpse );
		yield return gotoCastPos;

		Toil jumpIfCannotHit =  Toils_Jump.JumpIfCannotHitTarget( VictimInd, gotoCastPos );
		yield return jumpIfCannotHit;

		yield return Toils_General.Wait(2)
								 .JumpIfDespawned( VictimInd, startCollectCorpse );

		yield return Toils_Combat.CastVerb( VictimInd )
								 .JumpIfDespawned( VictimInd, startCollectCorpse );

		yield return Toils_Jump.Jump( jumpIfCannotHit );


		//================================================
		//============= Collect corpse ===================
		//================================================
		yield return startCollectCorpse;

		//----------------------------------------------------
		//Rearrange the job so the bill doer goes and stores the product
		//----------------------------------------------------
		Toil transformJobForCorpseStore = new Toil();
		transformJobForCorpseStore.initAction = ()=>
			{
				Pawn actor = transformJobForCorpseStore.actor;

				//Hack way of finding a ref to the corpse
				Corpse corpse = null;
				Thing targPawn = actor.CurJob.GetTarget( VictimInd ).Thing;
				foreach( Thing t in targPawn.Position )
				{
					corpse = t as Corpse;
					if( corpse != null && corpse.sourcePawn == targPawn )
						break;
				}

				corpse.SetForbidden(false);

				//Try find a store square
				IntVec3 storeSquare;
				if( corpse != null
					&& StoreUtility.TryFindBestStoreSquareFor( corpse, StoragePriority.Unstored, out storeSquare ) )
				{
					actor.CurJob.targetB = storeSquare;
					actor.CurJob.SetTarget(CorpseInd, corpse);
					actor.CurJob.maxNumToCarry = 1;
					actor.CurJob.haulMode = HaulMode.ToSquareStorage;
				}
				else
				{
					//No store square? We're done.
					actor.jobs.EndCurrentJob( JobCondition.Succeeded );
					return;
				}
			};
		yield return transformJobForCorpseStore;

		yield return Toils_Goto.GotoLoc( CorpseInd, PathMode.ClosestTouch )
										.FailOnDespawnedOrForbidden( CorpseInd );

		yield return Toils_Haul.StartCarryThing( CorpseInd );

		Toil carryToSquare = Toils_Haul.CarryHauledThingToSquare(TargetIndex.B);
		yield return carryToSquare;

		yield return Toils_Haul.PlaceHauledThingInSquare(TargetIndex.B, carryToSquare);
	}
}}

