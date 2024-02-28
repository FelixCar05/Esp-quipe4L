using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ÉtatEnnemi 
{
    protected MachineÉtatEnnemi étatEnnemi;
    protected Ennemi ennemi;

    public ÉtatEnnemi(MachineÉtatEnnemi étatEnnemi, Ennemi ennemi)
    {
        this.étatEnnemi = étatEnnemi;
        this.ennemi = ennemi;
    }

    public virtual void EntrerÉtat() { }
    public virtual void SortirÉtat() { }
    public virtual void FrameUpdate(GameObject ennemi) { }
    public virtual void PhysicsUpdate() { }
    public virtual void AnimationTriggerEvent() { }


}
