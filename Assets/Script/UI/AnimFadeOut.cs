using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimFadeOut  : Anim 
{
	override public bool StartAnim(MenuItem item)
	{
		if (isRunning)
		{
			return false;
		}

		this.item = item;

		isRunning = true;
		deltaTime = 0;
		ratio = 0;
		duration = item.duration;

		fromAlpha = item.endAlpha;
		toAlpha = item.startAlpha;

		UpdateAnim(0, fromAlpha, 0);

		CanvasGroup canvasFadeOut = item.button.GetComponent<CanvasGroup>();
		MakeClickable(canvasFadeOut, false);

		return true;
	}

	override protected void UpdateAnim(float radius, float alpha, float angle)
	{
		CanvasGroup canvasFadeOut = item.button.GetComponent<CanvasGroup>();
		MakeAlpha(canvasFadeOut, alpha, false);
		
		CanvasGroup canvasFadeIn = item.subButton.GetComponent<CanvasGroup>();
		MakeAlpha(canvasFadeIn, alpha, true);
	}

	override protected void StopAnim()
	{
		isRunning = false;

		UpdateAnim(0, toAlpha, 0);

		CanvasGroup canvasFadeIn = item.subButton.GetComponent<CanvasGroup>();
		MakeClickable(canvasFadeIn, true);
	}
	
}
