using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Joueur : Entité
{

    [SerializeField]
    Slider slider;
    [SerializeField]
    Gradient CouleurGraduel;
    [SerializeField]
    Image BarreDeVie;
    public event EventHandler ViandeRamassée;

    [SerializeField]
    int nbViandesMax = 20;
    public int NbViandesMax
    {
        get
        {
            return nbViandesMax;
        }
        private set
        {
            nbViandesMax = value;
        }
    }

    private int nbViandes = 0;

    public int NbViandes
    {
        get
        {
            return nbViandes;
        }
        private set
        {
            nbViandes = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(slider!= null)
        {
            slider.maxValue = PtVie;
            slider.value = PtVie;
            BarreDeVie.color = CouleurGraduel.Evaluate(1f);
        }
        
        nbViandes = 0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public Joueur(float ptVie,float ptDégât) :base( ptVie,  ptDégât)
    {
  
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Viande" && NbViandes < nbViandesMax)
        {
            //collision.gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            NbViandes++;
            OnViandeRamassée();
        }
    }

    private void OnViandeRamassée()
    {
        ViandeRamassée?.Invoke(this, null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Viande" && NbViandes <= nbViandesMax)
        //{
        //    //collision.gameObject.SetActive(false);
        //    Destroy(collision.gameObject);
        //    NbViandes++;
        //}
        if (collision.gameObject.tag == "Projectile")
        {
            RecevoirDégât(2f);
        }
        if (collision.gameObject.tag == "Zombie")
        {
            RecevoirDégât(2f);//commment avoir acces au pt degat de la classe
        }
    }
    protected override void Mourir()
    {
        //changer de scene
    }
    protected override void RecevoirDégât(float dégât)
    {
        base.RecevoirDégât(dégât);
        slider.value = PtVie;
        BarreDeVie.color = CouleurGraduel.Evaluate(slider.normalizedValue);
    }

    // pour la barre de vie je me suis grandement inspiré de cette vidéo https://www.youtube.com/watch?v=BLfNP4Sc_iA
}
