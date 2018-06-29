using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimCollapse  : Anim 
{
    override public bool StartAnim(MenuItem item)
    {
        if (isRunning || item.isOpened == false)
        {
            return false;
        }

        this.item = item;

        isRunning = true;
        deltaTime = 0;
        ratio = 0;
        duration = item.duration;

        fromRadius = item.endRadius;
        toRadius = item.startRadius;

		fromAlpha = item.endAlpha;
		toAlpha = item.startAlpha;

        deltaAngle = Mathf.PI * 2 / item.children.Count;
        offsetAngle = item.angle * Mathf.PI / 180;

        UpdateAnim(fromRadius, fromAlpha, 0);

        return true;
    }

    override protected void UpdateAnim(float radius, float alpha, float angle)
    {
        int i = 0;
        foreach(MenuItem child in item.children)
        {
            Vector3 pos = MakePos(radius, i * deltaAngle + offsetAngle);
            child.button.transform.localPosition = pos;

            CanvasGroup canvasFadeOut = child.button.GetComponent<CanvasGroup>();
            MakeAlpha(canvasFadeOut, alpha, false);

            i ++;
        }
    }

    override protected void StopAnim()
    {
        isRunning = false;
        item.isOpened = false;

        UpdateAnim(toRadius, toAlpha, 0);

        foreach(MenuItem child in item.children)
        {
            MakeClickable(child.button.GetComponent<CanvasGroup>(), false);
        }
    }
}