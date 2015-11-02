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
		food.Raw.Salt = -0.3f;
		food.Raw.Sweet = 0;
		food.Raw.Sour = 0;
		food.Raw.Spice = -0.1f;
		food.Raw.Bitter = 0.7f;
		food.Raw.Time = 30;

		food.Boil.Calorie = 15;
		food.Boil.Health = 1;
		food.Boil.Salt = -0.2f;
		food.Boil.Sweet = 0;
		food.Boil.Sour = 0;
		food.Boil.Spice = -0.15f;
		food.Boil.Bitter = 0;
		food.Boil.Time = 50;

		food.Bake.Calorie = 15;
		food.Bake.Health = 2;
		food.Bake.Salt = -0.2f;
		food.Bake.Sweet = 0;
		food.Bake.Sour = 0;
		food.Bake.Spice = -0.1f;
		food.Bake.Bitter = 0;
		food.Bake.Time = 40;

		itemID = 2;
		food = new ItemFood(itemID);
		food.name = "Salt";
		food.description = "Salt spice";
		food.sprite = "food_salt";

		food.Raw.Calorie = 0;
		food.Raw.Health = 1;
		food.Raw.Salt = 0.1f;
		food.Raw.Sweet = 0;
		food.Raw.Sour = 0;
		food.Raw.Spice = 0.05f;
		food.Raw.Bitter = 0.01f;
		food.Raw.Time = 0;

		food.Boil.Calorie = 0;
		food.Boil.Health = 1;
		food.Boil.Salt = 0.1f;
		food.Boil.Sweet = 0;
		food.Boil.Sour = 0;
		food.Boil.Spice = 0.05f;
		food.Boil.Bitter = 0.01f;
		food.Boil.Time = 0;

		food.Bake.Calorie = 0;
		food.Bake.Health = 1;
		food.Bake.Salt = 0.1f;
		food.Bake.Sweet = 0;
		food.Bake.Sour = 0;
		food.Bake.Spice = 0.05f;
		food.Bake.Bitter = 0.01f;
		food.Bake.Time = 0;

		itemID = 3;
		food = new ItemFood(itemID);
		food.name = "Pork";
		food.description = "Pork meat";
		food.sprite = "food_pork";

		food.Raw.Calorie = 10;
		food.Raw.Health = -5;
		food.Raw.Salt = -0.3f;
		food.Raw.Sweet = 0;
		food.Raw.Sour = 0;
		food.Raw.Spice = -0.3f;
		food.Raw.Bitter = 0.3f;
		food.Raw.Time = 10;

		food.Boil.Calorie = 15;
		food.Boil.Health = 2;
		food.Boil.Salt = -0.2f;
		food.Boil.Sweet = 0;
		food.Boil.Sour = 0;
		food.Boil.Spice = -0.2f;
		food.Boil.Bitter = 0;
		food.Boil.Time = 30;

		food.Bake.Calorie = 15;
		food.Bake.Health = 2;
		food.Bake.Salt = -0.2f;
		food.Bake.Sweet = 0;
		food.Bake.Sour = 0;
		food.Bake.Spice = -0.25f;
		food.Bake.Bitter = 0;
		food.Bake.Time = 60;

		itemID = 4;
		food = new ItemFood(itemID);
		food.name = "Pepper";
		food.description = "Black pepper";
		food.sprite = "food_pepper";

		food.Raw.Calorie = 0;
		food.Raw.Health = 1;
		food.Raw.Salt = 0;
		food.Raw.Sweet = 0;
		food.Raw.Sour = 0;
		food.Raw.Spice = 0.2f;
		food.Raw.Bitter = 0.08f;
		food.Raw.Time = 0;

		food.Boil.Calorie = 0;
		food.Boil.Health = 1;
		food.Boil.Salt = 0;
		food.Boil.Sweet = 0;
		food.Boil.Sour = 0;
		food.Boil.Spice = 0.2f;
		food.Boil.Bitter = 0.01f;
		food.Boil.Time = 0;

		food.Bake.Calorie = 0;
		food.Bake.Health = 1;
		food.Bake.Salt = 0;
		food.Bake.Sweet = 0;
		food.Bake.Sour = 0;
		food.Bake.Spice = 0.2f;
		food.Bake.Bitter = 0.01f;
		food.Bake.Time = 0;

		itemID = 5;
		food = new ItemFood(itemID);
		food.name = "Carrot";
		food.description = "Raw carrot";
		food.sprite = "food_carrot";

		food.Raw.Calorie = 5;
		food.Raw.Health = 5;
		food.Raw.Salt = 0;
		food.Raw.Sweet = 0.1f;
		food.Raw.Sour = 0;
		food.Raw.Spice = 0f;
		food.Raw.Bitter = -0.05f;
		food.Raw.Time = 0;

		food.Boil.Calorie = 3;
		food.Boil.Health = 4;
		food.Boil.Salt = 0f;
		food.Boil.Sweet = 0.1f;
		food.Boil.Sour = 0;
		food.Boil.Spice = 0f;
		food.Boil.Bitter = -0.05f;
		food.Boil.Time = 0;

		food.Bake.Calorie = 3;
		food.Bake.Health = 4;
		food.Bake.Salt = 0;
		food.Bake.Sweet = 0.1f;
		food.Bake.Sour = 0;
		food.Bake.Spice = -0.05f;
		food.Bake.Bitter = -0.05f;
		food.Bake.Time = 0;

		itemID = 6;
		food = new ItemFood(itemID);
		food.name = "Lemon";
		food.description = "Lemon frouit";
		food.sprite = "food_lemon";

		food.Raw.Calorie = 1;
		food.Raw.Health = 10;
		food.Raw.Salt = 0;
		food.Raw.Sweet = -0.2f;
		food.Raw.Sour = 0.3f;
		food.Raw.Spice = 0.05f;
		food.Raw.Bitter = -0.05f;
		food.Raw.Time = 10;

		food.Boil.Calorie = 1;
		food.Boil.Health = 0;
		food.Boil.Salt = 0f;
		food.Boil.Sweet = 0f;
		food.Boil.Sour = 0;
		food.Boil.Spice = 0f;
		food.Boil.Bitter = 0.2f;
		food.Boil.Time = 0;

		food.Bake.Calorie = 1;
		food.Bake.Health = 0;
		food.Bake.Salt = 0;
		food.Bake.Sweet = 0f;
		food.Bake.Sour = 0;
		food.Bake.Spice = 0f;
		food.Bake.Bitter = 0.2f;
		food.Bake.Time = 0;

	}

}
