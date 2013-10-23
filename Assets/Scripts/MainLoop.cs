using UnityEngine;
using System.Collections;
using System; // needed for Array.Clear

public class MainLoop : MonoBehaviour {

/* -Store variables that are generated and manipulated in (but need to persist after the destruction of) activePiece prefabs.
 * -Instantiate another activePiece if the previous one has landed and been turned into a fixedPiece.
 * -Handle game-wide keypresses such as going back to the menu or muting the audio.
 */
	
public GameObject activePiece;
Vector3 startPosition = new Vector2(5,22); // where new pieces spawn
public static int[] rows = new int[23];

	void Start () {
		Instantiate(activePiece,startPosition,Quaternion.identity);
	}
	
	void Update () {
		//Spawn Piece
		if (PieceDrop.dropComplete == true){
			Instantiate(activePiece,startPosition,Quaternion.identity);
			PieceDrop.dropComplete = false;
		}
		//Mute/Unmute:
		if (Input.GetKeyDown ("m")) {
			if (audio.mute == false) {
				audio.mute = true;
			}
			else {
				audio.mute = false;
			}
		}
		//Quit:
		if (Input.GetKeyDown ("escape")) {
			Application.LoadLevel ("Menu");
		}
		
	}
	public static void GameOver() {
		/*Returns player to menu and performs general variable cleanup.
		 */
		Array.Clear(rows, 0, rows.Length);
		Application.LoadLevel ("Menu");	
	}
}
