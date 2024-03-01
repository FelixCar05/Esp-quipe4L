using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Entité : MonoBehaviour
{
    [SerializeField]
    GameObject Inventaire;

    [SerializeField]
    protected float PtDégât = 15;
    [SerializeField]
    protected float PtVie = 200;
    [SerializeField]
    protected int chanceDeDéposerinventaire = 75;
 
    public Entité (float ptVie,  float ptDégât)
    {
        PtDégât = ptDégât;
        PtVie = ptVie;
    }
   

    protected virtual void RecevoirDégât( float dégât)
    {
        PtVie -= dégât;
        if(PtVie <= 0)
        {
            Mourir();
        }
    }
    protected virtual void Mourir()
    {
        //déclancher une animation
        //if(Random.Range(1,100)<=chanceDeDéposerinventaire)
        DéposerInventaire();
    }
    protected virtual void DéposerInventaire()
    {
        if(Random.Range(0,100)<=chanceDeDéposerinventaire)
        {
            Instantiate(Inventaire,transform.position, Quaternion.Euler(0,0,0));
        }
        //GameObject balleTemp = ObjectPool.instance.GetPooledObject(Inventaire);
        //balleTemp.transform.position = transform.position;
    }
}
