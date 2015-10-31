using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IncidentWorker_CropBlight : IncidentWorker
{
	public override bool TryExecute( IncidentParms parms )
	{
		bool cropFound = false;
		foreach( Plant plant in Find.Map.listerThings.ThingsInGroup( ThingRequestGroup.CultivatedPlant ) )
		{
		//	Plant plant = (Plant)t;

			if( plant.LifeStage == PlantLifeStage.Growing || plant.LifeStage == PlantLifeStage.Mature )
				cropFound = true;;
		}

		if( !cropFound )
			return false;


		List<Thing> plants = Find.Map.listerThings.ThingsInGroup( ThingRequestGroup.CultivatedPlant ).ToList();
		for( int i=plants.Count-1; i>=0; i-- )
		{
			((Plant)plants[i]).CropBlighted();
		}

		Find.LetterStack.ReceiveLetter( new UI.Letter("CropBlight".Translate(), UI.LetterType.BadNonUrgent) );

		return true;
	}
}

