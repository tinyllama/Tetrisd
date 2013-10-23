using UnityEngine;
using System.Collections;
using System; // needed for Convert

public class PieceDrop : MonoBehaviour {
	
/* Handles most of our game logic as it applies to our activePiece.  Forces the descent of our block and uses a raycast to identify where it stops. 
 * Fixed values in the left/right movement statement prevent going outside the bounds of the frame.
 * Upon hitting the ground or another fixedBlock the activeBlock is deleted and the SpawnFixedBlock method is called.
 */
	
	public float gameSpeed = 10f;
	public GameObject fixedBlock;
	float moveDirection;
	Vector2 position;
	Vector2 whereToMove;
//	private float skin = .005f; // space between ground and collider to prevent clipping through ground
	public static bool dropComplete = false;
	
	Ray ray;
	Ray spotCheck;
	RaycastHit hit;
	RaycastHit spotHit;
	
	void Start () {
		//position = transform.position;
	}
	
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
		//Debug.Log ("start " +y + "gameSpeed: " + -gameSpeed);
		if (Input.GetKey ("down")) {
			y = -gameSpeed * Time.deltaTime * 5f; // values higher can cause issues when landing
		}
		else {
			y = -gameSpeed * Time.deltaTime;
		}
		//Debug.Log ("postcalc" +y);
		
		if (Physics.Raycast (ray,out hit)) {
			float distance = Vector2.Distance (ray.origin, hit.point);
			//Debug.Log("Ray Length: " + distance +" Move : " + -y);
			if(distance - 0.5 <= -y) {
				//Debug.Log ("trying to make 0 from " + y + " - " + position.y);
				SpawnFixedBlock();
				Destroy(this.gameObject);
			}
		}
		
		Vector2 finalMove = new Vector2(x,y);
		//Debug.Log ("final" +finalMove.y + "from " + position.y);
		transform.Translate (finalMove);
	}
		
	void SpawnFixedBlock () {
		/* Tidy up the coordinates to whole numbers and create a fixedBlock in the grid.
		 * Check to see if the new block completes a line and if so, call the ClearRow method.
		 * Check to see if our new block is in the 21st row thus ending the game.
		 * Initiate the spawning of the next activeBlock
		 */
		float x = transform.position.x;
		float y = Mathf.Round(transform.position.y); // lock new y transform to our "grid"
		int numY = System.Convert.ToInt32(y);
		Vector2 fixedPos = new Vector2(x,y);
		GameObject newBlock = Instantiate(fixedBlock,fixedPos,transform.rotation) as GameObject; // create the block that remains
		newBlock.tag = numY.ToString ();
		//Debug.Log ("array val b4" +rows[numY] +"row number: "+numY);
		MainLoop.rows[numY]++;//rows[numY]++; //increment value by 1
		//Debug.Log (MainLoop.rows[numY]);
		
		
		// is the line complete?
		if(MainLoop.rows[numY] == 10) {
			string row = numY.ToString();
			ClearRow(row); // clear out our completed row
			MainLoop.rows[numY] = 0; // and reset the block count for that row to 0 in our array
		}
		if(MainLoop.rows[20] > 0) {
			MainLoop.GameOver();
		}
		dropComplete = true; // spawn the next piece
	}
	
	void ClearRow(string row){
		/* Clears out the completed row by identifying all the fixedBlocks with the appropriate tag, moving them a safe distance away from the grid so they can move freely then applying randomized forces to them for an explosion effect.
		 * Check remaining rows to move remaining blocks downwards and update the row count array accordingly.
		 */
		Debug.Log ("YO ITS TIME TO CLEAR OUT ROW: " + row);
		//apply force to the row in question
		GameObject[] clearBlocks = GameObject.FindGameObjectsWithTag (row);
		foreach (GameObject clearBlock in clearBlocks ){
			Vector3 pew = new Vector3(UnityEngine.Random.value * 100,UnityEngine.Random.value * 100,UnityEngine.Random.value * 100); // Generate some random data for us to launch each block with
			//Vector3 rot = new Vector3(UnityEngine.Random.rotation);
			clearBlock.transform.Translate(0,0,-2); // move them clear of remaining blocks
			Rigidbody gameObjectsRigidBody = clearBlock.AddComponent<Rigidbody>(); // make them rigidbodies
			clearBlock.rigidbody.AddForce (pew); // explode them out
			clearBlock.rigidbody.AddTorque (pew); // and spin them right round baby right round
		}

		for (int i = Convert.ToInt32(row)+1; i<21; i++){ // for each remaining row of blocks above the cleared one
			string iTag = i.ToString();
			//Debug.Log ("im totally about to move row "+i+" down a tad.  This be objects with the tag " +iTag);
			GameObject[] moveBlocks = GameObject.FindGameObjectsWithTag (iTag);	// create an array containing each block on that row
			foreach (GameObject moveBlock in moveBlocks) {
				moveBlock.transform.Translate (0,-1,0);	// and move each block down 1 position
				string newTag = (i-1).ToString ();
				moveBlock.tag = newTag;
			}
			int originalRow = i - 1;
			Debug.Log ("about to check to see if there's blocks on row "+i+" and there are "+MainLoop.rows[i]);
			if (MainLoop.rows[i] > 0) { // if there are any blocks on the row above the one I've cleared
				Debug.Log ("i = " + i + " and im correcting row: " + originalRow + " there were " + MainLoop.rows[i] + " blocks on the row above me before");
				MainLoop.rows[i] = MainLoop.rows[originalRow]; //update the count of tiles on each row to the one of that above it in the array
				Debug.Log ("and there's " + MainLoop.rows[i]+" now.");
			}
		}
	}
}
