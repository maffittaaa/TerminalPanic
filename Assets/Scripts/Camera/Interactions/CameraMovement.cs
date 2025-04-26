using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject cameraViewPoint;
    [SerializeField] private GameObject player;
    [SerializeField] private float sensivity = 15f;
    [SerializeField] private AnxietyBar anxietyBar;
    float rotationX = 0;
    float rotationY = 0;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!anxietyBar.interacted)
        {
            MouseMovement();
        }
    }

    private void MouseMovement()
    {
        Cursor.visible = false;
        MoveAroundWithMouse();
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

