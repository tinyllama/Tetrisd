using UnityEngine;
using System.Collections;
using System; // needed for Array.Clear

public class MainLoop : MonoBehaviour {

/* -Store variables that are generated and manipulated in (but need to persist after the destruction of) activePiece prefabs.
 * -Instantiate another activePiece if the previous one has landed and been turned into a fixedPiece.
 * -Handle game-wide keypresses such as going back to the menu or muting the audio.
 */
	
public GameObject activePiece;
static GameObject scoreboard;
Vector2 startPosition = new Vector2(5,22); // where new pieces spawn
static int score = 0;
public static int[] rows = new int[23];
public static bool groundHit = false;

	void Start () {
		SpawnPiece ();
		scoreboard = GameObject.Find("Score");
	}
	
	void Update () {
		//Spawn Piece
		if (PieceDrop.dropComplete == true){
			SpawnPiece ();
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
			GameOver ();
		}
		
	}
	
	void SpawnPiece() {
		/* Determines what kind of block to spawn and creates the required activeBlocks at the start location.
		 */
		int pieceType = UnityEngine.Random.Range(0,4);
		float startPosX = startPosition.x;
		float startPosY = startPosition.y;
		
		
		if (pieceType == 0) {
			Debug.Log("spawn long piece");
			Vector2 bOnePos = new Vector2(startPosX-1,startPosY);
			Vector2 bTwoPos = new Vector2(startPosX,startPosY);
			Vector2 bThreePos = new Vector2(startPosX+1,startPosY);
			Vector2 bFourPos = new Vector2(startPosX+2,startPosY);
			
			Instantiate(activePiece,bOnePos,Quaternion.identity);
			Instantiate(activePiece,bTwoPos,Quaternion.identity);
			Instantiate(activePiece,bThreePos,Quaternion.identity);
			Instantiate(activePiece,bFourPos,Quaternion.identity);
		}
		if (pieceType == 1) {
			Debug.Log("spawn slidebox piece");
			Vector2 bOnePos = new Vector2(startPosX-1,startPosY);
			Vector2 bTwoPos = new Vector2(startPosX,startPosY);
			Vector2 bThreePos = new Vector2(startPosX,startPosY-1);
			Vector2 bFourPos = new Vector2(startPosX+1,startPosY-1);
			
			Instantiate(activePiece,bOnePos,Quaternion.identity);
			Instantiate(activePiece,bTwoPos,Quaternion.identity);
			Instantiate(activePiece,bThreePos,Quaternion.identity);
			Instantiate(activePiece,bFourPos,Quaternion.identity);
		}
		if (pieceType == 2) {
			Debug.Log("spawn box piece");
			Vector2 bOnePos = new Vector2(startPosX,startPosY);
			Vector2 bTwoPos = new Vector2(startPosX+1,startPosY);
			Vector2 bThreePos = new Vector2(startPosX,startPosY-1);
			Vector2 bFourPos = new Vector2(startPosX+1,startPosY-1);
			
			Instantiate(activePiece,bOnePos,Quaternion.identity);
			Instantiate(activePiece,bTwoPos,Quaternion.identity);
			Instantiate(activePiece,bThreePos,Quaternion.identity);
			Instantiate(activePiece,bFourPos,Quaternion.identity);
		}
		if (pieceType == 3) {
			Debug.Log("spawn hat piece");
			Vector2 bOnePos = new Vector2(startPosX-1,startPosY);
			Vector2 bTwoPos = new Vector2(startPosX,startPosY);
			Vector2 bThreePos = new Vector2(startPosX+1,startPosY);
			Vector2 bFourPos = new Vector2(startPosX,startPosY+1);
			
			Instantiate(activePiece,bOnePos,Quaternion.identity);
			Instantiate(activePiece,bTwoPos,Quaternion.identity);
			Instantiate(activePiece,bThreePos,Quaternion.identity);
			Instantiate(activePiece,bFourPos,Quaternion.identity);
		}
		else {
			Debug.Log("spawn L piece");
			Vector2 bOnePos = new Vector2(startPosX-1,startPosY);
			Vector2 bTwoPos = new Vector2(startPosX,startPosY);
			Vector2 bThreePos = new Vector2(startPosX+1,startPosY);
			Vector2 bFourPos = new Vector2(startPosX+1,startPosY-1);
			
			Instantiate(activePiece,bOnePos,Quaternion.identity);
			Instantiate(activePiece,bTwoPos,Quaternion.identity);
			Instantiate(activePiece,bThreePos,Quaternion.identity);
			Instantiate(activePiece,bFourPos,Quaternion.identity);
		}
	}
	
	public static void GameOver() {
		/*Returns player to menu and performs general variable cleanup.
		 */
		Array.Clear(rows, 0, rows.Length);
		Application.LoadLevel ("Menu");	
	}
	
	public static void UpdateScore() {
		score++;
		scoreboard.guiText.text = score.ToString();
	}
}
