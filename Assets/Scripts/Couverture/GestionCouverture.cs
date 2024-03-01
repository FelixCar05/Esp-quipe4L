using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestionCouverture : MonoBehaviour
{
    private CoverValide[] Couvertures;
    protected bool EstPositionPrise = false;

    public Vector3 TrouverCouverture(Brute joueur)
    {
        List < CoverValide > coverValide = new List<CoverValide>();
        Couvertures = gameObject.GetComponentsInChildren<CoverValide>();
        for(int i = 0; i < Couvertures.Length;++i)
        {
            if (Couvertures[i].EstValide(joueur))
            {
                coverValide.Add(Couvertures[i]);
            }
        }
        return coverValide[Random.Range(1, coverValide.Count - 1)].transform.position;
    }
}
