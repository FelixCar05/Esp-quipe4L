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



    Vector3 Destination;
    private float distanceVisionMax;
    private float angleDeVision;


    public Patrouille(Machine…tatEnnemi ÈtatEnnemi, Brute ennemi, float distanceVisionMax ,  float angleDeVision) : base(ÈtatEnnemi, ennemi)
    {
        this.distanceVisionMax = distanceVisionMax;
        this.angleDeVision = angleDeVision;
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

    public override void FrameUpdate(GameObject ennemiGameObject, GameObject joueur) 
    {

        if(Mathf.Abs(ennemiGameObject.transform.position.x - Destination.x) <= 0.3f && Mathf.Abs(ennemiGameObject.transform.position.z - Destination.z) <= 0.3f)
        {
            TrouverDestination();
        }
        if(Vector3.Distance(ennemiGameObject.transform.position,joueur.transform.position) <= distanceVisionMax)
        {
            if (Vector3.Angle(ennemiGameObject.transform.forward, joueur.transform.position) <= angleDeVision)
            {
                RaycastHit hit;
                if (Physics.Raycast(ennemiGameObject.transform.position, (joueur.transform.position - ennemiGameObject.transform.position).normalized, out hit))
                {
                    if (hit.collider.tag == "Joueur")
                    {
                        ÈtatEnnemi.Changer…tat(ennemi.…tatDirigerVerCouverture);
                    }
                }
                
            }
        }
        
    }

    public override void PhysicsUpdate()
    {

    }

    public override void Sortir…tat()
    {
        ennemi.BougerNavMesh(ennemi.transform.position);
    }
}
