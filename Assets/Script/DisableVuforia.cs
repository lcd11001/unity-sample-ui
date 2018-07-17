using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.XR;

public class DisableVuforia : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		Setup(false);
	}

	public void Setup(bool enable)
	{
		XRSettings.enabled = enable;
		if (VuforiaBehaviour.Instance)
		{
			VuforiaBehaviour.Instance.enabled = enable;
		}
	}
}
