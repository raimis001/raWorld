using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spark : Projectile
{
	protected override void Impact(Thing hitThing)
	{
		base.Impact(hitThing);
		
		FireUtility.TryStartFireIn(Position, Fire.MinFireSize);
	}	
}
