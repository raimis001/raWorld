using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UI;


public class Building_Storage : Building, SlotGroupParent
{
		//Working vars
	public SlotGroup		slotGroup;
	public StorageSettings	settings;


	private List<IntVec3>	cachedOccupiedSquares;

	//=======================================================================
	//========================== SlotGrouParent interface=======================
	//=======================================================================

	public SlotGroup GetSlotGroup(){return slotGroup;}
	public virtual void Notify_ReceivedThing(Thing newItem){/*Nothing by default*/}
	public virtual void Notify_LostThing(Thing newItem){/*Nothing by default*/}
	public virtual IEnumerable<IntVec3> AllSlotSquares()
	{
		foreach( IntVec3 sq in GenAdj.SquaresOccupiedBy(this) )
		{
			yield return sq;
		}
	}
	public List<IntVec3> AllSlotSquaresListFast()
	{
		return cachedOccupiedSquares;
	}
	public StorageSettings GetStoreSettings()
	{
		return settings;
	}
	public StorageSettings GetParentStoreSettings()
	{
		return def.fixedStorageSettings;
	}
	public string SlotYielderLabel(){return Label;}
	


	//=======================================================================
	//============================== Other stuff ============================
	//=======================================================================

	public override void PostMake()
	{
		base.PostMake();
		settings = new StorageSettings(this);

		if( def.defaultStorageSettings != null )
			settings.CopyFrom( def.defaultStorageSettings );
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		slotGroup = new SlotGroup(this);

		cachedOccupiedSquares = AllSlotSquares().ToList();
	}
	
	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.LookDeep(ref settings, "settings", this);
	}
	
	public override void Destroy()
	{
		slotGroup.Notify_ParentDestroying();

		base.Destroy();
	}
}