using UnityEngine;
using System.Collections;

public class CreateBounds : MonoBehaviour {

	//Area size
	public float sizeX;
	public float sizeY;

	public bool LeftActive = true;
	public bool RightActive = true;
	public bool UpActive = true;
	public bool DownActive = true;

	public Vector3 position = Vector3.zero;
	public bool update = false;
	GameObject[] bounds = new GameObject[4];

	void OnValidate(){
		sizeX = Mathf.Clamp (sizeX, 1, 10000);
		sizeY = Mathf.Clamp (sizeY, 1, 10000);
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(position, new Vector3(position.x, position.y - sizeY, 0)); // Left 
		Gizmos.DrawLine(position, new Vector3(position.x + sizeX, position.y, 0)); // Up 
		Gizmos.DrawLine(new Vector3(position.x, position.y - sizeY, 0), new Vector3(position.x + sizeX, position.y - sizeY, 0)); // Down
		Gizmos.DrawLine(new Vector3(position.x + sizeX, position.y, 0), new Vector3(position.x + sizeX, position.y - sizeY, 0)); // Left side
		 
	}

	// Use this for initialization
	void Start () {
		transform.position = position;
		Create ();
	}

	void Update(){
		if (update)
			Refresh ();

		bounds [0].SetActive (LeftActive);
		bounds [1].SetActive (RightActive);
		bounds [2].SetActive (UpActive);
		bounds [3].SetActive (DownActive);
	}
	void Create(){
		
		for (int i = 0; i < bounds.Length; i++) {
			if(bounds[i] != null)
				Destroy (bounds [i]);
		}

		GameObject leftBounds = new GameObject ();
		leftBounds.layer = this.gameObject.layer;
		leftBounds.AddComponent<BoxCollider2D> ();
		leftBounds.transform.position = new Vector3(-0.5f, 0f, 0f) + position;
		leftBounds.GetComponent<BoxCollider2D> ().size = new Vector2 (1f, sizeY+2f);
		leftBounds.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, -sizeY / 2);
		leftBounds.transform.parent = gameObject.transform;
		leftBounds.name = "Left Bounds";
		bounds [0] = leftBounds;

		GameObject rightBounds = new GameObject ();
		rightBounds.layer = this.gameObject.layer;
		rightBounds.AddComponent<BoxCollider2D> ();
		rightBounds.transform.position = new Vector3(sizeX + 0.5f, 0f, 0f) + position;
		rightBounds.GetComponent<BoxCollider2D> ().size = new Vector2 (1f, sizeY+2f);
		rightBounds.GetComponent<BoxCollider2D> ().offset = new Vector2 (0, -sizeY / 2);
		rightBounds.transform.parent = gameObject.transform;
		rightBounds.name = "Right Bounds";
		bounds [1] = rightBounds;

		GameObject topBounds = new GameObject ();
		topBounds.layer = this.gameObject.layer;
		topBounds.AddComponent<BoxCollider2D> ();
		topBounds.transform.position = new Vector3(0f, 0.5f, 0f) + position;
		topBounds.GetComponent<BoxCollider2D> ().size = new Vector2 (sizeX+2f, 1f);
		topBounds.GetComponent<BoxCollider2D> ().offset = new Vector2 (sizeX / 2, 0);
		topBounds.transform.parent = gameObject.transform;
		topBounds.name = "Top Bounds";
		bounds [2] = topBounds;

		GameObject bottomBounds = new GameObject ();
		bottomBounds.layer = this.gameObject.layer;
		bottomBounds.AddComponent<BoxCollider2D> ();
		bottomBounds.transform.position = new Vector3(0f, -sizeY-0.5f, 0f) + position;
		bottomBounds.GetComponent<BoxCollider2D> ().size = new Vector2 (sizeX+2f, 1f);
		bottomBounds.GetComponent<BoxCollider2D> ().offset = new Vector2 (sizeX / 2, 0);
		bottomBounds.transform.parent = gameObject.transform;
		bottomBounds.name = "Bottom Bounds";
		bounds [3] = bottomBounds;
	}

	void Refresh(){
		for (int i = 0; i < bounds.Length; i++) {
			if (bounds [i] == null) {
				Create ();
				break;
			}
		}
		bounds[0].transform.position = new Vector3(-0.5f, 0f, 0f) + position;
		bounds[0].GetComponent<BoxCollider2D> ().size = new Vector2 (1f, sizeY);
		bounds[0].GetComponent<BoxCollider2D> ().offset = new Vector2 (0, -sizeY / 2);

		bounds[1].transform.position = new Vector3(sizeX + 0.5f, 0f, 0f) + position;
		bounds[1].GetComponent<BoxCollider2D> ().size = new Vector2 (1f, sizeY);
		bounds[1].GetComponent<BoxCollider2D> ().offset = new Vector2 (0, -sizeY / 2);

		bounds[2].transform.position = new Vector3(0f, 0.5f, 0f) + position;
		bounds[2].GetComponent<BoxCollider2D> ().size = new Vector2 (sizeX, 1f);
		bounds[2].GetComponent<BoxCollider2D> ().offset = new Vector2 (sizeX / 2, 0);

		bounds[3].transform.position = new Vector3(0f, -sizeY-0.5f, 0f) + position;
		bounds[3].GetComponent<BoxCollider2D> ().size = new Vector2 (sizeX, 1f);
		bounds[3].GetComponent<BoxCollider2D> ().offset = new Vector2 (sizeX / 2, 0);
	}
}
