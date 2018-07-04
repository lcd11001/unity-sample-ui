using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebCamDisplay : MonoBehaviour {

	// Use this for initialization
	private WebCamTexture wct;

	void Start () 
	{
		wct = new WebCamTexture();
		this.GetComponent<MeshRenderer>().material.mainTexture = wct;
	}

	public void Play()
	{
		wct.Play();
	}

	public void Stop()
	{
		wct.Stop();
	}
}
