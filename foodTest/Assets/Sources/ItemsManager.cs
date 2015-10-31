using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemClass {
	public int item_id;
	public string name;
	public string description;
	public string sprite;

	public Sprite Image {
		get {
			Sprite temp = null;
			ItemsManager.Icons.TryGetValue(sprite, out temp);

			return temp;
		}
	}

	public ItemClass(int itemID) {
		item_id = itemID;
		ItemsManager.Items.Add(item_id, this);
	}

}

public class FoodClass {
	public int id = -1;
	public int Calorie = 0;
	public int Health = 0;
	public int Sweet = 0;
	public int Sour = 0;
	public int Salt = 0;
	public int Bitter = 0;
	public float Time = 0;
}

public class ItemFood : ItemClass {
	
	public FoodClass Raw = new FoodClass();
	public FoodClass Bake = new FoodClass();
	public FoodClass Boil = new FoodClass();

	public ItemFood(int itemID)
		: base(itemID) {
		Raw.id = itemID;
		Bake.id = itemID;
		Boil.id = itemID;
	}
}

public class FoodCollection : FoodClass {

	List<FoodClass> Collection = new List<FoodClass>();

	public float Taste {
		get {
			float temp = 0;

			float tBitter = Mathf.Abs(1 - Bitter / 50f);
			tBitter = Bitter > 50 ? 22f - tBitter * 22f : tBitter * 80f + 20;
			if (tBitter > 20) return tBitter;

			float tSweet = Mathf.Abs(Sweet);
			float tSalt = Mathf.Abs(Salt);
			float tSour = Mathf.Abs(Sour);

			if (Sweet < temp) temp = Sweet;
			if (Salt < temp) temp = Salt;
			if (Sour < temp) temp = Sour;

			if (Bitter < temp) temp = Bitter;

			return 0;
		}
	}

	public void Add(FoodClass collection) {
		Collection.Add(collection);
		Recalc();
	}

	public void Remove(int itemID) {

		for (int i = 0; i < Collection.Count; i++) {
			if (Collection[i].id == itemID) { 
				Collection.RemoveAt(i);
				break;
			}
		}

		Recalc();
	}

	public void Recalc() {
		Calorie = 0;
		Health = 900;
		Sweet = 0;
		Sour = 0;
		Salt = 0;
		Bitter = 0;
		Time = 0;

		foreach (FoodClass food in Collection) {
			Calorie += food.Calorie;
			Sweet += food.Sweet;
			Salt += food.Salt;
			Sour += food.Sour;
			Bitter += food.Bitter;

			if (food.Health < Health) Health = food.Health;
			if (food.Time > Time) Time = food.Time;

		}

	}

}

public class ItemsManager {

	public static Dictionary<string, Sprite> Icons = new Dictionary<string, Sprite>();
	public static Dictionary<int, ItemClass> Items = new Dictionary<int, ItemClass>();

	public ItemsManager() {

		Sprite[] sprites = Resources.LoadAll<Sprite>("Icons");
		foreach (Sprite sp in sprites) Icons.Add(sp.name, sp);

		DataBase.InitItems();
		
		//food = GetItem<ItemFood>(1);
		//Debug.Log(food.name);

		Debug.Log("Items manager init");
	}

	public static T GetItem<T>(int itemID) where T : ItemClass {

		ItemClass result = null;
		Items.TryGetValue(itemID,out result);
		if (result == null) return default(T);

		return (T)result;
	} 

	public static void Init() {
		new ItemsManager();
	}


}