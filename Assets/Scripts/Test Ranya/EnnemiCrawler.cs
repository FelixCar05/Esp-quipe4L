using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemiCrawler : Ennemi //beaucoup du code ici se retrouve dans ennemi, il faut modifier cette classe pour qu'elle herite d'ennemi.
{
    private Salle ScriptSalleM�re;
    private Animator animateur;
    private float ptVieMax;
    private bool mort;
    private bool idle;


    [SerializeField]
    GameObject TestFuiteEnnemi;
    [SerializeField]
    float vitesse4pattes;
    [SerializeField]
    float vitesseRamper;

    public EnnemiCrawler(float ptVie, float ptD�g�t) : base(ptVie, ptD�g�t)
    {
    }
    void Start()
    {
        ScriptSalleM�re = GetComponentInParent<Salle>();
        animateur = GetComponent<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();
        rb = GetComponentsInChildren<Rigidbody>();
        mort = false;
        ptVieMax = PtVie;
        vitesseRamper = 3f;
        vitesse4pattes = 6f;
        animateur.SetBool("Idle", true);

    }

    // Update is called once per frame
    void Update()
    {
        if (animateur.GetBool("Idle") && ScriptSalleM�re != null && ScriptSalleM�re.d�couverte == true)
        {
            animateur.SetBool("Idle", false); //comme cela on ne reset pas le trigger ind�finiment pour rien une fois que le crawler se r�veille
            animateur.SetTrigger("EnnemiD�tect�");
        }
        animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleM�re.listeEnnemisDansSalle.Count);

        if (!mort)
        {
            if (animateur.GetCurrentAnimatorStateInfo(0).IsName("Ramper"))//pour qu'il ne commence � ramper que lordqu'il est en animation de d�placement
            {
                NavAgent.speed = vitesseRamper;
                base.BougerNavMesh(joueur.transform.position);
            }
            else if (animateur.GetCurrentAnimatorStateInfo(0).IsName("Ramper4pattes"))
            {
                NavAgent.speed = vitesse4pattes;
                base.BougerNavMesh(TestFuiteEnnemi.transform.position);
            }
        }
        

    }
    protected override void OnCollisionEnter(Collision collision)
    {

        Debug.Log(collision.gameObject.name);   

        if (collision.gameObject.CompareTag("Projectile"))
        {
            base.OnCollisionEnter(collision);
            animateur.SetTrigger("Attaqu�");
        }

        if (collision.gameObject.CompareTag("Salle"))
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleM�re.listeEnnemisDansSalle.Count);

        
            

    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Salle"))
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleM�re.listeEnnemisDansSalle.Count);
    }


    protected override void Mourir()
    {
        mort = true;
        ScriptSalleM�re.listeEnnemisDansSalle.Remove(gameObject);
        animateur.SetBool("Mort", true);
        NavAgent.enabled = false;
        base.Mourir();
        //Destroy(gameObject, 5f);
        //GetComponent<Ragdoll>().ActiverRagdoll();
        //TrouverRigidBodyPlusPr�s(PositionBalleDeFinVie).AddForce(DirectionBalleFinDeVie * 750f, ForceMode.Impulse);

    }
    protected override Rigidbody TrouverRigidBodyPlusPr�s(Transform PositionBalleDeFinVie)
    {
        return base.TrouverRigidBodyPlusPr�s(PositionBalleDeFinVie);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Joueur"))
        {
            animateur.SetBool("EnnemiZoneDattaque", true);
            NavAgent.isStopped = true;

            if (PtVie < (ptVieMax * 0.2) && Random.Range(1, 11) == 5)
            {
                QTEManager.Instance.StartQTE(gameObject);
                animateur.SetBool("QTE", true);
            }
                
        }
        if (other.CompareTag("TestFuite")&& animateur.GetCurrentAnimatorStateInfo(0).IsName("Ramper4pattes"))
        {
            animateur.SetBool("Idle", true);//on ne va pas l'apercevoir tout de suite, puisque le crawler ne change pas reellement de salle (hypothethique)
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Joueur"))
        {
            NavAgent.isStopped = false;
            animateur.SetBool("EnnemiZoneDattaque", false);
            if(animateur.GetBool("QTE"))
                Mourir();

        }
    }

    private void Attaquer()
    {
        //Joueur.GetComponent<Joueur>().RecevoirD�g�t(); Marche pas pcq cest une methode protected
    }
}
