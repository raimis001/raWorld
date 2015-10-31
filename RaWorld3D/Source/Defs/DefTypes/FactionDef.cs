using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;






public class SpawnGroup
{
	public int 					selectionWeight;
	public int					cost;
	public List<PawnKindDef>	kinds = new List<PawnKindDef>();
}





public class FactionDef : Def
{
	public bool					makeOnGameStart = true;
	public NameMakerDef			factionNameMaker;
	public NameMakerDef			pawnNameMaker;
	public TechLevel			techLevel = TechLevel.Undefined;
	public string				backstoryCategory = null;
	public List<SpawnGroup>		spawnGroups = null;
	public List<string>			hairTags = new List<string>();
	public bool					hidden = false;

	public FloatRange			startingRelations = new FloatRange(-90, 60);
	public FloatRange			naturalColonyGoodwill = new FloatRange(-50, 50);
	public float				goodwillDailyGain = 1f;
	public float				goodwillDailyFall = 1f;




	public static FactionDef Named( string defName )
	{
		return DefDatabase<FactionDef>.GetNamed(defName);
	}

	public override IEnumerable<string> ConfigErrors()
	{
		foreach( var error in base.ConfigErrors() )
			yield return error;

		if( factionNameMaker == null )
			yield return "FactionTypeDef " + defName + " lacks a nameMaker.";

		if( techLevel == TechLevel.Undefined )
			yield return defName + " has no tech level.";

		if( backstoryCategory == null )
			yield return defName + " has no backstory category.";

		if( hairTags.Count == 0 )
			yield return defName + " has no hairTags.";

	}
}

