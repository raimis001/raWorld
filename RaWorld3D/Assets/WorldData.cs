using System.Collections.Generic;
using UnityEngine;

public class WorldData {

	public const int weaponsCount = 12;
	
	//public static List<DataTile> tiles = new List<DataTile>();
	
	public static Dictionary<int, DataTile> tiles = new Dictionary<int, DataTile>();
	
	public const int TILE_TYPE_NONE = 0;
	public const int TILE_TYPE_PLANT = 1;
	public const int TILE_TYPE_WEAPON = 2;
	public const int TILE_TYPE_RESOURCE = 3;
	public const int TILE_TYPE_FURNITURE = 4;
	public const int TILE_TYPE_WALL = 5;
	
	public const int TILE_TYPE_CRAFTING = 10;
	
	public const int TILE_STATUS_READY = 0;
	public const int TILE_STATUS_GROW = 1;
	
	public static void initTiles() {
		DataTile tile;
		
		int wid = 0;
		tile = new DataTile(wid,0,TILE_TYPE_NONE);
		tile.name = "Parasta zāle";
		tile.addReward(39,2);
		tiles.Add(wid, tile);
		
		wid = 1;
		tile = new DataTile(wid,1,TILE_TYPE_NONE);
		tile.name = "Krūms";
		tile.addReward(28,2);
		tile.speed = 0.5f;
		tiles.Add(wid, tile);
		
		wid = 2;
		tile = new DataTile(wid,2,TILE_TYPE_NONE);
		tile.name = "Kaktuss";
		tile.addReward(39,1);
		tile.addReward(28,1);
		tiles.Add(wid, tile);
		
		wid = 3;
		tile = new DataTile(wid,3,TILE_TYPE_NONE);
		tile.name = "Kaktuss";
		tile.addReward(39,1);
		tile.addReward(28,1);
		tiles.Add(wid, tile);
		
		wid = 4;
		tile = new DataTile(wid,4,TILE_TYPE_NONE);
		tile.name = "Agave";
		tile.addReward(35,2);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 5;
		tile = new DataTile(wid,5,TILE_TYPE_NONE);
		tile.name = "Lillija";
		tile.addReward(39,4);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 6;
		tile = new DataTile(wid,6,TILE_TYPE_NONE);
		tile.name = "Puķe";
		tile.addReward(39,1);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 7;
		tile = new DataTile(wid,7,TILE_TYPE_PLANT);
		tile.name = "Kartupelis";
		tile.addReward(33,3);
		tile.speed = 0.3f;
		tile.growTime = 15f;
		tiles.Add(wid, tile);
		
		wid = 8;
		tile = new DataTile(wid,8,TILE_TYPE_PLANT);
		tile.name = "Ogas";
		tile.addReward(34,1);
		tile.addReward(2,1);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 9;
		tile = new DataTile(wid,9,TILE_TYPE_PLANT);
		tile.name = "Kokvilna";
		tile.addReward(40,2);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 11;
		tile = new DataTile(wid,10,TILE_TYPE_PLANT);
		tile.name = "Zemeņogas";
		tile.addReward(1,1);
		tile.addReward(2,1);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 12;
		tile = new DataTile(wid,11,TILE_TYPE_PLANT);
		tile.name = "Priede";
		tile.addReward(1,1);
		tile.addReward(2,1);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 13;
		tile = new DataTile(wid,12,TILE_TYPE_PLANT);
		tile.name = "Osis";
		tile.addReward(1,1);
		tile.addReward(2,1);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 14;
		tile = new DataTile(wid,13,TILE_TYPE_PLANT);
		tile.name = "Liepa";
		tile.addReward(1,1);
		tile.addReward(2,1);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 15;
		tile = new DataTile(wid,14,TILE_TYPE_PLANT);
		tile.name = "Ozols";
		tile.addReward(1,1);
		tile.addReward(2,1);
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 16;
		tile = new DataTile(wid,12,TILE_TYPE_WEAPON);
		tile.name = "Rokas parastās";
		tiles.Add(wid, tile);
		
		wid = 17;
		tile = new DataTile(wid,1,TILE_TYPE_WEAPON);
		tile.name = "Koka vāle";
		tiles.Add(wid, tile);
		
		wid = 18;
		tile = new DataTile(wid,2,TILE_TYPE_WEAPON);
		tile.name = "Akmens cirvis";
		tiles.Add(wid, tile);
		
		wid = 19;
		tile = new DataTile(wid,3,TILE_TYPE_WEAPON);
		tile.name = "Bronzas cirvis";
		tiles.Add(wid, tile);
		
		wid = 20;
		tile = new DataTile(wid,4,TILE_TYPE_WEAPON);
		tile.name = "Dzelzs cirvis";
		tiles.Add(wid, tile);
		
		wid = 21;
		tile = new DataTile(wid,5,TILE_TYPE_WEAPON);
		tile.name = "Tērauda cirvis";
		tiles.Add(wid, tile);
		
		wid = 22;
		tile = new DataTile(wid,6,TILE_TYPE_WEAPON);
		tile.name = "Mačete";
		tiles.Add(wid, tile);
		
		wid = 23;
		tile = new DataTile(wid,7,TILE_TYPE_WEAPON);
		tile.name = "Koka cirtnis";
		tiles.Add(wid, tile);
		
		wid = 24;
		tile = new DataTile(wid,8,TILE_TYPE_WEAPON);
		tile.name = "Dzelzs cirtnis";
		tiles.Add(wid, tile);
		
		wid = 25;
		tile = new DataTile(wid,9,TILE_TYPE_WEAPON);
		tile.name = "Tērauda cirtnis";
		tiles.Add(wid, tile);
		
		wid = 26;
		tile = new DataTile(wid,10,TILE_TYPE_WEAPON);
		tile.name = "Mednieka loks";
		tiles.Add(wid, tile);
		
		wid = 27;
		tile = new DataTile(wid,11,TILE_TYPE_WEAPON);
		tile.name = "Arbalets";
		tiles.Add(wid, tile);
		
		wid = 28;
		tile = new DataTile(wid,0,TILE_TYPE_RESOURCE);
		tile.name = "Malka";
		tiles.Add(wid, tile);
		
		wid = 29;
		tile = new DataTile(wid,1,TILE_TYPE_RESOURCE);
		tile.name = "Baļķi";
		tiles.Add(wid, tile);
		
		wid = 30;
		tile = new DataTile(wid,2,TILE_TYPE_RESOURCE);
		tile.name = "Dēļi";
		tiles.Add(wid, tile);
		
		wid = 31;
		tile = new DataTile(wid,3,TILE_TYPE_RESOURCE);
		tile.name = "Akmeņi";
		tiles.Add(wid, tile);
		
		wid = 32;
		tile = new DataTile(wid,4,TILE_TYPE_RESOURCE);
		tile.name = "Sudrabs";
		tiles.Add(wid, tile);
		
		wid = 33;
		tile = new DataTile(wid,5,TILE_TYPE_RESOURCE);
		tile.name = "Kartupeļi";
		tile.result = new DataReward(7,1);
		tiles.Add(wid, tile);
		
		wid = 34;
		tile = new DataTile(wid,6,TILE_TYPE_RESOURCE);
		tile.name = "Ogas";
		tiles.Add(wid, tile);
		
		wid = 35;
		tile = new DataTile(wid,7,TILE_TYPE_RESOURCE);
		tile.name = "Dārzeņi";
		tiles.Add(wid, tile);
		
		wid = 36;
		tile = new DataTile(wid,8,TILE_TYPE_RESOURCE);
		tile.name = "Gaļa";
		tiles.Add(wid, tile);
		
		wid = 37;
		tile = new DataTile(wid,9,TILE_TYPE_RESOURCE);
		tile.name = "Gaļa";
		tiles.Add(wid, tile);
		
		wid = 38;
		tile = new DataTile(wid,10,TILE_TYPE_RESOURCE);
		tile.name = "Gaļa";
		tiles.Add(wid, tile);
		
		wid = 39;
		tile = new DataTile(wid,21,TILE_TYPE_RESOURCE);
		tile.name = "Salmi";
		tiles.Add(wid, tile);

		wid = 40;
		tile = new DataTile(wid,15,TILE_TYPE_RESOURCE);
		tile.name = "Audums";
		tiles.Add(wid, tile);
		
		wid = 41;
		tile = new DataTile(wid,2,TILE_TYPE_CRAFTING);
		tile.name = "Dēļi";
		tile.result = new DataReward(30, 1);
		tile.addReward(28,3);
		tiles.Add(wid, tile);
		
		wid = 42;
		tile = new DataTile(wid,5,TILE_TYPE_CRAFTING);
		tile.name = "Kartupeli";
		tile.result = new DataReward(33, 3);
		tile.addReward(39,5);
		tiles.Add(wid, tile);
		
		wid = 43;
		tile = new DataTile(wid,22,TILE_TYPE_RESOURCE);
		tile.name = "Benkitis";
		tile.result = new DataReward(45,1);
		tiles.Add(wid, tile);
		
		wid = 44;
		tile = new DataTile(wid,22,TILE_TYPE_CRAFTING);
		tile.name = "Benkitis";
		tile.result = new DataReward(43, 1);
		tile.addReward(30,2);
		tiles.Add(wid, tile);
		
		wid = 45;
		tile = new DataTile(wid,16,TILE_TYPE_FURNITURE);
		tile.name = "Benkitis";
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 46;
		tile = new DataTile(wid,23,TILE_TYPE_RESOURCE);
		tile.name = "Akmens siena";
		tile.result = new DataReward(47,1);
		tiles.Add(wid, tile);
		
		wid = 47;
		tile = new DataTile(wid,16,TILE_TYPE_WALL);
		tile.name = "Akmens siena";
		tile.speed = 0.3f;
		tiles.Add(wid, tile);
		
		wid = 48;
		tile = new DataTile(wid,23,TILE_TYPE_CRAFTING);
		tile.name = "Siena";
		tile.result = new DataReward(46, 1);
		tile.addReward(28,5);
		tiles.Add(wid, tile);
		
	}

}
