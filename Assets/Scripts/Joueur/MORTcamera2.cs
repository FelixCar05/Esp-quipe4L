using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;



public class camera2 : MonoBehaviour
{
    [SerializeField]
    Transform Joueur;
    [SerializeField]
     float sensibilitéSourisVerticale = 2f;
    [SerializeField]
    float sensibilitéSourisHorizontale = 3f;

     float rotationVerticale = 2f;

  
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {

        float AxeX = Input.GetAxis("Mouse X") * sensibilitéSourisHorizontale;
        float AxeY = Input.GetAxis("Mouse Y") * sensibilitéSourisVerticale;


        rotationVerticale -= AxeY;
        rotationVerticale = Mathf.Clamp(rotationVerticale, -90f, 90f);
        transform.localEulerAngles = Vector3.right * rotationVerticale;



        Joueur.Rotate(Vector3.up * AxeX);

    }

}

