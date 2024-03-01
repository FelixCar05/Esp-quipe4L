using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoverValide : GestionCouverture
{
    [SerializeField]
    float distanceMax = 25;
    [SerializeField]
    float distanceMin = 5;

    public bool EstValide(Brute joueur)
    {
       bool EstValide = false;
       float distance = Vector3.Distance(joueur.transform.position, transform.position);
       if (distance <= distanceMax && distance >= distanceMin)
       {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, (joueur.transform.position - transform.position).normalized,out hit))
            {
                if(hit.collider.tag != "Joueur")
                {
                    EstValide = true;
                }
            }
       }
        return EstValide;
    }
  
}
