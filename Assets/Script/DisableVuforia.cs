using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using UnityEngine.XR;

public class DisableVuforia : MonoBehaviour {

	// Use this for initialization
	public Camera AR_Camera;
	void Start () {
		Setup(false);
	}

	public void Setup(bool enable)
	{
		StartCoroutine(_Setup(enable));
	}

	private IEnumerator _Setup(bool enable)
	{
		Debug.Log ("DelayFunction");
		yield return new WaitForSeconds(3f); // waits 3 seconds

		Debug.Log ("DelayFunction after 3 seconds");

		XRSettings.enabled = enable;
		if (VuforiaBehaviour.Instance)
		{
			VuforiaBehaviour.Instance.enabled = enable;
		}
		if (AR_Camera)
		{
			AR_Camera.gameObject.SetActive(enable);
			AR_Camera.enabled = enable;
		}
	}
}
