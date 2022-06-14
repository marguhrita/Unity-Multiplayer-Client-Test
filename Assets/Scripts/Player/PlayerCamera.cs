using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public float sensitivity;
    public Transform orientation;

    [SerializeField] Camera cam;

    float xRotation;
    float yRotation;

    private void Start()
    {
        disableCursor();

    }

    private void Update()
    {

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        cam.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

    }

    public void disableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void enableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    

}
