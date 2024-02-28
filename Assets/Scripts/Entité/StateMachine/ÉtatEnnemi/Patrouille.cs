using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Patrouille : …tatEnnemi
{
    private float RayonPatrouille = 50f;

    private bool EstDestinationTrouvÈ = false;

    private bool EstDestinationAtteinte = false;
    GameObject ennemiGameObject;

    Vector3 Destination;
    
    public Patrouille(Machine…tatEnnemi ÈtatEnnemi, Ennemi ennemi) : base(ÈtatEnnemi, ennemi)
    {
    }

    public override void AnimationTriggerEvent()
    {
        
    }

    public override void Entrer…tat()
    {
        TrouverDestination();
    }

    private void TrouverDestination()
    {
        float PositionX;
        float PositionZ;
        NavMeshHit hit;
        do
        {
            PositionX = UnityEngine.Random.Range(-RayonPatrouille, RayonPatrouille) + ennemi.transform.position.x;
            PositionZ = UnityEngine.Random.Range(-RayonPatrouille, RayonPatrouille) + ennemi.transform.position.z;
            Destination = new Vector3(PositionX, ennemi.transform.position.y, PositionZ);

        } while (!NavMesh.SamplePosition(Destination, out hit, 10f, NavMesh.AllAreas));

        Destination = hit.position;
        ennemi.BougerNavMesh(Destination);
    }

    public override void FrameUpdate(GameObject ennemi)
    {
        this.ennemiGameObject = ennemi;
        if(Mathf.Abs(ennemi.transform.position.x - Destination.x) <= 0.3f && Mathf.Abs(ennemi.transform.position.z - Destination.z) <= 0.3f)
        {
            TrouverDestination();
        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void Sortir…tat()
    {

    }
}
