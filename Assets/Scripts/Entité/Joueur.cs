using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Joueur : Entit�
{

    [SerializeField]
    Slider slider;
    [SerializeField]
    Gradient CouleurGraduel;
    [SerializeField]
    Image BarreDeVie;
    public event EventHandler ViandeRamass�e;

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
    public Joueur(float ptVie,float ptD�g�t) :base( ptVie,  ptD�g�t)
    {
  
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Viande" && NbViandes < nbViandesMax)
        {
            //collision.gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            NbViandes++;
            OnViandeRamass�e();
        }
    }

    private void OnViandeRamass�e()
    {
        ViandeRamass�e?.Invoke(this, null);
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
            RecevoirD�g�t(2f);
        }
        if (collision.gameObject.tag == "Zombie")
        {
            RecevoirD�g�t(2f);//commment avoir acces au pt degat de la classe
        }
    }
    protected override void Mourir()
    {
        //changer de scene
    }
    protected override void RecevoirD�g�t(float d�g�t)
    {
        base.RecevoirD�g�t(d�g�t);
        slider.value = PtVie;
        BarreDeVie.color = CouleurGraduel.Evaluate(slider.normalizedValue);
    }

    // pour la barre de vie je me suis grandement inspir� de cette vid�o https://www.youtube.com/watch?v=BLfNP4Sc_iA
}
