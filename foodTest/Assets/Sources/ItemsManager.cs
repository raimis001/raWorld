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
	public float Calorie = 0;
	public float Health = 0;
	public float Sweet = 0;
	public float Sour = 0;
	public float Salt = 0;
	public float Spice = 0;
	public float Bitter = 0;
	public float Time = 0;

	public override string ToString() {
		return "Food" +
			" Calorie:" + Calorie.ToString("0") +
			" Sweet:" + Sweet.ToString("0") +
			" Salt:" + Salt.ToString("0") +
			" Sour:" + Sour.ToString("0") +
			" Spice:" + Spice.ToString("0") +
			" Bitter:" + Bitter.ToString("0");
	}

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

	public int Taste {
		get {
			float temp = 0;

			float[] tastes = new float[5];

			tastes[0] = Mathf.Abs(Sweet);
			tastes[1] = Mathf.Abs(Salt);
			tastes[2] = Mathf.Abs(Sour);
			tastes[3] = Mathf.Abs(Spice);
			tastes[4] = Mathf.Abs(Bitter);

			float modif = 0;
			List<int> unique = new List<int>();

			foreach (FoodClass food in Collection) {
				if (!unique.Contains(food.id)) {
					modif ++;
					unique.Add(food.id);
				}
			}

			modif = modif < 7 ? modif / 7f : 7f / modif;

			temp = 9 * (Mathf.Max(tastes) - modif);

			//Debug.Log(temp);
			//Debug.Log(modif);
			
			return Mathf.Clamp((int)temp, 0, 9 );
		}
	}

	public void Add(FoodClass collection, int amount = 1) {
		for (int i = 0; i < amount; i++)
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
	public void Clear() {
		Collection.Clear();
		Recalc();
	}

	public void Recalc() {
		Calorie = 0;
		Health = 900;
		Sweet = 0;
		Sour = 0;
		Salt = 0;
		Bitter = 0;
		Spice = 0;
		Time = 0;

		float pBitter = 0;
		float nBitter = 0;
		foreach (FoodClass food in Collection) {
			Calorie += food.Calorie;
			Sweet += food.Sweet;
			Salt += food.Salt;
			Sour += food.Sour;
			Spice += food.Spice;

			if (food.Bitter > 0 && food.Bitter > pBitter) pBitter = food.Bitter;
			if (food.Bitter < 0 ) nBitter += food.Bitter;

			if (food.Health < Health) Health = food.Health;
			if (food.Time > Time) Time = food.Time;

		}

		Bitter = (int)(pBitter + nBitter);

		if (Health >= 900) Health = 0;

		Calorie = Mathf.Clamp(Calorie, -100, 100);
		Sweet = Mathf.Clamp(Sweet, -1, 1);
		Salt = Mathf.Clamp(Salt, -1, 1);
		Sour = Mathf.Clamp(Sour, -1, 1);
		Spice = Mathf.Clamp(Spice, -1, 1);
		Bitter = Mathf.Clamp(Bitter, -1, 1);


		//Debug.Log(this.ToString());
	}
	public int Count() {
		return Collection.Count;
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