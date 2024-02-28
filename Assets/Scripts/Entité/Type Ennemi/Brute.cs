using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brute : Ennemi
{
    [SerializeField] GameObject Joueur;
    enum …tat { Patrouille, DirigerVersCouverture, Combat}

    private …tatEnnemi[] ÈtatsEnnemi;

    private …tatEnnemi …tatPatrouille;
    private Machine…tatEnnemi machine…tatEnnemi;
    public Brute(float ptVie, float ptDÈg‚t) : base(ptVie, ptDÈg‚t)
    {
    }

    private void Awake()
    {
        machine…tatEnnemi = new Machine…tatEnnemi();
        …tatPatrouille = new Patrouille(machine…tatEnnemi, this);
        //ÈtatsEnnemi[(int)…tat.Patrouille] = new Patrouille(machine…tatEnnemi,this);
        //ÈtatsEnnemi[(int)…tat.DirigerVersCouverture] = new DirigerVerCouverture(machine…tatEnnemi, this);
        //ÈtatsEnnemi[(int)…tat.Combat] = new Combat(machine…tatEnnemi, this);
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
        machine…tatEnnemi.ÈtatActuel.FrameUpdate(gameObject);
    }

    private void FixedUpdate()
    {
        machine…tatEnnemi.ÈtatActuel.PhysicsUpdate();
    }
    public void Patrouiller(Vector3 position)
    {
       
    }
    public void Tirer()
    {

    }
    public void Recharger()
    {

    }
    public override void BougerNavMesh(Vector3 position)
    {
        base.BougerNavMesh(position);
    }
}
