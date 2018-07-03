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

		MakeClickable(item, false);

		return true;
	}

	override protected void UpdateAnim(float radius, float alpha, float angle)
	{
		CanvasGroup canvasFadeOut = item.button.GetComponent<CanvasGroup>();
		canvasFadeOut.alpha =  alpha;
		
		CanvasGroup canvasFadeIn = item.subButton.GetComponent<CanvasGroup>();
		canvasFadeIn.alpha = 1 - alpha;
	}

	override protected void StopAnim()
	{
		isRunning = false;

		UpdateAnim(0, toAlpha, 0);

		item.isSubButtonActive = true;
		MakeClickable(item, true);
	}
	
}
