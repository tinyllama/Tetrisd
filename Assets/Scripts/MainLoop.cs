using UnityEngine;
using System.Collections;

public class MainLoop : MonoBehaviour {

int nextPiece;
public GameObject activePiece;
Vector3 startPosition = new Vector2(5,22);
public static int[] rows = new int[21];

	// Use this for initialization
	void Start () {
		Instantiate(activePiece,startPosition,Quaternion.identity);
	}
	
	void Update () {
		if (PieceDrop.dropComplete == true){
			Instantiate(activePiece,startPosition,Quaternion.identity);
			PieceDrop.dropComplete = false;
		}
		if (Input.GetKeyDown ("m")) {
			if (audio.mute == false) {
				audio.mute = true;
			}
			else {
				audio.mute = false;
			}
		}
	}
}
