using UnityEngine;
using System.Collections;
using System; // needed for Convert

public class PieceDrop : MonoBehaviour {
	
/* Handles most of our game logic as it applies to our activePiece.  Forces the descent of our block and uses a raycast to identify where it stops. 
 * Fixed values in the left/right movement statement prevent going outside the bounds of the frame.
 * Upon hitting the ground or another fixedBlock the activeBlock is deleted and the SpawnFixedBlock method is called.
 */
	
	public float gameSpeed;
	public GameObject fixedBlock;
	public float explosionForce;
	public static bool dropComplete = false;
	
	float moveDirection;
	Vector2 position;
	Vector2 whereToMove;
		
	Ray ray;
	Ray spotCheck;
	RaycastHit hit;
	RaycastHit spotHit;
	
	void Update () {
		float x = whereToMove.x;
		float y = whereToMove.y;
		
		//Movement left/right
		position = transform.position;
		if (Input.GetKeyDown ("left") && position.x > 0) {
			x = -1;
		}
		if (Input.GetKeyDown ("right") && position.x < 9) {
			x = 1;
		}
		
		ray = new Ray(new Vector2(position.x, position.y),-transform.up);
		Debug.DrawRay (ray.origin,ray.direction);
		
		//Movement downwards
		if (Input.GetKey ("down")) {
			y = -gameSpeed * Time.deltaTime * 5f; // values higher can cause issues when landing
		}
		else {
			y = -gameSpeed * Time.deltaTime;
		}
		
		if (Physics.Raycast (ray,out hit)) { // needs refinement, doesn't work at higher speeds (can drop block on row above
			float distance = Vector2.Distance (ray.origin, hit.point);
			if(distance - 0.5 <= -y) {
				SpawnFixedBlock();
				Destroy(this.gameObject);
			}
		}
		
		Vector2 finalMove = new Vector2(x,y);
		transform.Translate (finalMove);
	}
		
	void SpawnFixedBlock () {
		/* Tidy up the coordinates to whole numbers and create a fixedBlock in the grid.
		 * Check to see if the new block completes a line and if so, call the ClearRow method.
		 * Check to see if our new block is in the 21st row thus ending the game.
		 * Initiate the spawning of the next activeBlock.
		 */
		float x = transform.position.x;
		float y = Mathf.Round(transform.position.y); // lock new y transform to our "grid"
		int numY = System.Convert.ToInt32(y);
		string stringY = numY.ToString ();
		Vector2 fixedPos = new Vector2(x,y);
		GameObject newBlock = Instantiate(fixedBlock,fixedPos,transform.rotation) as GameObject; // create the block that remains
		newBlock.tag = numY.ToString ();
		
		if(GameObject.FindGameObjectsWithTag(stringY).Length == 10) {
			ClearRow(stringY); 
		}
		
		if(MainLoop.rows[20] > 0) {
			MainLoop.GameOver();
		}
		
		dropComplete = true; // spawn the next piece
	}
	
	void ClearRow(string row){
		
		/* Clears out the completed row by identifying all the fixedBlocks with the appropriate tag, moving them a safe distance away from the grid so they can move freely then applying randomized forces to them for an explosion effect.
		 * Check remaining rows to move remaining blocks downwards and update their tag with their new row.
		 * Update the score variable and refresh display.
		 */

		GameObject[] clearBlocks = GameObject.FindGameObjectsWithTag (row);
		foreach (GameObject clearBlock in clearBlocks ){
			Vector3 rand = new Vector3(UnityEngine.Random.value * explosionForce,UnityEngine.Random.value * explosionForce,UnityEngine.Random.value * explosionForce); // Generate some random data for us to launch each block with
			clearBlock.transform.Translate(0,0,-2); // move them clear of remaining blocks
			Debug.Log ("applying forces to block "+clearBlock+" with a value of "+rand);
			clearBlock.AddComponent<Rigidbody>(); // make them rigidbodies
			clearBlock.rigidbody.AddForce (rand); // explode them out
			clearBlock.rigidbody.AddTorque (rand); // and spin them right round baby right round
			clearBlock.tag = "dumped";
		}

		for (int i = Convert.ToInt32(row)+1; i<21; i++){ 
			string iTag = i.ToString();
			GameObject[] moveBlocks = GameObject.FindGameObjectsWithTag (iTag);	
			foreach (GameObject moveBlock in moveBlocks) {
				moveBlock.transform.Translate (0,-1,0);	// and move each block down 1 position
				string newTag = (i-1).ToString ();
				moveBlock.tag = newTag;
			}
		}
		MainLoop.UpdateScore();
	}
}
