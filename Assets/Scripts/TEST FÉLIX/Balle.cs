using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balle : MonoBehaviour
{
	[SerializeField] float vitesse = 50;
	Rigidbody body;
	//TEMPS DE VIE
	[SerializeField] float tempsDeVie = 3;
	float tempsInitial;
	void Start()
	{
		body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
	{
		tempsInitial = Time.time;
	}

	// Update is called once per frame
	void Update()
	{
		if(Time.time - tempsInitial > tempsDeVie)
		{
			gameObject.SetActive(false);
		}
	}

	private void FixedUpdate()
	{
		body.velocity = transform.forward * vitesse;
	}

	private void OnCollisionEnter(Collision collision)
	{

		//if (collision.collider.gameObject.tag == "Ennemis")
		//{
		//	// A VOIR POUR FAIURE DU DÉGATS !!!!!!!!!!!!
		//	//ennemy ennemie = collision.collider.gameObject.GetComponent<ennemy>();
		//	//ennemie.PrendreDegat(3);
		////}
		//if (collision.gameObject.tag == "Entité")
		//{
			
		//	//collision.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 1000f, ForceMode.Impulse);

		////}
		//else
		//{
			gameObject.SetActive(false);

        //}
      
    }
}
