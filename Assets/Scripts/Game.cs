using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	private GameObject[] tiles;

	// Use this for initialization
	void Start () {
		tiles = GameObject.FindGameObjectsWithTag("tile");
    }
	
	// Update is called once per frame
	void Update () {
	    if (!Input.GetMouseButtonDown(0)) return;
	    RaycastHit hit;
	    if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit)) return;
		GameObject obj = hit.collider.gameObject.transform.parent.gameObject;
		if (obj.name.StartsWith("Tile"))
		{
			GameObject tile = obj;
		}
	}
}
