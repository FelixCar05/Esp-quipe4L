using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Brute : Ennemi
{
    [SerializeField] GameObject Joueur;
    enum �tat { Patrouille, DirigerVersCouverture, Combat}

    private �tatEnnemi[] �tatsEnnemi;

    private �tatEnnemi �tatPatrouille;
    private Machine�tatEnnemi machine�tatEnnemi;
    public Brute(float ptVie, float ptD�g�t) : base(ptVie, ptD�g�t)
    {
    }

    private void Awake()
    {
        machine�tatEnnemi = new Machine�tatEnnemi();
        �tatPatrouille = new Patrouille(machine�tatEnnemi, this);
        //�tatsEnnemi[(int)�tat.Patrouille] = new Patrouille(machine�tatEnnemi,this);
        //�tatsEnnemi[(int)�tat.DirigerVersCouverture] = new DirigerVerCouverture(machine�tatEnnemi, this);
        //�tatsEnnemi[(int)�tat.Combat] = new Combat(machine�tatEnnemi, this);
    }

    private void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
        NavAgent = GetComponent<NavMeshAgent>();

        if(NavAgent!= null)
        {
            machine�tatEnnemi.Initialiser(�tatPatrouille);

        }
    }

    private void Update()
    {
        machine�tatEnnemi.�tatActuel.FrameUpdate(gameObject);
    }

    private void FixedUpdate()
    {
        machine�tatEnnemi.�tatActuel.PhysicsUpdate();
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
