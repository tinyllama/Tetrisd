using UnityEngine;
using System.Collections;
using System; // needed for Array.Clear

public class PieceDrop : MonoBehaviour {
	
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
	
	// Use this for initialization
	void Start () {
		//position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		//Quit:
		if (Input.GetKeyDown ("escape")) {
			Application.LoadLevel ("Menu");
		}
		
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
			GameOver();
		}
		dropComplete = true; // spawn the next piece
	}
	
	void ClearRow(string row){
		Debug.Log ("YO ITS TIME TO CLEAR OUT ROW: " + row);
		//apply force to the row in question
		GameObject[] clearBlocks = GameObject.FindGameObjectsWithTag (row);
		foreach (GameObject pushBlock in clearBlocks ){
			Rigidbody gameObjectsRigidBody = pushBlock.AddComponent<Rigidbody>();
			pushBlock.rigidbody.AddForce (0,0,-10);
		}
		
		//foreach
		
		
		foreach (GameObject pushBlock in clearBlocks ){
			//pushBlock.rigidbody.AddTorque (3,3,3);
		}
	}
	void GameOver() {
		Array.Clear(MainLoop.rows, 0, MainLoop.rows.Length); // clear array values so they're empty for next game
		Application.LoadLevel ("Menu");	
	}
}
