using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimRotate  : Anim 
{
    float localRadius;
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

        fromAngle = item.startAngle;
        toAngle = item.endAngle;

        localRadius = Vector3.Magnitude(item.button.transform.localPosition);

        UpdateAnim(0, 0, fromAngle);

        MakeClickable(item.button.GetComponent<CanvasGroup>(), false);

        return true;
    }

    override protected void UpdateAnim(float radius, float alpha, float angle)
    {
        float radian = angle * Mathf.PI / 180;
        Vector3 pos = MakePos(localRadius, radian);
        item.button.transform.localPosition = pos;

        if (item.subButton)
        {
            item.subButton.transform.localPosition = pos;
        }
    }

    override protected void StopAnim()
    {
        isRunning = false;

        UpdateAnim(0, 0, toAngle);

        MakeClickable(item.button.GetComponent<CanvasGroup>(), true);
    }
}