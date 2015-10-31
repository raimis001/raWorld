using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

using AI;




public static class AnimalInsanityUtility
{
	public static float PointsPerAnimal( ThingDef animalDef )
	{
		float cost = 10;

		cost += (animalDef.race.meleeDamage*1.8f) + (animalDef.maxHealth * 0.23f);

		 //Bias up boomrats for fire explosive
		if( animalDef.label == "Boomrat" )
			cost += 30;

		return cost;
	}
}



//Special case of AnimalInsanity
public class IncidentWorker_AnimalInsanitySingle : IncidentWorker
{
	private const int FixedPoints = 30; //one squirrel

	public override bool TryExecute( IncidentParms parms )
	{
		int maxPoints = 150;
		if( Find.TickManager.tickCount < 20000*10 )
			maxPoints = 40;

		//Choose an animal type
		List<Pawn> validAnimals = Find.ListerPawns.AllPawns
							.Where( p => !p.RaceDef.humanoid && AnimalInsanityUtility.PointsPerAnimal(p.def) <= maxPoints)
							.ToList();

		if( validAnimals.Count == 0 )
			return false;

		Pawn animal = validAnimals.RandomListElement();
		PsychologyUtility.TryDoMentalBreak(animal, SanityState.Psychotic);

		string letter;
		letter = "AnimalInsanitySingle".Translate(  animal.Label.ToLower() );
		Find.LetterStack.ReceiveLetter( new UI.Letter(letter, UI.LetterType.BadUrgent, animal) );
		return true;
	}
}



public class IncidentWorker_AnimalInsanity : IncidentWorker
{
	public override bool TryExecute( IncidentParms parms )
	{
		if( parms.points <= 0 )
		{
			Log.Error("AnimalInsanity running without points.");
			parms.points = (int)(Find.StoryWatcher.watcherStrength.StrengthRating * 50);
		}


		//Choose an animal type
		List<ThingDef> animalDefs = DefDatabase<ThingDef>.AllDefs
											.Where( def => def.category == EntityCategory.Pawn
															&& !def.race.humanoid 
															&& AnimalInsanityUtility.PointsPerAnimal(def) <= parms.points)
											.ToList();

		//Remove all animal types for whom less than 3 are on the map
		animalDefs.RemoveAll( d=> Find.ListerPawns.AllPawns.Where(p=>p.def == d).Count() < 3 );

		if( animalDefs.Count == 0 )
			return false;

		ThingDef animalDef = animalDefs.RandomListElement();

		List<Pawn> allUsableAnimals = Find.ListerPawns.AllPawns
												.Where(p=>p.def == animalDef )
												.ToList();

		float pointsPerAnimal = AnimalInsanityUtility.PointsPerAnimal( animalDef );
		float pointsSpent = 0;
		int animalsMaddened = 0;
        Pawn lastAnimal = null;
		foreach( Pawn animal in allUsableAnimals.InRandomOrder() )
		{
			if( pointsSpent+pointsPerAnimal > parms.points )
				break;

			PsychologyUtility.TryDoMentalBreak(animal, SanityState.Psychotic);

			pointsSpent += pointsPerAnimal;
			animalsMaddened++;
            lastAnimal = animal;
		}

		//Not enough points/animals for even one animal to be maddened
		if( pointsSpent == 0 )
			return false;

		string letter;
		if( animalsMaddened == 1 )
			letter = "AnimalInsanitySingle".Translate(  animalDef.label.ToLower() );
		else
			letter = "AnimalInsanityMultiple".Translate(  animalDef.label.ToLower() );

		Find.LetterStack.ReceiveLetter( new UI.Letter(letter, UI.LetterType.BadUrgent, lastAnimal) );

		return true;
	}
}