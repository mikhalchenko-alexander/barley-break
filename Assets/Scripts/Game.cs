using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour {

	private GameObject[] tiles;
	private int[] field;

	// Use this for initialization
	void Start () {
		tiles = GameObject.FindGameObjectsWithTag("tile");
		Array.Sort (tiles,
			delegate(GameObject a, GameObject b) {
				return tileNumber(a.name) - tileNumber(b.name);
			});
		field = new int[16];
		for (int i = 1; i < 16; i++)
			field [i - 1] = i;
		
    }
	
	// Update is called once per frame
	void Update () {
	    if (!Input.GetMouseButtonDown(0)) return;
	    RaycastHit hit;
	    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
		GameObject obj = hit.collider.gameObject.transform.parent.gameObject;
		if (obj.tag == "tile")
		{
			GameObject tile = obj;
			int tileNum = tileNumber(tile.name);

			int tileIndex = -1;
			int zeroIndex = -1;
			for (int i = 0; i < 16; i++) {
				if (field [i] == tileNum)
					tileIndex = i;
				if (field [i] == 0)
					zeroIndex = i;
				if (tileIndex >= 0 && zeroIndex >= 0)
					break;
			}

			int tileRow = tileIndex / 4;
			int tileCol = tileIndex % 4;

			int zeroRow = zeroIndex / 4;
			int zeroCol = zeroIndex % 4;

			if (tileRow == zeroRow) {
				if (tileCol == zeroCol + 1) {
					Vector3 p = tiles [tileNum - 1].transform.position;
					tiles [tileNum - 1].transform.position = new Vector3 (p.x - 3.5f, p.y, p.z);
					field [zeroIndex] = field [tileIndex];
					field [tileIndex] = 0;
				} else if (tileCol == zeroCol - 1) {
					Vector3 p = tiles [tileNum - 1].transform.position;
					tiles [tileNum - 1].transform.position = new Vector3(p.x + 3.5f, p.y, p.z);
					field [zeroIndex] = field [tileIndex];
					field [tileIndex] = 0;
				}
			}
            else if (tileCol == zeroCol)
            {
                if (tileRow == zeroRow + 1)
                {
                    Vector3 p = tiles[tileNum - 1].transform.position;
                    tiles[tileNum - 1].transform.position = new Vector3(p.x, p.y + 3.5f, p.z);
                    field[zeroIndex] = field[tileIndex];
                    field[tileIndex] = 0;
                } else if (tileRow == zeroRow - 1)
                {
                    Vector3 p = tiles[tileNum - 1].transform.position;
                    tiles[tileNum - 1].transform.position = new Vector3(p.x, p.y - 3.5f, p.z);
                    field[zeroIndex] = field[tileIndex];
                    field[tileIndex] = 0;
                }
            }

		}
	}

	private int tileNumber(String tileName) 
	{
		return int.Parse(tileName.Split(" ".ToCharArray())[1]);
	}
}
