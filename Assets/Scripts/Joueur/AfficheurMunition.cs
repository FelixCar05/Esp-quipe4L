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
          //  joueur.Tir� += Tirer;
           // joueur.Recharg� += Recharger;
        }
        AffichageMunition.text = (joueur.Capacit�Chargeur - joueur.NbBallesTir�es).ToString("00/" + joueur.Capacit�Chargeur);
        AffichageMunition.text = (joueur.Capacit�Chargeur - joueur.NbBallesTir�es).ToString("00/" + joueur.Capacit�Chargeur);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void Tirer(object sender, EventArgs dataEvent)
    {
        //AffichageMunition.text = (joueur.Capacit�Chargeur-joueur.NbBallesTir�es).ToString("00/" + joueur.Capacit�Chargeur);
    }

    void Recharger(object sender, EventArgs dataEvent)
    {
        //AffichageMunition.text = joueur.Capacit�Chargeur.ToString("00/" + joueur.Capacit�Chargeur);
    }

}
