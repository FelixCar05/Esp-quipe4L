using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brute : Ennemi
{

    enum …tat { Patrouille, DirigerVersCouverture, Combat}

    [SerializeField] float distanceVisionMax = 10;
    [SerializeField] float angleDeVision = 60;
    [SerializeField] GestionCouverture Couvertures;
    [SerializeField] float distanceCouvertureMax = 5;
    [SerializeField] int capacitÈChargeur = 35;
    [SerializeField]
    GameObject balleEnnemi;
    [SerializeField] int vitesseProjectile = 17;
    [SerializeField] float tempsEntreTir = 0.5f;
    [SerializeField] float tempsDeRecharge = 3;
    [SerializeField] float distanceVisionCouvertureMax = 15;
    private float TimerEntreTir;
    private float TimerEntreRecharge;

    int nbBalleDansChargeur;
    public …tatEnnemi …tatPatrouille { get; private set; }
    public …tatEnnemi …tatDirigerVerCouverture { get; private set; }
    public  …tatEnnemi …tatCombat { get; private set; }
    private Machine…tatEnnemi machine…tatEnnemi;
    private bool estEnRecharge = false;
    public Brute(float ptVie, float ptDÈg‚t) : base(ptVie, ptDÈg‚t)
    {
    }

    private void Awake()
    {
        //Couvertures = GetComponent<GestionCouverture>();
        machine…tatEnnemi = new Machine…tatEnnemi();
        …tatPatrouille = new Patrouille(machine…tatEnnemi, this,distanceVisionMax, angleDeVision);
        …tatDirigerVerCouverture = new DirigerVerCouverture(machine…tatEnnemi, this, Couvertures);
        …tatCombat = new Combat(machine…tatEnnemi, this, distanceCouvertureMax, distanceVisionCouvertureMax);
        nbBalleDansChargeur = capacitÈChargeur;
    }

    private void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
        NavAgent = GetComponent<NavMeshAgent>();

        if(NavAgent!= null)
        {
            machine…tatEnnemi.Initialiser(…tatPatrouille);

        }
    }

    private void Update()
    {
        machine…tatEnnemi.ÈtatActuel.FrameUpdate(gameObject,joueur);
    }

    private void FixedUpdate()
    {
        machine…tatEnnemi.ÈtatActuel.PhysicsUpdate();
    }

    public void Tirer()
    {
        if(TimerEntreRecharge <= 0)
        {
            if (nbBalleDansChargeur >= capacitÈChargeur && !estEnRecharge)
            {
                if (TimerEntreTir <= 0)
                {
                    GameObject balle = ObjectPool.instance.GetPooledObject(balleEnnemi);

                    balle.transform.position = transform.position;
                    balle.SetActive(true);

                    Vector3 direction = (joueur.transform.position - balle.transform.position).normalized;

                    Rigidbody rbBalle = balle.GetComponent<Rigidbody>();
                    if (rbBalle != null)
                    {
                        rbBalle.velocity = direction * vitesseProjectile; //vrm un code de base, trajectoire ‡ amÈliorer prochainement (balistique)
                    }
                    TimerEntreTir = tempsEntreTir; // rÈnitialiser le timer 

                }
                else
                {
                    TimerEntreTir -= Time.deltaTime;
                }
            }

            else
            {
                TimerEntreRecharge = tempsDeRecharge;
            }
        }
        else
        {
            TimerEntreRecharge -= Time.deltaTime;
        }
    }
    
    public override void BougerNavMesh(Vector3 position)
    {
        base.BougerNavMesh(position);
    }
    public void RegarderCible(Vector3 position)
    {
        transform.LookAt(position);
    }
}
