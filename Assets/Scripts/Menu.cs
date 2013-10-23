using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown ("space")) {
			Debug.Log ("Space was pressed");
			Application.LoadLevel ("Scene1");
		}
	
	}
}
