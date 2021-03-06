﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimFadeIn  : Anim 
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
		
		fromAlpha = item.startAlpha;
		toAlpha = item.endAlpha;

		UpdateAnim(0, fromAlpha, 0);
		
		MakeClickable(item, false);

		return true;
	}

	override protected void UpdateAnim(float radius, float alpha, float angle)
	{
		CanvasGroup canvasFadeIn = item.button.GetComponent<CanvasGroup>();
		canvasFadeIn.alpha =  alpha;
		
		CanvasGroup canvasFadeOut = item.subButton.GetComponent<CanvasGroup>();
		canvasFadeOut.alpha = 1 - alpha;
	}

	override protected void StopAnim()
	{
		isRunning = false;

		UpdateAnim(0, toAlpha, 0);

		item.isSubButtonActive = false;
		MakeClickable(item, true);
	}
}
