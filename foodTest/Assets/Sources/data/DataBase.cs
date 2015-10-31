using UnityEngine;
using System.Collections;

public class DataBase {

	public static void InitItems() {
		int itemID = 0;
		new ItemClass(itemID);

		itemID = 1;
		ItemFood food = new ItemFood(itemID);
		food.name = "Potato";
		food.description = "Potato raw";
		food.sprite = "food_potato";
		food.Raw.Calorie = 5;
		food.Raw.Health = 0;
		food.Raw.Salt = 0;
		food.Raw.Sweet = 0;
		food.Raw.Sour = 0;
		food.Raw.Bitter = 10;
		food.Raw.Time = 30;

		food.Bake.Calorie = 50;
		food.Bake.Health = 1;
		food.Bake.Salt = -5;
		food.Bake.Sweet = 0;
		food.Bake.Sour = 0;
		food.Bake.Bitter = 0;
		food.Bake.Time = 50;

		food.Boil.Calorie = 50;
		food.Boil.Health = 1;
		food.Boil.Salt = -5;
		food.Boil.Sweet = 0;
		food.Boil.Sour = 0;
		food.Boil.Bitter = 0;
		food.Boil.Time = 60;


	}

}
