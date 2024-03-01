using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineÉtatEnnemi
{
    public ÉtatEnnemi étatActuel { get; private set; }

    public void Initialiser(ÉtatEnnemi étatInitial)
    {
        étatActuel = étatInitial;
        étatActuel.EntrerÉtat();
    }

    public void ChangerÉtat(ÉtatEnnemi nouvelÉtat)
    {
        étatActuel.SortirÉtat();
        étatActuel = nouvelÉtat;
        étatActuel.EntrerÉtat();
    }

}
