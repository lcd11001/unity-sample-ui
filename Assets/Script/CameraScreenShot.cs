using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CameraScreenShot : MonoBehaviour
{
    public GameObject objFlashing;
    public float duration;
    public float startAlpha;
    public float endAlpha;
    // Update is called once per frame

    private float timer, midDuration;
    private float percent;
    private bool isRunning;
    private enum FLASH {
        START_TO_END,
        ZERO,
        END_TO_START
    }

    private FLASH status;

    public void TakeScreenShot()
    {
        timer = 0;
        midDuration = duration / 2;
        isRunning = true;
        status = FLASH.START_TO_END;
    }

    private void Save()
    {
        // ScreenCapture.CaptureScreenshot("AR-" + date + ".png");

        Debug.Log("platform = " + Application.platform);
        if (Application.platform == RuntimePlatform.Android)
        {
            StartCoroutine(SaveToAndroid());
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            StartCoroutine(SaveToIos());
        }
        else
        {
            StartCoroutine(SaveToPC());
        }
        
    }

    private IEnumerator SaveToAndroid()
    {
        yield return new WaitForEndOfFrame();

        Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();

        string date = System.DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
        string path = CameraScreenShotAndroid.SaveImageToGallery(tex, "AR-" + date + ".jpg", "AR Car Demo Screenshot");
        Debug.Log("Image is saved to " + path);

        Object.Destroy(tex);
    }

    private IEnumerator SaveToIos()
    {
        yield return new WaitForEndOfFrame();
        // https://www.quora.com/How-do-you-call-native-iOS-functions-from-Unity
        // https://stackoverflow.com/questions/30938859/unity-receive-event-from-object-c/30946257#30946257
        int sum = CameraScreenShotIos.Add(5, 4);
        Debug.Log("result " + sum);
    }

    private IEnumerator SaveToPC()
    {
        // https://docs.unity3d.com/ScriptReference/ScreenCapture.CaptureScreenshotAsTexture.html
        yield return new WaitForEndOfFrame();

        Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        byte[] bytes = ImageConversion.EncodeToJPG(tex, 100);

        string date = System.DateTime.UtcNow.ToString("yyyyMMdd-HHmmss");
        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../" + "AR-" + date + ".jpg", bytes);
        
        Object.Destroy(tex);
    }

    private void Stop()
    {
        isRunning = false;
        ApplyAlpha(startAlpha);
    }

    void Update()
    {
        if (isRunning)
        {
            timer += Time.deltaTime;

            if (timer < duration)
            {
                percent = timer / midDuration;

                float alpha = 0;
                
                switch(status)
                {
                    case FLASH.START_TO_END:
                        alpha = Mathf.Lerp(startAlpha, endAlpha, percent);
                        if (percent >= 1)
                        {
                            status = FLASH.ZERO;
                        }
                    break;

                    case FLASH.END_TO_START:
                        alpha = Mathf.Lerp(endAlpha, startAlpha, percent - 1);
                    break;

                    case FLASH.ZERO:
                        Save();
                        status = FLASH.END_TO_START;
                    break;
                }

                ApplyAlpha(alpha);
            }
            else
            {
                Stop();
            }
        }
    }

    void ApplyAlpha(float alpha)
    {
        CanvasGroup canvas = objFlashing.GetComponent<CanvasGroup>();
        if (canvas)
        {
            // Debug.Log("alpha " + alpha);
            canvas.alpha = alpha;
        }
        else
        {
            Debug.LogWarning("object " + objFlashing.name + " should have CanvasGroup component");
        }
    }
}
