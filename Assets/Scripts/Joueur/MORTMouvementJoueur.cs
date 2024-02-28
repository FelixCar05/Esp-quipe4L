using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MouvementJoeur : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float dashSpeed;

    public Transform orientation;

    //air stuff
    public float jumpForce;
    public float jumpCoolDown;//adjust cooldown for float bug
    public float airMultiplier;
    bool readyToJump = true;



    //controls

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

    Vector3 PositionInitiale { get; set; }

















    

    //les touches de déplacement de la caméra
    static KeyCode[] TouchesTranslation = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    //les vecteurs unitaires associés aux différents déplacements de la caméra
    static Vector3[] VecteursTranslation = { Vector3.forward, Vector3.left, Vector3.back, Vector3.right };



    Action[] ActionsCaméra;



    private void Awake()
    {
        PositionInitiale = transform.localPosition;
        AssocierActionsCaméra();
    }

    void Update()
    {
        GérerTouchesCaméra();

    }

    private void GérerTouchesCaméra()
    {

        
        // On calcule le déplacement total du frame
        Vector3 déplacement = InterpréterTouches(TouchesTranslation, VecteursTranslation, moveSpeed);
        // On applique le déplacement total calculée dans le Frame à la caméra
        transform.Translate(déplacement);
        
    }

    private Vector3 InterpréterTouches(KeyCode[] touches, Vector3[] vecteurs, float vitesse)
    {
        Vector3 vecteurTransformationFrame = Vector3.zero;
        int nbTouches = touches.Length;
        // Pour chacune des touches
        for (int i = 0; i < nbTouches; ++i)
        {
            // Si la touche est enfoncée
            if (Input.GetKey(touches[i]))
            {
                vecteurTransformationFrame += vecteurs[i] * vitesse * Time.deltaTime;
            }
        }
        return vecteurTransformationFrame;
    }


    private void AssocierActionsCaméra()
    {
        ActionsCaméra = new Action[]
        {
         () => transform.localPosition = PositionInitiale,
        };
    }


}
