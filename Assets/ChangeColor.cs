using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

	public void Change_Color(string colorRGB)
	{
		Material mat = this.gameObject.GetComponent<MeshRenderer>().material;
		int colorHexaRGB = System.Int32.Parse(colorRGB.ToLower().Replace("0x", ""), System.Globalization.NumberStyles.HexNumber);
		int r = (colorHexaRGB >> 16) & 0xFF;
		int g = (colorHexaRGB >> 8) & 0xFF;
		int b = (colorHexaRGB & 0xFF);
		mat.color = new Color(r, g, b);
	}
}
