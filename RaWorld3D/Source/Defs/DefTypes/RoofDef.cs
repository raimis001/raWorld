using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;







public class RoofDef : EntityDef
{
	public bool			vanishOnCollapse = true;
	public ThingDef		collapseLeavingThingDef = null;


	public RoofDef() : base()
	{
		category = EntityCategory.Roof;
	}
}

