using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{

    Collider[] ColliderRagdoll…teint;

    [SerializeField]
    Animator animationPrÈsente;
    Rigidbody[] RigidbodyRagdoll…teint;
    private Collider[] CollidersRagdoll;
    private Rigidbody[] RigidbodiesRagdoll;

    void Start()
    {
        ColliderRagdoll…teint = GetComponents<Collider>();
        RigidbodyRagdoll…teint = GetComponents<Rigidbody>();
        PrendreComposanteRagdoll();
        …teindreRagdoll();
    }

    void Update()
    {

    }
    public void ActiverRagdoll()
    {
        animationPrÈsente.enabled = false;


        foreach (Collider collider in CollidersRagdoll)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in RigidbodiesRagdoll)
        {
            rigidbody.isKinematic = false;
        }
        foreach (Collider collider in ColliderRagdoll…teint)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in RigidbodyRagdoll…teint)
        {
            rigidbody.isKinematic = true;
        }

    }
    private void …teindreRagdoll()
    {
        foreach (Collider collider in CollidersRagdoll)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in RigidbodiesRagdoll)
        {
            rigidbody.isKinematic = true;
        }

        animationPrÈsente.enabled = true;
        foreach (Collider collider in ColliderRagdoll…teint)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in RigidbodyRagdoll…teint)
        {
            rigidbody.isKinematic = false;
        }
    }
    private void PrendreComposanteRagdoll()
    {
        CollidersRagdoll = GetComponentsInChildren<Collider>();
        RigidbodiesRagdoll = GetComponentsInChildren<Rigidbody>();
    }
}