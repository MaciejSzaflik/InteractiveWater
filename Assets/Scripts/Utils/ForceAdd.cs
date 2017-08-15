using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceAdd : MonoBehaviour {

	public Vector3 direction = Vector3.down;
	public float force = 100;

	public float delayedStart = 1;

	void Start () {
		StartCoroutine(ApplyForce());
	}

	private IEnumerator ApplyForce()
	{
		yield return new WaitForSeconds(delayedStart);
		Rigidbody rigidBody = gameObject.GetComponent<Rigidbody>();
		rigidBody.AddForce(direction*force);
	}
	

}
