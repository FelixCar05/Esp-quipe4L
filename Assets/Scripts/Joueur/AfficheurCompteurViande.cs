using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AfficheurCompteurViande : MonoBehaviour
{
    TextMeshProUGUI AffichageCompteurViande;
    [SerializeField]
    Joueur joueur;


    void Start()
    {

        AffichageCompteurViande = GetComponent<TextMeshProUGUI>();
        if(joueur!=null)
        joueur.ViandeRamassée += RamasserViande;
        AffichageCompteurViande.text = joueur.NbViandes.ToString("00") + "/" + joueur.NbViandesMax.ToString();

    }

    // Update is called once per frame

    void RamasserViande(object sender, EventArgs dataEvent)
    {
        AffichageCompteurViande.text = joueur.NbViandes.ToString("00") + "/" + joueur.NbViandesMax.ToString();
    }

}
