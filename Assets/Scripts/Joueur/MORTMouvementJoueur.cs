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

















    

    //les touches de d�placement de la cam�ra
    static KeyCode[] TouchesTranslation = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };

    //les vecteurs unitaires associ�s aux diff�rents d�placements de la cam�ra
    static Vector3[] VecteursTranslation = { Vector3.forward, Vector3.left, Vector3.back, Vector3.right };



    Action[] ActionsCam�ra;



    private void Awake()
    {
        PositionInitiale = transform.localPosition;
        AssocierActionsCam�ra();
    }

    void Update()
    {
        G�rerTouchesCam�ra();

    }

    private void G�rerTouchesCam�ra()
    {

        
        // On calcule le d�placement total du frame
        Vector3 d�placement = Interpr�terTouches(TouchesTranslation, VecteursTranslation, moveSpeed);
        // On applique le d�placement total calcul�e dans le Frame � la cam�ra
        transform.Translate(d�placement);
        
    }

    private Vector3 Interpr�terTouches(KeyCode[] touches, Vector3[] vecteurs, float vitesse)
    {
        Vector3 vecteurTransformationFrame = Vector3.zero;
        int nbTouches = touches.Length;
        // Pour chacune des touches
        for (int i = 0; i < nbTouches; ++i)
        {
            // Si la touche est enfonc�e
            if (Input.GetKey(touches[i]))
            {
                vecteurTransformationFrame += vecteurs[i] * vitesse * Time.deltaTime;
            }
        }
        return vecteurTransformationFrame;
    }


    private void AssocierActionsCam�ra()
    {
        ActionsCam�ra = new Action[]
        {
         () => transform.localPosition = PositionInitiale,
        };
    }


}
