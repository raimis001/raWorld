using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UI;


public class IncidentWorker_MiracleHeal : IncidentWorker
{
	public override bool TryExecute(IncidentParms parms)
	{
		Pawn pawn;
		if( Find.ListerPawns.FreeColonists.Where( col=>col.Incapacitated ).TryRandomElement(out pawn) )
		{
			int healthAmount = pawn.healthTracker.MaxHealth - pawn.healthTracker.Health - Random.Range(2,10);

			pawn.TakeDamage( new DamageInfo( DamageTypeDefOf.Healing, healthAmount, null) );

			Letter letter = new Letter( "MiracleHeal".Translate(pawn.Name.nick), LetterType.BadNonUrgent, pawn );
			Find.LetterStack.ReceiveLetter( letter );

			return true;
		}

		return true;
	}
}

