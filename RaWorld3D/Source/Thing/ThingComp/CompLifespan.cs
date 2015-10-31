using UnityEngine;
using System.Collections;

public class CompLifespan : ThingComp
{
	public int LifespanTicksLeft;
	
	public override void CompTick()
	{
		LifespanTicksLeft--;
		
		if( LifespanTicksLeft <= 0 )
			parent.Destroy();
	}
}
