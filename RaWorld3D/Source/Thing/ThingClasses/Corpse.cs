using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Corpse : ThingWithComponents, ThoughtGiver
{
	//Config
	public Pawn sourcePawn = null;

	//Working vars
	private int timeOfDeath = -1000;

	//Constants
	private const float BaseButcherProductAmount = 90;

	//Properties
    public int Age{get{return Find.TickManager.tickCount - timeOfDeath;}}
	public override string Label
	{
		get
		{
			return sourcePawn.Label + " (dead)";
		}
	}




	public override void SpawnSetup()
	{
		base.SpawnSetup();

		if( timeOfDeath < 0 )
			timeOfDeath = Find.TickManager.tickCount;

		sourcePawn.rotation = IntRot.south; //Fixes drawing errors

		//Clearing out some data saves us from saveload errors, since dead pawns
		// are still kept around and saved by corpses
		//
		// note this doesn't happen in pawn.Destroy because that can cause little accessed nones in the
		// frame when a pawn is destroyed (e.g. when it exits the map).
		sourcePawn.prisoner = null;
		sourcePawn.jobs = null;
		sourcePawn.thinker = null;
		sourcePawn.pather = null;
	}


	public IEnumerable<Thing> ButcherProducts( Pawn butcher, float efficiency )
	{
		butcher.skills.Learn( SkillDefOf.Cooking, LearnRates.XpPerPawnSizeButchered*sourcePawn.def.race.bodySize );
		
		{
			Filth blood = (Filth)ThingMaker.MakeThing( DefDatabase<ThingDef>.GetNamed("FilthBlood") );
			blood.sources.Add( sourcePawn.Label );
			GenPlace.TryPlaceThing(blood, butcher.Position, ThingPlaceMode.Near );
		}

		if( sourcePawn.RaceDef.humanoid )
			butcher.psychology.thoughts.GainThought( ThoughtDef.Named("ButcheredHumanoidCorpse" ) );

		{
			Thing meat = ThingMaker.MakeThing( sourcePawn.def.race.meatDef );
			meat.stackCount = Mathf.RoundToInt(BaseButcherProductAmount * sourcePawn.def.race.bodySize * efficiency);
			yield return meat;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.LookValue( ref timeOfDeath, "timeOfDeath" );
		Scribe_Deep.LookDeep( ref sourcePawn, "sourcePawn" );
	}

	public override void DrawAt(Vector3 drawLoc)
	{
		//Don't draw in graves
		Building storeBuilding = this.StoringBuilding();
		if( storeBuilding != null && storeBuilding.def == ThingDefOf.Grave )
			return;

		sourcePawn.drawer.renderer.RenderPawnAt( drawLoc );
	}

	public Thought GiveObservedThought()
	{
		//Non-humanoid corpses never give thoughts
		if( !sourcePawn.RaceDef.humanoid )
			return null;

        Thing storingBuilding = this.StoringBuilding();
		if( storingBuilding == null )
		{
			//Laying on the ground
			return new Thought_Observation(ThoughtDef.Named("ObservedLayingCorpse"), this);
		}
        else if( storingBuilding.def.defName == "GibbetCage" )
        {
            if( sourcePawn.Faction == Faction.OfColony )
                return new Thought_Observation(ThoughtDef.Named("ObservedGibbetCageFullColonist"), this);
            else
                return new Thought_Observation(ThoughtDef.Named("ObservedGibbetCageFullStranger"), this);
        }

		return null;
	}

	public override string GetInspectString()
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.AppendLine("Faction".Translate() + ": " + sourcePawn.Faction);
		sb.AppendLine("DeadTime".Translate( Age.TicksInDaysString() ) );
		sb.AppendLine(base.GetInspectString());
		return sb.ToString();
	}
}
