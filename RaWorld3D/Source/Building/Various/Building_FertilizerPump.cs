using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;



public class Building_FertilizerPump : Building
{
	//Working vars
	private int				ticksSinceExpand = 0;
	private int				timesExpanded = 0;

	//Component references
	private CompPowerTrader powerComp;
	private TerrainDef		soilDef;

	//Constants
	private int				TicksPerSquare = 5000;	//four squares per day
	private int				MaxSquaresToAffect = 25;



	public override void PostMake()
	{
		base.PostMake();

		powerComp = GetComp<CompPowerTrader>();
		soilDef = DefDatabase<TerrainDef>.GetNamed("Soil");
	}

	public override void ExposeData()
	{
		base.ExposeData();

		Scribe_Values.LookValue( ref ticksSinceExpand, "ticksSinceExpand" );
		Scribe_Values.LookValue( ref timesExpanded, "timesExpanded" );
	}

	public override void TickRare()
	{
		if( powerComp.PowerOn )
		{
			ticksSinceExpand += DateHandler.TickRareInterval;

			if( ticksSinceExpand >= TicksPerSquare )
			{
				Expand();
				ticksSinceExpand = 0;
			}
		}
	}

	private void Expand()
	{
		timesExpanded++;

		if( timesExpanded > MaxSquaresToAffect )
			return;

		for( int i=0; i<timesExpanded; i++ )
		{
			IntVec3 sq = Position + GenRadial.RadialPattern[i];

			if( !sq.InBounds() )
				continue;

			if( Find.TerrainGrid.TerrainAt( sq ).fertility < soilDef.fertility )
				Find.TerrainGrid.SetTerrain( sq, soilDef );

		}
	}
}
