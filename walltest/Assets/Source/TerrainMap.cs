using UnityEngine;
using System.Collections;

public class TerrainMap : MonoBehaviour {

	public Terrain terrain;

	// Use this for initialization
	void Start () {
	// I'm a JS guy, thus the code below may have horrible C# errors
    float[,,] element = new float[1,1,2]; // create a temp 1x1x2 array
    terrain.terrainData.SetAlphamaps(250, 250, element); // update only the selected terrain element

		/*
		splatmapData[y, x, 0] = element[0, 0, 0] = 0; // set the element and
		splatmapData[y, x, 1] = element[0, 0, 1] = 1; // update splatmapData
		paintingTerrain = true;
		int x = Convert.ToInt32(((npcPosition.position.x - terrainObj.transform.position.x) / terrain.terrainData.size.x) * terrain.terrainData.heightmapWidth);
		int y = Convert.ToInt32(((npcPosition.position.z - terrainObj.transform.position.z) / terrain.terrainData.size.z) * terrain.terrainData.heightmapHeight);

		splatmapData[y, x, 0] = 0;
		splatmapData[y, x, 1] = 1;

		terrain.terrainData.SetAlphamaps(0, 0, splatmapData);
		 */
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
