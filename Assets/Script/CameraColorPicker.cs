using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraColorPicker : MonoBehaviour {

	// Use this for initialization
	public GameObject picker;

	public void ApplyColor()
	{
		Image matPicker = picker.GetComponent<Image>();
		Material mat = this.gameObject.GetComponent<MeshRenderer>().material;
		
		mat.color = matPicker.color;
	}
}
