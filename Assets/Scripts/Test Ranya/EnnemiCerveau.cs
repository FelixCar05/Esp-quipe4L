using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemiCerveau : Ennemi //beaucoup du code ici se retrouve dans ennemi, il faut modifier cette classe pour qu'elle herite d'ennemi.
{

    //Déclaration et méthodes de base-----------------------------------------------------------------------------------------
    private Animator animateur;
    private float ptVieMax;
    private bool mort;
    private bool idle;
    private float vitesseProjectile;
    private float distanceAuJoueur;
    private Salle ScriptSalleMère;


    [SerializeField]
    GameObject TeteCerveau;
    Vector3 origineSlime;
    [SerializeField]
    GameObject slimeBall;
    [SerializeField]
    GameObject testFuite;


    public EnnemiCerveau(float ptVie, float ptDégât) : base(ptVie, ptDégât)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        ScriptSalleMère = GetComponentInParent<Salle>();
        animateur = GetComponent<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();
        rb = GetComponentsInChildren<Rigidbody>();
        origineSlime = TeteCerveau.transform.position;
        vitesseProjectile = 5f;
        mort = false;
        ptVieMax = PtVie;
        idle = true;
    }

    void Update()
    {
        GérerDéplacement();
        if (idle && ScriptSalleMère != null && ScriptSalleMère.découverte == true)
        {
            idle = false; //comme cela on ne reset pas le trigger indéfiniment pour rien une fois que le crawler se réveille
            animateur.SetTrigger("EnnemiDétecté");
        }
    }



    //Méthodes qui sont propres au cerveau----------------------------------------------------------------------------------

    private void CracherProjectileSlime()//appelé par un event dans son animation HitAvant
    {
        GameObject projectileSlime = ObjectPool.instance.GetPooledObject(slimeBall);
        if (projectileSlime != null)
        {
            projectileSlime.transform.position = origineSlime;
            projectileSlime.SetActive(true);

            Vector3 direction = (joueur.transform.position - projectileSlime.transform.position).normalized;

            Rigidbody rbBalle = projectileSlime.GetComponent<Rigidbody>();
            if (rbBalle != null)
            {
                rbBalle.velocity = direction * vitesseProjectile; //vrm un code de base, trajectoire à améliorer prochainement (balistique)
            }
        }
    }

    private void GérerDéplacement()
    {
        distanceAuJoueur = (transform.position - joueur.transform.position).magnitude; //les animations du cerveau sont determines par la distance qu'il y a entre lui et le joueur
        Debug.Log(distanceAuJoueur);
        animateur.SetFloat("DistanceJoueur", distanceAuJoueur);

        if (animateur.GetCurrentAnimatorStateInfo(0).IsName("Marcher"))
        {
            NavAgent.isStopped = false;
            NavAgent.destination = joueur.transform.position;
        }

        if (animateur.GetCurrentAnimatorStateInfo(0).IsName("Cracher")|| animateur.GetCurrentAnimatorStateInfo(0).IsName("Kick")|| animateur.GetCurrentAnimatorStateInfo(0).IsName("Élan"))
        {
            NavAgent.isStopped = true;
            transform.LookAt(joueur.transform.position);
        }

        if (animateur.GetCurrentAnimatorStateInfo(0).IsName("Course"))
        {
            NavAgent.isStopped = false;
            NavAgent.destination = testFuite.transform.position;
            transform.LookAt(joueur.transform.position);
        }






    }




    //Méthodes héritées de la classe Ennemi---------------------------------------------------------------------------------

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }

    protected override void Mourir()
    {
        base.Mourir();
    }

    protected override Rigidbody TrouverRigidBodyPlusPrès(Transform PositionBalleDeFinVie)
    {
        return base.TrouverRigidBodyPlusPrès(PositionBalleDeFinVie);
    }


    //Things this dude's gotta do:
    //-Follow player but stay at a distance (raycast)
    //-reculer si le player est trop proche
    //-lancer du goo goo slime (object pool instantiate
    //-look silly !
}
