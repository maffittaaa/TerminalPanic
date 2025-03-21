using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject cameraViewPoint;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float sensivity = 15f;

    float rotationX = 0;
    float rotationY = 0;

    private LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Default");
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Cursor.visible = false;
        MoveAroundWithMouse();

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        shootingPoint.transform.position = hit.point;
    }

    private void MoveAroundWithMouse()
    {
        rotationY += Input.GetAxis("Mouse X") * sensivity;
        rotationX += Input.GetAxis("Mouse Y") * -1 * sensivity;
        rotationX = Mathf.Clamp(rotationX, -90, 60);
        transform.localEulerAngles = new Vector3(rotationX, 0, 0);
        player.transform.localEulerAngles = new Vector3(player.transform.localEulerAngles.x, rotationY, player.transform.localEulerAngles.z);
    }
}
