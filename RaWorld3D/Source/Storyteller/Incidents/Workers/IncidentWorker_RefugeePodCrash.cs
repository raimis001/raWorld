using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



public class IncidentWorker_RefugeePodCrash : IncidentWorker
{
	private const float FogClearRadius = 4.5f;



	public override bool TryExecute( IncidentParms parms )
	{
		IntVec3 dropSpot = GenSquareFinder.RandomSquareWith( (sq)=>sq.Standable() && !sq.IsFogged() );	

		Find.LetterStack.ReceiveLetter( new UI.Letter("RefugeePodCrash".Translate(), UI.LetterType.BadNonUrgent, dropSpot));

		Faction fac = Find.FactionManager.FirstFactionOfDef( FactionDef.Named("Spacer") );

		Pawn refugee = PawnGenerator.GeneratePawn( PawnKindDef.Named("SpaceRefugee"), fac );
		refugee.healthTracker.ForceIncap();

		DropPodInfo contents = new DropPodInfo(refugee, 180);			
		DropPodUtility.MakeDropPodAt( dropSpot, contents );	

		Find.Storyteller.intenderPopulation.Notify_PopulationGainIncident();
		return true;
	}
}
