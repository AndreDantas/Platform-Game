using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider2D))]
public class CheckCollision : MonoBehaviour {

	Collider2D colliderRef;
	public PhysicsMaterial2D material;
	public bool collision = false;
	public GameObject col;

	void Start(){
		
		colliderRef = GetComponent<Collider2D> ();
		if (material != null)
			colliderRef.sharedMaterial = material;
	}

	void OnCollisionEnter2D(Collision2D col){
		collision = true;
		this.col = col.transform.root.gameObject;
	}

	void OnCollisionStay2D(Collision2D col){
		collision = true;
		this.col = col.transform.root.gameObject;
	}

	void OnCollisionExit2D(Collision2D col){
		collision = false;
		this.col = null;
	}
	void OnTriggerEnter2D(Collider2D col){
		collision = true;
		this.col = col.transform.root.gameObject;
	}

	void OnTriggerStay2D(Collider2D col){
		collision = true;
		this.col = col.transform.root.gameObject;
	}

	void OnTriggerExit2D(Collider2D col){
		collision = false;
		this.col = null;
	}
}
