using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirigerVerCouverture : …tatEnnemi
{
    Vector3 Destination;
    GestionCouverture couvertures;
    public DirigerVerCouverture(Machine…tatEnnemi ÈtatEnnemi, Brute ennemi, GestionCouverture couvertures) : base(ÈtatEnnemi, ennemi)
    {
        this.couvertures = couvertures;
    }

    public override void AnimationTriggerEvent()
    {
        
    }

    public override void Entrer…tat()
    {
        Destination = couvertures.TrouverCouverture(ennemi);
       
        ennemi.BougerNavMesh(Destination);
    }

    public override void FrameUpdate(GameObject ennemiGameObject, GameObject joueur)
    {
        if (Mathf.Abs(ennemiGameObject.transform.position.x -Destination.x) <= 4f && Mathf.Abs(ennemiGameObject.transform.position.z - Destination.z) <= 4f)
        {
            ÈtatEnnemi.Changer…tat(ennemi.…tatCombat);
        }
        ennemi.RegarderCible(joueur.transform.position);
        ennemi.Tirer();
    }



    public override void Sortir…tat()
    {

    }
}
