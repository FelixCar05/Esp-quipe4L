using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Ennemi : Entit�
{
    // Start is called before the first frame update
    protected Vector3 DirectionBalleFinDeVie;
    protected Transform PositionBalleDeFinVie;
    protected Rigidbody[] rb;
    [SerializeField]
    GameObject ExplosionSang;
    [SerializeField]
    GameObject CouliSang;
    protected NavMeshAgent NavAgent;
    [SerializeField]
    protected GameObject joueur;

    private void Start()
    {
        rb = GetComponentsInChildren<Rigidbody>();
        NavAgent = GetComponent<NavMeshAgent>();

    }


    public Ennemi(float ptVie, float ptD�g�t) : base(ptVie, ptD�g�t)
    {
    }
   
    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
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
                    GameObject explosionSang = ObjectPool.instance.GetPooledObject(ExplosionSang);

                    explosionSang.transform.position = collision.transform.position;//issue is here
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
    // historique qui a avanc�
    protected override void Mourir()
    {
        //d�clancher une animation
        base.Mourir();
        Destroy(gameObject, 5f);
        GetComponent<Ragdoll>().ActiverRagdoll();
        TrouverRigidBodyPlusPr�s(PositionBalleDeFinVie).AddForce(DirectionBalleFinDeVie * 750f, ForceMode.Impulse);
    }
    protected virtual Rigidbody TrouverRigidBodyPlusPr�s(Transform PositionBalleDeFinVie)
    {
        Rigidbody RigidbodyPlusPr�s = rb[0];
        Vector3 positionCollision = PositionBalleDeFinVie != null ? PositionBalleDeFinVie.position : transform.position;

        float distancePlusPr�s = Vector3.Distance(positionCollision, rb[0].transform.position);//si la mort n'est pas caus�e par une balle


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
    public virtual void BougerNavMesh(Vector3 position)
    {
        NavAgent.destination = position;
    }
}
