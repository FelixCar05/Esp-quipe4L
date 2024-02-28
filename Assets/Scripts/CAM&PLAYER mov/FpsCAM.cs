using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsCAM : MonoBehaviour
{
    //orientation stuff
    [SerializeField] Transform orientation;

    [SerializeField] float sensX = 0;
    [SerializeField] float sensY = 0;

    float xRotation = 0;
    float yRotation = 0;


    //position stuff
    [SerializeField] Transform cameraPos;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void Update()
    {
        //get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        //rotate the camera and store rotation in orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);


        //move camera
        transform.position = cameraPos.position;
    }
}
