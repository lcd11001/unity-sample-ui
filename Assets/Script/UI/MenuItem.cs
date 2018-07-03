using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuItem
{
	public Button button;
    public Button subButton;
	public bool isSubButtonActive;
	public bool isOpened;
	public List<MenuItem> children;
	public float duration;
	public float startRadius, endRadius;
	public float startAlpha, endAlpha;
	public float startAngle, endAngle;
	public Vector3 direction;
	public float angle;
}