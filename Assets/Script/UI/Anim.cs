using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

abstract public class Anim
{
    protected MenuItem item;
    protected float deltaTime, duration;
    protected float offsetRadius, fromRadius, toRadius;
    protected float fromAlpha, toAlpha;
    protected float fromAngle, toAngle;
    protected float offsetAngle;
    protected bool isRunning;

    protected float ratio, radius, alpha, angle;

    abstract public bool StartAnim(MenuItem item);
    abstract protected void UpdateAnim(float radius, float alpha, float angle);
    abstract protected void StopAnim();

    virtual public bool StartAnim(MenuItem item, float duration)
    {
        this.duration = duration;
        return StartAnim(item);
    }

    public float Ratio
    {
        get {
            return ratio;
        }
    }

    public float Radius
    {
        get {
            return radius;
        }
    }

    public float Alpha
    {
        get {
            return alpha;
        }
    }

    public float Angle
    {
        get {
            return angle;
        }
    }

    public void Update() 
    {
        if (isRunning)
		{
			deltaTime += Time.deltaTime;
			
			ratio = deltaTime / duration;
            radius = Mathf.Lerp(fromRadius, toRadius, ratio); 
			alpha = Mathf.Lerp(fromAlpha, toAlpha, ratio);
            angle = Mathf.LerpAngle(fromAngle, toAngle, ratio);

			if (ratio < 1)
			{
				UpdateAnim(radius, alpha, angle);
			}
			else
			{
				StopAnim();
			}
		}
    }

    public void Abort()
    {
        isRunning = false;
        // for removing this anim from the list
        ratio = 1;
    }

    protected void MakeClickable(MenuItem item, bool check, bool useRecursive = false)
    {
        if (item.isSubButtonActive)
        {
            if (item.subButton)
            {
                MakeClickable(item.subButton.GetComponent<CanvasGroup>(), check, useRecursive);
            }
            MakeClickable(item.button.GetComponent<CanvasGroup>(), false, useRecursive);
        }
        else
        {
            MakeClickable(item.button.GetComponent<CanvasGroup>(), check, useRecursive);
            if (!isRunning && item.subButton)
            {
                MakeClickable(item.subButton.GetComponent<CanvasGroup>(), false, useRecursive);
            }
        }
    }
    
    private void MakeClickable(CanvasGroup canvas, bool check, bool useRecursive)
	{
		if (canvas == null)
		{
			return;
		}

		canvas.interactable = check;
		canvas.blocksRaycasts = check;

		if (useRecursive)
		{
			CanvasGroup[] children = canvas.GetComponentsInChildren<CanvasGroup>(true);
			foreach(CanvasGroup child in children)
			{
				MakeClickable(child, check, true);
			}
		}
	}

    protected void MakeAlpha(MenuItem item, float alpha, bool revert)
    {
        if (item.isSubButtonActive)
        {
            if (item.subButton)
            {
                MakeAlpha(item.subButton.GetComponent<CanvasGroup>(), alpha, revert);
            }
        }
        else
        {
            MakeAlpha(item.button.GetComponent<CanvasGroup>(), alpha, revert);
        }
        
    }

	private void MakeAlpha(CanvasGroup canvas, float alpha, bool revert)
	{
		if (canvas == null)
		{
			return;
		}

		if (revert)
		{
			canvas.alpha = 1 - alpha;
		}
		else
		{
			canvas.alpha = alpha;
		}
	}

	protected Vector3 MakePos(float radius, float radian)
	{
		Vector3 pos = Vector3.zero;
		pos.x = Mathf.Cos(radian) * radius;
		pos.y = Mathf.Sin(radian) * radius;

		return pos;
	}
}