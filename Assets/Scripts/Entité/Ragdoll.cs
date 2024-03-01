using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{

    Collider[] ColliderRagdollÉteint;

    [SerializeField]
    Animator animationPrésente;
    Rigidbody[] RigidbodyRagdollÉteint;
    private Collider[] CollidersRagdoll;
    private Rigidbody[] RigidbodiesRagdoll;

    void Start()
    {
        ColliderRagdollÉteint = GetComponents<Collider>();
        RigidbodyRagdollÉteint = GetComponents<Rigidbody>();
        PrendreComposanteRagdoll();
        ÉteindreRagdoll();
    }

    void Update()
    {

    }
    public void ActiverRagdoll()
    {
        animationPrésente.enabled = false;


        foreach (Collider collider in CollidersRagdoll)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in RigidbodiesRagdoll)
        {
            rigidbody.isKinematic = false;
        }
        foreach (Collider collider in ColliderRagdollÉteint)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in RigidbodyRagdollÉteint)
        {
            rigidbody.isKinematic = true;
        }

    }
    private void ÉteindreRagdoll()
    {
        foreach (Collider collider in CollidersRagdoll)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in RigidbodiesRagdoll)
        {
            rigidbody.isKinematic = true;
        }

        animationPrésente.enabled = true;
        foreach (Collider collider in ColliderRagdollÉteint)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in RigidbodyRagdollÉteint)
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