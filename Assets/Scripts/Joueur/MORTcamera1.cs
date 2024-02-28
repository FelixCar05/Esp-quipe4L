using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class camera1 : MonoBehaviour
{
    //todo:
    //clean up
    //better fps
    //less ifs
    //better feel
    //more control
    //dash + iframes




    //clean this shit upp
    [Header("Movement")]
    public float moveSpeed;
    public float dashSpeed;

    public Transform orientation;

    public float jumpForce;
    public float jumpCoolDown;//adjust cooldown for float bug
    public float airMultiplier;
    bool readyToJump = true;

    [Header("KeyBinds")]
    public KeyCode jumpKey = KeyCode.Space;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public float groundDrag;
    public bool grounded;

    [Header("Ground Check")]
    public float height;
    public LayerMask ThisIsGround;



    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 140;
        QualitySettings.vSyncCount = 0;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }



    // Update is called once per frame
    void Update()
    {

        //grounded = Physics.Raycast(transform.position, Vector3.down, height * 0.5f + 0.2f);//cela peut etre ce qui cause le lag
        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }


    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


        if (grounded)//ground move
            rb.AddForce(moveDirection.normalized * moveSpeed * 5f, ForceMode.Force);

        else//air move
            rb.AddForce(moveDirection.normalized * moveSpeed * 5f * airMultiplier, ForceMode.Force);

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (grounded & readyToJump & Input.GetKey(jumpKey))//fucké
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCoolDown);//continuous jump as key held down
        }
    }

    private void SpeedControl()
    {
        Vector3 flatvel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatvel.magnitude > moveSpeed)
        {
            Vector3 limitedvel = flatvel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        Debug.Log("jump");
    }
    private void ResetJump()
    {
        readyToJump = true;
    }
}
