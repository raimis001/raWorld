using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UI;


public class CompForbiddable : ThingComp
{
	//Working vars
	public bool forbidden = false;
	
	//Constants
	private static readonly Texture2D ButtonIconForbidden = ContentFinder<Texture2D>.Get("UI/Commands/Forbidden");
	
	public override void CompExposeData()
	{
		Scribe_Values.LookValue(ref forbidden, "forbidden", false);	
	}
	
	
	public override void CompDraw()
	{	
		if( forbidden )
			OverlayDrawer.DrawOverlay(parent, OverlayTypes.Forbidden);
	}
	
	public override IEnumerable<Command> CompCommands()
	{
		Command_Toggle com = new Command_Toggle();
		com.hotKey = KeyCode.F;
		com.icon = ButtonIconForbidden;
		com.isActive = ()=>!forbidden;
		com.toggleAction = ()=>
			{
				forbidden = !forbidden;

				KnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Forbidding, KnowledgeAmount.SpecificInteraction);
			};

		if( forbidden )
			com.defaultDesc = "CommandForbiddenDesc".Translate();
		else
			com.defaultDesc = "CommandNotForbiddenDesc".Translate();
		com.groupKey = 125691;
		com.tutorHighlightTag = "ToggleForbidden";

		yield return com;
	}
}


public static class ForbiddableUtility
{
	public static void SetForbidden(this Thing t, bool value)
	{
		ThingWithComponents twc = t as ThingWithComponents;
		if( twc == null )
		{
			Log.Error("Tried to SetForbidden on non-ThingWithComponents Thing " + t );
			return;
		}
		
		CompForbiddable f = twc.GetComp<CompForbiddable>();
		if( f == null )
		{
			Log.Error("Tried to SetForbidden on non-Forbiddable Thing " + t );
			return;
		}
		
		f.forbidden = value;
	}

	public static bool IsForbidden(this Thing t)
	{
		ThingWithComponents twc = t as ThingWithComponents;
		if( twc == null )
			return false;
		
		CompForbiddable f = twc.GetComp<CompForbiddable>();
		if( f == null )
			return false;
		
		if( f.forbidden )
			return true;
		
		return false;
	}
}







