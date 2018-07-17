using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour {
	public float speed = 20f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Time.deltaTime * speed, Time.deltaTime * speed * 2, Time.deltaTime * speed * 3);
	}
}
