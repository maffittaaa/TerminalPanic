using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHead : MonoBehaviour
{
    [SerializeField] private GameObject point;
    [SerializeField] private float speed;

    private void Update()
    {
        Quaternion a = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w);
        Quaternion b = Quaternion.Euler(point.transform.localEulerAngles.x, point.transform.localEulerAngles.y, transform.localEulerAngles.z);
        Quaternion newRot = Quaternion.Slerp(a, b, speed);
        transform.rotation = newRot;
    }
}
