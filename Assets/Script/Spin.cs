using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float speed = 1f;
    float x, y, z;

    // Use this for initialization
    void Start()
    {
		Vector3 v = transform.localRotation.eulerAngles;
        x = v.x;
        y = v.y;
        z = v.z;
    }

    // Update is called once per frame
    void Update()
    {
		float delta = speed * Time.deltaTime;
        x = (x + delta) % 360;
		y = (y + delta) % 360;
		z = (z + delta) % 360;
        transform.localRotation = Quaternion.Euler(x, y, z);
    }
}
