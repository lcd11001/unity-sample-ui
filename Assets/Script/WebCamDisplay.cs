using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// https://docs.unity3d.com/ScriptReference/WebCamTexture-videoRotationAngle.html

public class WebCamDisplay : MonoBehaviour
{

    // Use this for initialization
    public GameObject picker;
    private Quaternion baseRotation;
    private WebCamTexture wct;
    private bool isPlaying;
    private int centerX, centerY;
    private int camW, camH;
    private bool camEnable;
    private bool hasMirrorY;

#if UNITY_EDITOR
    private Color camFakeColor;
    private float camTimer;
#endif

    void Awake()
    {
        camEnable = WebCamTexture.devices.Length > 0;
        if (camEnable)
        {
            camW = Screen.width;
            camH = Screen.height;

            wct = new WebCamTexture(camW, camH);
            centerX = camW / 2;
            centerY = camH / 2;

            this.GetComponent<MeshRenderer>().material.mainTexture = wct;
        }

        isPlaying = false;

#if UNITY_EDITOR
        camFakeColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
#endif
    }

    void Start()
    {
        baseRotation = transform.rotation;
        // Debug.Log("baseRotation " + baseRotation);
    }

    public void Play()
    {
        if (camEnable)
        {
            wct.Play();
            isPlaying = true;
        }

        hasMirrorY = false;

#if UNITY_EDITOR
        camTimer = 0;
#endif
    }

    public void Stop()
    {
        if (camEnable)
        {
            isPlaying = false;
            wct.Stop();
        }

        if (hasMirrorY)
        {
            Vector3 mirrorY = transform.localScale;
            mirrorY.z *= -1;
            transform.localScale = mirrorY;
            hasMirrorY = false;
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (!camEnable)
        {
            camTimer += Time.deltaTime;
            if (camTimer > 3)
            {
                camFakeColor = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
                ApplyColor(camFakeColor);

                camTimer = 0;
            }

            return;
        }
#endif
        if (isPlaying)
        {
            ApplyColor(wct.GetPixel(centerX, centerY));

            transform.rotation = baseRotation * Quaternion.AngleAxis(wct.videoRotationAngle, Vector3.up);

            if (wct.videoVerticallyMirrored && !hasMirrorY)
            {
                Vector3 mirrorY = transform.localScale;
                mirrorY.z *= -1;
                transform.localScale = mirrorY;
                hasMirrorY = true;
            }
        }
    }

    private void ApplyColor(Color c)
    {
        if (picker)
        {
            Image img = picker.GetComponent<Image>();
            img.color = c;
        }
    }
}
