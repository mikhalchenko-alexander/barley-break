using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour {

	private GameObject[] tiles;
	private int[] field;
    private float tileSize = 2.5f;
    private GameObject youWin;
    private GameObject tilesContainer;

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

        youWin = GameObject.FindGameObjectWithTag("youwin");
        youWin.SetActive(false);
        tilesContainer = GameObject.FindGameObjectWithTag("tiles");

        newGame();
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
            bool moved = move(tile);

            if (moved && isWin())
            {
                tilesContainer.SetActive(false);
                youWin.SetActive(true);
            }
		}
	}

	private int tileNumber(String tileName) 
	{
		return int.Parse(tileName.Split(" ".ToCharArray())[1]);
	}

    private bool isWin()
    {
        for (int i = 1; i < 16; i++)
        {
            if (field[i - 1] != i) return false;
        }
        return true;
    }

    public void newGame()
    {
        List<KeyValuePair<int, int>> validMoves = new List<KeyValuePair<int, int>>();
        System.Random rnd = new System.Random();
        for (int i = 0; i < 200; i++)
        {
            int zeroIndex = -1;
            for (int j = 0; j < 16; j++)
            {
                if (field[j] == 0)
                    zeroIndex = j;
                if (zeroIndex >= 0)
                    break;
            }
            int zeroX = zeroIndex / 4;
            int zeroY = zeroIndex % 4;

            if (zeroX + 1 <= 3) validMoves.Add(new KeyValuePair<int, int>(zeroX + 1, zeroY));
            if (zeroX - 1 >= 0) validMoves.Add(new KeyValuePair<int, int>(zeroX - 1, zeroY));
            if (zeroY + 1 <= 3) validMoves.Add(new KeyValuePair<int, int>(zeroX, zeroY + 1));
            if (zeroY - 1 >= 0) validMoves.Add(new KeyValuePair<int, int>(zeroX, zeroY - 1));

            int random = rnd.Next(validMoves.Count);
            KeyValuePair<int, int> validMove = validMoves[random];
            int idx = validMove.Key * 4 + validMove.Value;
            int tileNumber = field[idx];
            GameObject tile = tiles[tileNumber - 1];
            move(tile);

            validMoves.Clear();
        }
        youWin.SetActive(false);
        tilesContainer.SetActive(true);
    }

    public void exit()
    {
        Application.Quit();
    }

    private bool move(GameObject tile)
    {
        int tileNum = tileNumber(tile.name);

        int tileIndex = -1;
        int zeroIndex = -1;
        for (int i = 0; i < 16; i++)
        {
            if (field[i] == tileNum)
                tileIndex = i;
            if (field[i] == 0)
                zeroIndex = i;
            if (tileIndex >= 0 && zeroIndex >= 0)
                break;
        }

        int tileRow = tileIndex / 4;
        int tileCol = tileIndex % 4;

        int zeroRow = zeroIndex / 4;
        int zeroCol = zeroIndex % 4;

        bool moved = false;

        if (tileRow == zeroRow)
        {
            if (tileCol == zeroCol + 1)
            {
                Vector3 p = tiles[tileNum - 1].transform.position;
                tiles[tileNum - 1].transform.position = new Vector3(p.x - tileSize, p.y, p.z);
                field[zeroIndex] = field[tileIndex];
                field[tileIndex] = 0;
                moved = true;
            }
            else if (tileCol == zeroCol - 1)
            {
                Vector3 p = tiles[tileNum - 1].transform.position;
                tiles[tileNum - 1].transform.position = new Vector3(p.x + tileSize, p.y, p.z);
                field[zeroIndex] = field[tileIndex];
                field[tileIndex] = 0;
                moved = true;
            }
        }
        else if (tileCol == zeroCol)
        {
            if (tileRow == zeroRow + 1)
            {
                Vector3 p = tiles[tileNum - 1].transform.position;
                tiles[tileNum - 1].transform.position = new Vector3(p.x, p.y + tileSize, p.z);
                field[zeroIndex] = field[tileIndex];
                field[tileIndex] = 0;
                moved = true;
            }
            else if (tileRow == zeroRow - 1)
            {
                Vector3 p = tiles[tileNum - 1].transform.position;
                tiles[tileNum - 1].transform.position = new Vector3(p.x, p.y - tileSize, p.z);
                field[zeroIndex] = field[tileIndex];
                field[tileIndex] = 0;
                moved = true;
            }
        }
        return moved;
    }

}
