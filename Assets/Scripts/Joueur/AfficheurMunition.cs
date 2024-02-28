using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AfficheurMunition : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI AffichageMunition;
    [SerializeField]
    CharacterController1 joueur;


    void Start()
    {
        
        AffichageMunition = GetComponent<TextMeshProUGUI>();
        if (joueur != null)
        {
          //  joueur.Tiré += Tirer;
           // joueur.Rechargé += Recharger;
        }
        AffichageMunition.text = (joueur.CapacitéChargeur - joueur.NbBallesTirées).ToString("00/" + joueur.CapacitéChargeur);
        AffichageMunition.text = (joueur.CapacitéChargeur - joueur.NbBallesTirées).ToString("00/" + joueur.CapacitéChargeur);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Tirer(object sender, EventArgs dataEvent)
    {
        //AffichageMunition.text = (joueur.CapacitéChargeur-joueur.NbBallesTirées).ToString("00/" + joueur.CapacitéChargeur);
    }

    void Recharger(object sender, EventArgs dataEvent)
    {
        //AffichageMunition.text = joueur.CapacitéChargeur.ToString("00/" + joueur.CapacitéChargeur);
    }

}
