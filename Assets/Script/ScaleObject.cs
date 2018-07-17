using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour {

	// Use this for initialization
	public void Scale(float ratio)
	{
		transform.localScale = Vector3.one * ratio;
	}
}
