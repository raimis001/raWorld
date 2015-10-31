using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;



namespace AI{
public class JobDriver_HaulToStorageSquare : JobDriver
{
	public JobDriver_HaulToStorageSquare(Pawn pawn) : base(pawn){}
	

	public override string GetReport()
	{
		IntVec3 destLoc = pawn.jobs.curJob.targetB.Loc;

		Thing hauledThing = null;
		if( pawn.carryHands.CarriedThing != null )
			hauledThing = pawn.carryHands.CarriedThing;
		else
			hauledThing = TargetThingA;

		string destName = null;
		SlotGroup destGroup = StoreUtility.ContainingSlotGroup(destLoc);
		if( destGroup != null )
			destName = destGroup.parent.SlotYielderLabel();

		string repString;
		if( destName != null )
			repString = "ReportHaulingTo".Translate( hauledThing.Label, destName);
		else
			repString = "ReportHauling".Translate( hauledThing.Label );

		return repString;
	}
	
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//A: haul thing
		//B: destination loc

		//Set fail conditions
		this.FailOnDestroyed(TargetIndex.A);
		this.FailOnBurningImmobile( TargetIndex.B );
		//Note we only fail on forbidden if the target doesn't start that way
		//This helps haul-aside jobs on forbidden items
		if( !TargetThingA.IsForbidden() )
			this.FailOnForbidden(TargetIndex.A);


		//Reserve targets
		yield return Toils_Reserve.ReserveTarget( TargetIndex.B, ReservationType.Store );

		Toil reserveTargetA = Toils_Reserve.ReserveTarget( TargetIndex.A, ReservationType.Total );
		yield return reserveTargetA;

		Toil toilGoto = null;
		toilGoto = Toils_Goto.GotoThing( TargetIndex.A, PathMode.ClosestTouch )
			.FailOn( ()=>
			{
				//Note we don't fail on losing hauling designation
				//Because that's a special case anyway

				//If hauling to square storage, ensure storage dest is still valid
				Pawn actor = toilGoto.actor;
				Job curJob = actor.jobs.curJob;
				if( curJob.haulMode == HaulMode.ToSquareStorage )
				{
					Thing haulThing = curJob.GetTarget(TargetIndex.A).Thing;

					IntVec3 destLoc = actor.jobs.curJob.GetTarget(TargetIndex.B).Loc;
					if(!destLoc.IsValidStorageFor( haulThing)  )
						return true;
				}

				return false;
			});
		yield return toilGoto;


		yield return Toils_Haul.StartCarryThing(TargetIndex.A);

		if( CurJob.haulOpportunisticDuplicates )
			yield return Toils_Haul.CheckForGetOpportunityDuplicate( reserveTargetA );

		Toil carryToSquare = Toils_Haul.CarryHauledThingToSquare(TargetIndex.B);
		yield return carryToSquare;
		 
		yield return Toils_Haul.PlaceHauledThingInSquare(TargetIndex.B, carryToSquare);
	}

}}
