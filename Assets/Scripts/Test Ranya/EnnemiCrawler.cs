using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnnemiCrawler : Entit� //beaucoup du code ici se retrouve dans ennemi, il faut modifier cette classe pour qu'elle herite d'ennemi.
{
    // Start is called before the first frame update
    Salle ScriptSalleM�re;
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
        ScriptSalleM�re = GetComponentInParent<Salle>();
        Joueur = GameObject.FindGameObjectWithTag("Joueur");
        animateur = GetComponent<Animator>();
        NavAgent = GetComponent<NavMeshAgent>();
        rb = GetComponentsInChildren<Rigidbody>();
        mort = false;
    }

    public EnnemiCrawler(float ptVie, float ptD�g�t) : base(ptVie, ptD�g�t)
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (ScriptSalleM�re != null && ScriptSalleM�re.d�couverte == true)
        {
            animateur.SetTrigger("EnnemiD�tect�");
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleM�re.listeEnnemisDansSalle.Count);
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
            base.RecevoirD�g�t(2f); //commment avoir acces au pt degat de la classe
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

            animateur.SetTrigger("Attaqu�");
        }

        if (collision.gameObject.CompareTag("Salle"))//RANYA MODIFY THIS
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleM�re.listeEnnemisDansSalle.Count);

    }// historique qui a avanc�

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Salle"))
            animateur.SetInteger("NombreEnnemisDansSalle", ScriptSalleM�re.listeEnnemisDansSalle.Count);
    }


    protected override void Mourir()
    {
        //d�clancher une animation
        mort = true;
        animateur.SetBool("Mort", true);
        NavAgent.enabled = false;
        base.Mourir();
        Destroy(gameObject, 5f);
        GetComponent<Ragdoll>().ActiverRagdoll();
        TrouverRigidBodyPlusPr�s(PositionBalleDeFinVie).AddForce(DirectionBalleFinDeVie * 750f, ForceMode.Impulse);

    }
    private Rigidbody TrouverRigidBodyPlusPr�s(Transform PositionBalleDeFinVie)
    {
        Rigidbody RigidbodyPlusPr�s = rb[0];
        float distancePlusPr�s = Vector3.Distance(PositionBalleDeFinVie.position, rb[0].transform.position);


        for (int i = 1; i < rb.Length; ++i)
        {
            float distance = Vector3.Distance(PositionBalleDeFinVie.position, rb[i].transform.position);

            if (distance < distancePlusPr�s)
            {
                distancePlusPr�s = distance;
                RigidbodyPlusPr�s = rb[i];
            }
            if (distancePlusPr�s <= .5f)
            {
                return RigidbodyPlusPr�s;
            }
        }

        return RigidbodyPlusPr�s;
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
        //Joueur.GetComponent<Joueur>().RecevoirD�g�t(); Marche pas pcq cest une methode protected
    }
}
