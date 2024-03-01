using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : …tatEnnemi
{
    float distanceCouvertureMax;
    float distanceVisionCouvertureMax;
    public Combat(Machine…tatEnnemi ÈtatEnnemi, Brute ennemi, float distanceCouvertureMax, float distanceVisionCouvertureMax) : base(ÈtatEnnemi, ennemi)
    {
        this.distanceCouvertureMax = distanceCouvertureMax;
        this.distanceVisionCouvertureMax = distanceVisionCouvertureMax;
    }
    public override void AnimationTriggerEvent()
    {

    }
    public override void Entrer…tat()
    {

    }
    public override void FrameUpdate(GameObject ennemiGameObject, GameObject joueur)
    {
        if (Mathf.Abs(ennemiGameObject.transform.position.x - joueur.transform.position.x) <= distanceCouvertureMax&&
            Mathf.Abs(ennemiGameObject.transform.position.z - joueur.transform.position.z) <= distanceCouvertureMax)
        {
            ÈtatEnnemi.Changer…tat(ennemi.…tatDirigerVerCouverture);
        }
        ennemi.RegarderCible(joueur.transform.position);
        ennemi.Tirer();
        if(Mathf.Abs(ennemiGameObject.transform.position.x - joueur.transform.position.x) >= distanceVisionCouvertureMax &&
            Mathf.Abs(ennemiGameObject.transform.position.z - joueur.transform.position.z) >= distanceVisionCouvertureMax)
        {
            ÈtatEnnemi.Changer…tat(ennemi.…tatPatrouille);

        }
    }
    public override void Sortir…tat()
    {

    }
}
