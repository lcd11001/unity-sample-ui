using UnityEngine;
// We need this one for importing our IOS functions
using System.Runtime.InteropServices;

public class CameraScreenShotIos
{
#if UNITY_IPHONE
    [DllImport("__Internal")]
    private static extern void _SaveSnapShot(byte[] bytes, int len);
#endif

    public static void SaveSnapShot(byte[] bytes, int len)
    {
        // We check for UNITY_IPHONE again so we don't try this if it isn't iOS platform.
#if UNITY_IPHONE
        // Now we check that it's actually an iOS device/simulator, not the Unity Player. You only get plugins on the actual device or iOS Simulator.
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            _SaveSnapShot(bytes, len);
        }
#endif
    }
}