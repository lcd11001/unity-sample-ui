using UnityEngine;
using System.Collections;
 
using Vuforia;
 
public class CameraImageAccess : MonoBehaviour
{
 
    public GameObject picker;
   
    #region PRIVATE_MEMBERS
 
    private Image.PIXEL_FORMAT mPixelFormat = Image.PIXEL_FORMAT.UNKNOWN_FORMAT;
 
    private bool mAccessCameraImage = false;
    private bool mFormatRegistered = false;
 
    #endregion // PRIVATE_MEMBERS
 
    #region MONOBEHAVIOUR_METHODS
 
    void Start()
    {
 
        #if UNITY_EDITOR
        mPixelFormat = Image.PIXEL_FORMAT.GRAYSCALE; // Need Grayscale for Editor
        #else
        mPixelFormat = Image.PIXEL_FORMAT.RGB888; // Use RGB888 for mobile
        #endif
 
        // Register Vuforia life-cycle callbacks:
        VuforiaARController.Instance.RegisterVuforiaStartedCallback(OnVuforiaStarted);
        VuforiaARController.Instance.RegisterTrackablesUpdatedCallback(OnTrackablesUpdated);
        VuforiaARController.Instance.RegisterOnPauseCallback(OnPause);
 
    }
 
    #endregion // MONOBEHAVIOUR_METHODS
 
    #region PRIVATE_METHODS
 
    void OnVuforiaStarted()
    {
 
        // Try register camera image format
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered pixel format " + mPixelFormat.ToString());
 
            mFormatRegistered = true;
        }
        else
        {
            Debug.LogError(
                "\nFailed to register pixel format: " + mPixelFormat.ToString() +
                "\nThe format may be unsupported by your device." +
                "\nConsider using a different pixel format.\n");
 
            mFormatRegistered = false;
        }
 
    }
 
    /// <summary>
    /// Called each time the Vuforia state is updated
    /// </summary>
    void OnTrackablesUpdated()
    {
        if (mFormatRegistered)
        {
            if (mAccessCameraImage)
            {
                // https://library.vuforia.com/content/vuforia-library/en/reference/unity/classVuforia_1_1Image.html
                Vuforia.Image image = CameraDevice.Instance.GetCameraImage(mPixelFormat);
 
                if (image != null)
                {
                    // Debug.Log(
                    //     "\nImage Format: " + image.PixelFormat +
                    //     "\nImage Size:   " + image.Width + "x" + image.Height +
                    //     "\nBuffer Size:  " + image.BufferWidth + "x" + image.BufferHeight +
                    //     "\nImage Stride: " + image.Stride + "\n"
                    // );
 
                    byte[] pixels = image.Pixels;
 
                    if (pixels != null && pixels.Length > 0)
                    {
                        int bytePerPixel = image.Stride / image.Width;
                        int centerX = image.Width >> 1;
                        int centerY = image.Height >> 1;
                        int idx = (centerY * image.Width + centerX) * bytePerPixel;

                        float r = pixels[idx + 0] * 1.0f / 255;
                        float g = pixels[idx + 1] * 1.0f / 255;
                        float b = pixels[idx + 2] * 1.0f / 255;

                        Color c = new Color(r, g, b);

                        // Debug.Log("\nImage pixels: " + c);

                        ApplyColor(c);
                    }
                }
            }
        }
    }
 
    /// <summary>
    /// Called when app is paused / resumed
    /// </summary>
    void OnPause(bool paused)
    {
        if (paused)
        {
            Debug.Log("App was paused");
            UnregisterFormat();
        }
        else
        {
            Debug.Log("App was resumed");
            RegisterFormat();
        }
    }
 
    /// <summary>
    /// Register the camera pixel format
    /// </summary>
    void RegisterFormat()
    {
        if (CameraDevice.Instance.SetFrameFormat(mPixelFormat, true))
        {
            Debug.Log("Successfully registered camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = true;
        }
        else
        {
            Debug.LogError("Failed to register camera pixel format " + mPixelFormat.ToString());
            mFormatRegistered = false;
        }
    }
 
    /// <summary>
    /// Unregister the camera pixel format (e.g. call this when app is paused)
    /// </summary>
    void UnregisterFormat()
    {
        Debug.Log("Unregistering camera pixel format " + mPixelFormat.ToString());
        CameraDevice.Instance.SetFrameFormat(mPixelFormat, false);
        mFormatRegistered = false;
    }


    private void ApplyColor(Color c)
	{
		if (picker)
		{
			UnityEngine.UI.Image img = picker.GetComponent<UnityEngine.UI.Image>();
			img.color = c;
		}
	}
 
    #endregion //PRIVATE_METHODS

    #region PUBLIC_METHODS
    public void SetAccessCameraImage(bool enable)
    {
        mAccessCameraImage = enable;
    }
    #endregion //PUBLIC_METHODS
}