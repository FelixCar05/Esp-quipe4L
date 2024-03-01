using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//optimisation limiter les normalized

public class MouvementPersonnage : MonoBehaviour

{

    [Header("Movement")]
    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    public float vitesseMarche = 100;
    public float airMultiplier = 0.5f;

    public Rigidbody rb;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask ThisIsGround;

    public float groundDrag;
    public bool grounded;

    public float forceJump = 7;
    public float forceDash = 12;
    public float jumpCoolDown;//adjust cooldown for float bug
    bool readyToJump = true;  //public float airMultiplier;

    //controls

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;
 



    private void start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void myInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal1");
        verticalInput = Input.GetAxisRaw("Vertical1");

        if (grounded & readyToJump & Input.GetKey(jumpKey))//fucké
        {
            readyToJump = false;

           
            rb.AddForce(Vector3.up * forceJump, ForceMode.Impulse);
            

            Invoke(nameof(ResetJump), jumpCoolDown);//continuous jump as key held down
        }
    }

    private void movePlayer()//devrait utiliser le input system de unity
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;



        if (grounded)//ground move
            rb.AddForce(moveDirection.normalized * vitesseMarche * 10f, ForceMode.Force);

        else//air move
            rb.AddForce(moveDirection.normalized * vitesseMarche * 10f * airMultiplier, ForceMode.Force);


        if (Input.GetKeyDown(KeyCode.Mouse3))//dash
        {
            rb.AddForce(moveDirection.normalized * forceDash, ForceMode.Impulse);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse3))//dash return to normal
        {
            rb.AddForce(moveDirection.normalized * vitesseMarche, ForceMode.VelocityChange);
        }


        if (Input.GetKeyDown(KeyCode.Mouse4))//ground slam
        {
            rb.AddForce(Vector3.down * forceJump * 20f, ForceMode.Force);
        }
    }
    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatvel.magnitude > vitesseMarche)
        {
            Vector3 limitedvel = flatvel.normalized * vitesseMarche;
            rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }

    void Start()
    {

    }
    void Update()
    {
        myInput();
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ThisIsGround);

        rb.drag = grounded ? groundDrag : 1;
    }

    private void FixedUpdate()
    {
        movePlayer();
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
