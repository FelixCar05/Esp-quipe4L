using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemiCrawler : Entité //beaucoup du code ici se retrouve dans ennemi, il faut modifier cette classe pour qu'elle herite d'ennemi.
{
    // Start is called before the first frame update
    Salle ScriptSalleMère;
    GameObject Joueur;
    Animator animateur;
    NavMeshAgent NavAgent;
    bool mort;

    Vector3 DirectionBalleFinDeVie;
    Transform PositionBalleDeFinVie;
    Rigidbody[] rb;
    [SerializeField]
    ParticleSystem ExplosionSang;
    [SerializeField]
    ParticleSystem CouliSang;

    void Start()
    {
        ScriptSalleMère = GetComponentInParent<Salle>();
        Joueur = GameObject.FindGameObjectWithTag("Joueur");
        animateur = GetComponent<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();
        rb = GetComponentsInChildren<Rigidbody>();
        mort = false;
    }

    public EnnemiCrawler(float ptVie, float ptDégât) : base(ptVie, ptDégât)
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (ScriptSalleMère != null && ScriptSalleMère.découverte == true)
        {
            animateur.SetTrigger("EnnemiDétecté");
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleMère.listeEnnemisDansSalle.Count);
            Debug.Log(animateur.GetInteger("NombreEnnemisDansSalle"));

            if (!mort && (animateur.GetCurrentAnimatorStateInfo(0).IsName("Ramper")
                || animateur.GetCurrentAnimatorStateInfo(0).IsName("Ramper4pattes")))
            {
                NavAgent.destination = Joueur.transform.position;
            }

        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectile"))
        {
            //Destroy(collision.gameObject);
            collision.gameObject.SetActive(false);
            DirectionBalleFinDeVie = collision.gameObject.transform.forward.normalized;
            PositionBalleDeFinVie = collision.gameObject.transform;
            base.RecevoirDégât(2f); //commment avoir acces au pt degat de la classe
            if (PtVie > 0)
            {
                if (ExplosionSang != null)
                {
                    Instantiate(ExplosionSang, collision.transform.position, Quaternion.Euler
                    (collision.transform.rotation.x, -collision.transform.rotation.y, collision.transform.rotation.z));
                }
                if (CouliSang != null)
                {
                    Instantiate(CouliSang, collision.transform.position, Quaternion.Euler
                   (collision.transform.rotation.x, -collision.transform.rotation.y, collision.transform.rotation.z));
                }
            }

            animateur.SetTrigger("Attaqué");
        }

        if (collision.gameObject.CompareTag("Salle"))//RANYA MODIFY THIS
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleMère.listeEnnemisDansSalle.Count);

    }// historique qui a avancé

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Salle"))
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleMère.listeEnnemisDansSalle.Count);
    }


    protected override void Mourir()
    {
        //déclancher une animation
        mort = true;
        animateur.SetBool("Mort", true);
        NavAgent.enabled = false;
        base.Mourir();
        Destroy(gameObject, 5f);
        GetComponent<Ragdoll>().ActiverRagdoll();
        TrouverRigidBodyPlusPrès(PositionBalleDeFinVie).AddForce(DirectionBalleFinDeVie * 750f, ForceMode.Impulse);

    }
    private Rigidbody TrouverRigidBodyPlusPrès(Transform PositionBalleDeFinVie)
    {
        Rigidbody RigidbodyPlusPrès = rb[0];
        float distancePlusPrès = Vector3.Distance(PositionBalleDeFinVie.position, rb[0].transform.position);


        for (int i = 1; i < rb.Length; ++i)
        {
            float distance = Vector3.Distance(PositionBalleDeFinVie.position, rb[i].transform.position);

            if (distance < distancePlusPrès)
            {
                distancePlusPrès = distance;
                RigidbodyPlusPrès = rb[i];
            }
            if (distancePlusPrès <= .5f)
            {
                return RigidbodyPlusPrès;
            }
        }

        return RigidbodyPlusPrès;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Joueur"))
        {
            QTEManager.Instance.StartQTE(gameObject);
            animateur.SetBool("EnnemiZoneDattaque", true);
            NavAgent.isStopped = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Joueur"))
        {
            NavAgent.isStopped = false;
            animateur.SetBool("EnnemiZoneDattaque", false);
        }
    }

    private void Attaquer()
    {
        //Joueur.GetComponent<Joueur>().RecevoirDégât(); Marche pas pcq cest une methode protected
    }
}
