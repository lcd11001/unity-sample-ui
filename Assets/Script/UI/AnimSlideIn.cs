using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimSlideIn  : Anim 
{
    override public bool StartAnim(MenuItem item)
    {
        if (isRunning || item.isOpened == false || item.children.Count == 0)
        {
            return false;
        }

        this.item = item;

        isRunning = true;
        deltaTime = 0;
        ratio = 0;

        offsetRadius = item.startRadius;
        fromRadius = item.endRadius - offsetRadius;
        toRadius = 0;
        duration = item.duration;

		fromAlpha = item.endAlpha;
		toAlpha = item.startAlpha;

        UpdateAnim(fromRadius, fromAlpha, 0);

        MakeClickable(item, false);

        return true;
    }

    override protected void UpdateAnim(float radius, float alpha, float angle)
    {
        int i = 0;
        int size = item.children.Count;
        foreach(MenuItem child in item.children)
        {
            float ratio = size > 1 
                ? i * 1.0f / (size - 1)
                : 1;
            Vector3 pos = (radius * ratio + offsetRadius) * item.direction;
            child.button.transform.localPosition = pos;

            MakeAlpha(child, alpha, false);

            i ++;
        }
    }

    override protected void StopAnim()
    {
        isRunning = false;
        item.isOpened = false;

        UpdateAnim(toRadius, toAlpha, 0);

        MakeClickable(item, true);

        foreach(MenuItem child in item.children)
        {
            MakeClickable(child, false);
        }
    }
}