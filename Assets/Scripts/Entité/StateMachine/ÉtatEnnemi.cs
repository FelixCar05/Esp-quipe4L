using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÉtatEnnemi 
{
    protected MachineÉtatEnnemi étatEnnemi;
    protected Brute ennemi;

    public ÉtatEnnemi(MachineÉtatEnnemi étatEnnemi, Brute ennemi)
    {
        this.étatEnnemi = étatEnnemi;
        this.ennemi = ennemi;
    }

    public virtual void EntrerÉtat() { }
    public virtual void SortirÉtat() { }
    public virtual void FrameUpdate(GameObject ennemi, GameObject joueur) { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }


}
