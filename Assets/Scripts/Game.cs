using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AssemblyCSharp;

public class Game : MonoBehaviour {

	private int[] field;
    private GameObject youWin;
	private int SHUFFLE_COUNT = 200;
	private TileManager tileManager;

    void Start () {
		field = new int[16];
		for (int i = 1; i < 16; i++)
			field [i - 1] = i;

        youWin = GameObject.FindGameObjectWithTag("youwin");
        youWin.SetActive(false);
		tileManager = new TileManager (
			GameObject.FindGameObjectsWithTag("tile"), 
			GameObject.FindGameObjectWithTag("tiles")
		);

        newGame();
    }

	void Update () {
		if (tileManager.isMoving()) {
			tileManager.Update ();
		}
		else if(isWin()) {
			tileManager.hide();
			youWin.SetActive (true);
		}
		else if (Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
			GameObject obj = hit.collider.gameObject.transform.parent.gameObject;
			if (obj.tag == "tile") {
				GameObject tile = obj;
				move (tile, false);
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
		for (int i = 0; i < SHUFFLE_COUNT; i++)
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

			field[zeroIndex] = field[idx];
			field[idx] = 0;

			if (validMove.Key > zeroX) {
				tileManager.move(tileNumber, TileManager.Direction.TOP, true);
			} else if (validMove.Key < zeroX) {
				tileManager.move(tileNumber, TileManager.Direction.BOTTOM, true);
			} else if (validMove.Value > zeroY) {
				tileManager.move(tileNumber, TileManager.Direction.LEFT, true);
			} else if (validMove.Value < zeroY) {
				tileManager.move(tileNumber, TileManager.Direction.RIGHT, true);
			}

            validMoves.Clear();
        }
        youWin.SetActive(false);
		tileManager.show();
    }

    public void exit()
    {
        Application.Quit();
    }

	private void move(GameObject tile, bool immediate)
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

		TileManager.Direction direction = TileManager.Direction.NONE;
		if (tileRow == zeroRow)
		{
			if (tileCol == zeroCol + 1) {
				direction = TileManager.Direction.LEFT;
			} else if (tileCol == zeroCol - 1) {
				direction = TileManager.Direction.RIGHT;
			}
		}
		else if (tileCol == zeroCol)
		{
			if (tileRow == zeroRow + 1) {
				direction = TileManager.Direction.TOP;
			} else if (tileRow == zeroRow - 1) {
				direction = TileManager.Direction.BOTTOM;
			}
		}
		if (direction != TileManager.Direction.NONE) {
			tileManager.move (tileNum, direction, false);
			field [zeroIndex] = field [tileIndex];
			field [tileIndex] = 0;
		}
	}

}
