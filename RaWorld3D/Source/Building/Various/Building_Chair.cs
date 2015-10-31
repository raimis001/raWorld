using UnityEngine;
using System.Collections;
using System.Linq;


public class Building_Chair : Building
{
	//Properties
	public IntVec3 SpotInFrontOfChair
	{
		get
		{
			return Position + rotation.FacingSquare;
		}
	}
	public bool IsFacingTable
	{
		get
		{
			return SpotInFrontOfChair.ItemSurface();
		}
	}
}
