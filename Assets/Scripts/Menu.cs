using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	
/* Presents the menu until keypress */

	void Update () {
		if(Input.GetKeyDown ("space")) {
			Debug.Log ("Space was pressed");
			Application.LoadLevel ("Scene1");
		}
	
	}
}
