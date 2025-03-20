using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject cameraViewPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private int height;
    [SerializeField] private float sensivity = 15f;

    float rotationX = 0;
    float rotationY = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Cursor.visible = false;
        //gameObject.transform.position = new Vector3(cameraViewPoint.transform.position.x, cameraViewPoint.transform.position.y + height, cameraViewPoint.transform.position.z - (height + height + height));
        MoveAroundWithMouse();
    }

    private void MoveAroundWithMouse()
    {
        rotationY += Input.GetAxis("Mouse X") * sensivity;
        rotationX += Input.GetAxis("Mouse Y") * -1 * sensivity;
        rotationX = (Mathf.Clamp(rotationX, -90, 60));
        transform.localEulerAngles = new Vector3 (rotationX, rotationY, 0);
        player.transform.localEulerAngles = new Vector3 (player.transform.localEulerAngles.x, rotationY, player.transform.localEulerAngles.z);
    }
}
