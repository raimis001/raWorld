using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



namespace AI{
public class JobDriver_Ignite : JobDriver
{
	public JobDriver_Ignite(Pawn pawn) : base(pawn){}



	protected override IEnumerable<Toil> MakeNewToils()
	{
		yield return Toils_Goto.GotoThing( TargetIndex.A, PathMode.Touch )
								.FailOnBurningImmobile( TargetIndex.A );

		Toil ignite = new Toil();
		ignite.initAction = ()=>
		{
			pawn.natives.TryIgnite( TargetThingA );
		};
		yield return ignite;
	}
}}	

