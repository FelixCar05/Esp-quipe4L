using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Ennemi : Entité
{
    // Start is called before the first frame update
    Vector3 DirectionBalleFinDeVie;
    Transform PositionBalleDeFinVie;
    protected Rigidbody[] rb;
    [SerializeField]
    GameObject ExplosionSang;
    [SerializeField]
    GameObject CouliSang;
    protected NavMeshAgent NavAgent;

    private void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
        NavAgent = GetComponent<NavMeshAgent>();

    }


    public Ennemi(float ptVie, float ptDégât) : base(ptVie, ptDégât)
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
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
                    GameObject explosionSang = ObjectPool.instance.GetPooledObject(ExplosionSang);

                    explosionSang.transform.position = collision.transform.position;
                    explosionSang.transform.rotation = new Quaternion(collision.transform.rotation.x, -collision.transform.rotation.y, collision.transform.rotation.z,collision.transform.rotation.w);
                    explosionSang.SetActive(true);
                    StartCoroutine(Attendre(explosionSang));

                }
                if (CouliSang != null)
                {
                    GameObject couliSang = ObjectPool.instance.GetPooledObject(CouliSang);

                    couliSang.transform.position = collision.transform.position;
                    couliSang.transform.rotation = new Quaternion(collision.transform.rotation.x, -collision.transform.rotation.y, collision.transform.rotation.z, collision.transform.rotation.w);
                    couliSang.SetActive(true);
                    StartCoroutine(Attendre(couliSang));

                }
            }
           
            
        }

    }
   
    IEnumerator Attendre(GameObject particulesang)
    {
        yield return new
        WaitForSeconds(1);
        particulesang.SetActive(false);
    }
    // historique qui a avancé
    protected override void Mourir()
    {
        //déclancher une animation
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
    public virtual void BougerNavMesh(Vector3 position)
    {
        NavAgent.destination = position;
    }
}
