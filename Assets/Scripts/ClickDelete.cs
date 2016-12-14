using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDelete : MonoBehaviour {

    public GameObject cube;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject == cube)
                {
                    Destroy(gameObject);
                }
            }           
        }
	}
}
