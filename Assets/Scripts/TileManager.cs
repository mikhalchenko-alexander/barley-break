using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class TileManager
	{
		private SortedDictionary<int, GameObject> tiles = new SortedDictionary<int, GameObject>();
		private GameObject tilesContainer;
		private static float tileSize = 2.5f;
		private MovingTile movingTile;
		private bool moving;
		private int moveSpeed = 10;

		struct MovingTile {
			public GameObject tile;
			public Vector3 target;

			public MovingTile(GameObject tile, float x, float y) 
			{
				this.tile = tile;
				this.target = new Vector3(x, y, tile.transform.position.z);
			}
		}

		public enum Direction 
		{
			LEFT, RIGHT, TOP, BOTTOM, NONE
		}

		public TileManager (GameObject[] tiles, GameObject tilesContainer)
		{
			foreach (GameObject tile in tiles) {
				this.tiles.Add (tileNumber(tile.name), tile);
			}
			this.tilesContainer = tilesContainer;
		}

		public void move(int tileNumber, Direction direction, bool immediate) 
		{
			GameObject tile = tiles [tileNumber];
			Vector3 curPos = tile.transform.position;

			switch (direction) {
			case Direction.TOP:
				movingTile = new MovingTile (tile, curPos.x, curPos.y + tileSize);
				break;
			case Direction.BOTTOM:
				movingTile = new MovingTile (tile, curPos.x, curPos.y - tileSize);
				break;
			case Direction.LEFT:
				movingTile = new MovingTile (tile, curPos.x - tileSize, curPos.y);
				break;
			case Direction.RIGHT:
				movingTile = new MovingTile (tile, curPos.x + tileSize, curPos.y);
				break;
			}
			moving = true;

			if (immediate) {
				movingTile.tile.transform.position = movingTile.target;
				moving = false;
			}
		}

		public void show()
		{
			tilesContainer.SetActive (true);
		}

		public void hide()
		{
			tilesContainer.SetActive (false);
		}

		public bool isMoving()
		{
			return moving;
		}

		public void Update() 
		{
			if (moving)
			{
				Vector3 currentTilePos = movingTile.tile.transform.position;

				if (Vector3.Distance (movingTile.target, currentTilePos) > 0.00001) {
					movingTile.tile.transform.position = Vector3.MoveTowards (currentTilePos, movingTile.target, Time.deltaTime * moveSpeed);
				} else {
					movingTile.tile.transform.position = movingTile.target;
					moving = false;
				}
			}
		}

		private int tileNumber(String tileName) 
		{
			return int.Parse(tileName.Split(" ".ToCharArray())[1]);
		}
	}
}

