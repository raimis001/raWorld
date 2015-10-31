using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class IncidentWorker_Eclipse : IncidentWorker
{
	public override bool TryExecute( IncidentParms parms )
	{
		if( Find.MapConditionManager.ConditionIsActive(MapConditionDefOf.Eclipse) )
			return false;

		int eclipseDuration = Mathf.RoundToInt(Random.Range(1.5f,2.5f) * DateHandler.TicksPerDay);
		Find.MapConditionManager.RegisterCondition( new MapCondition_Eclipse( eclipseDuration)  );

		string letterString = "EclipseIncident".Translate();
		Find.LetterStack.ReceiveLetter( new UI.Letter(letterString, UI.LetterType.BadNonUrgent));


		return true;
	}
}

