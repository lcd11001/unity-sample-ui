using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://docs.unity3d.com/ScriptReference/WebCamTexture-videoRotationAngle.html

public class WebCamDisplay : MonoBehaviour {

	// Use this for initialization
	public GameObject picker;
	private Quaternion baseRotation;
	private WebCamTexture wct;
	private bool isPlaying;
	private int centerX, centerY;
	private int camW, camH;

	void Awake () 
	{
		camW = Screen.width;
		camH = Screen.height;

		wct = new WebCamTexture(camW, camH);
		centerX = camW / 2;
		centerY = camH / 2;

		this.GetComponent<MeshRenderer>().material.mainTexture = wct;
		isPlaying = false;
	}

	void Start() 
	{
		baseRotation = transform.rotation;
		// Debug.Log("baseRotation " + baseRotation);
	}

	public void Play()
	{
		wct.Play();
		isPlaying = true;
	}

	public void Stop()
	{
		isPlaying = false;
		wct.Stop();
	}

	private void Update() 
	{
		if (picker && isPlaying)
		{
			Image img = picker.GetComponent<Image>();
			img.color = wct.GetPixel(centerX, centerY);

			transform.rotation = baseRotation * Quaternion.AngleAxis(wct.videoRotationAngle, Vector3.up);
		}
	}
}
