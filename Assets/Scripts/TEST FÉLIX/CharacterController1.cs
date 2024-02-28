using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class CharacterController1 : MonoBehaviour
{
	// Start is called before the first frame update

    public event EventHandler Tiré;
    public event EventHandler Rechargé;

    //Vitesse
    [SerializeField] float vitesseMarche = 100;
	[SerializeField] float vitesseCourse = 150;
	[SerializeField] float forceJump = 7;
	[SerializeField] float mouseSensitivity = 300;
	[SerializeField] int capacitéChargeur = 5;
	private bool peutSeMouvoir = true;

    public int CapacitéChargeur
    {
        get { return capacitéChargeur; }

        private set { capacitéChargeur = value; }
    }

    //[SerializeField] int balles = 50;
    //Character controller
    [SerializeField] float gravity = 9.8f;

	Vector3 jump = Vector3.zero;
	CharacterController controller;
    Camera camerafps;
    //test
   

    int nbBallesTirées = 0;
	public int NbBallesTirées
	{
		get { return nbBallesTirées; }

		private set { nbBallesTirées = value; }
	}
	// Armes
	[SerializeField] float vitesseTir = 0.1f;
	float TimerEntreTir;
	[SerializeField] GameObject balle;
	GameObject barrel;

	Vector3 rotationCamera = Vector3.zero;

	void Start()
	{

		controller = GetComponent<CharacterController>();
		camerafps = gameObject.GetComponentInChildren<Camera>();
		Cursor.lockState = CursorLockMode.Locked;
		barrel = GameObject.FindGameObjectWithTag("Barrel");
    }

    // Update is called once per frame
    void Update()
	{
		if (peutSeMouvoir)
		{
			RotationCamera();
			Deplacement();
			GererTir();
			//GestionPause();
			GestionRecharge();
		}
		


    }

     void GestionRecharge()
    {
        if (Input.GetButtonDown("Recharger"))
        {
            Recharger();
        }
    }

    void GererTir() 
	{
		// Venir regarder si la personne clique sur la souris en respectant le timer
		if(nbBallesTirées < CapacitéChargeur) 
		{
			if (Input.GetAxis("Fire") > 0 && TimerEntreTir <= 0 )
			{
				GameObject balleTemp = ObjectPool.instance.GetPooledObject(balle);
				balleTemp.transform.position = barrel.transform.position; // PLacer la balle à la position du barrel donc si ça tire tout croche c'est srm ici ou dans le script de la balle 
				nbBallesTirées++;
				OnTiré();
				RaycastHit hit;

				if (Physics.Raycast(camerafps.transform.position, camerafps.transform.forward, out hit, 500, 3))
				{
					balleTemp.transform.rotation = Quaternion.LookRotation(hit.point - barrel.transform.position); // Si on touche quelque chose envoyer la balle vers cette direction
				}
				else
				{
					balleTemp.transform.rotation = Quaternion.LookRotation((camerafps.transform.position + camerafps.transform.forward * 500) - barrel.transform.position); // Sinon s'occuper qu'elle va bien ou le personnage regarde et non ou le gun pointe
				}
				balleTemp.SetActive(true);
				TimerEntreTir = vitesseTir; // rénitialiser le timer 
			}
			else
			{
				TimerEntreTir -= Time.deltaTime;
			}
		}
		else
		{
			Recharger();
		}

	}
	void Deplacement()
	{
		Vector3 direction = camerafps.transform.forward * Input.GetAxis("Vertical1") + camerafps.transform.right * Input.GetAxis("Horizontal1"); // Gérer selon le input des touches le vecteur qui déplacera le personnage

		if (direction.magnitude > 0)
		{
			direction.y = 0; //meme chose
			direction = direction.normalized;
			direction = direction * (Input.GetButton("Sprint") ? vitesseCourse : vitesseMarche); //  Si il marche ou si il cours 
		}

		if (!controller.isGrounded) // Si le personnage ne touche pas au sol appliqué de la gravité
		{
			jump -= Vector3.up * gravity * Time.deltaTime;
		}
		else
		{
			if (Input.GetButtonDown("Jump")) // Sauter
			{
				jump = Vector3.up * forceJump;
			}
			jump.y = Mathf.Max(-1, jump.y);
		}

		controller.Move((direction + jump) * Time.deltaTime);
	}
	void Recharger()
	{
		
		//mettre animation realoder
		nbBallesTirées = 0;
        OnRechargé();
    }
	protected void OnRechargé()
	{
		Rechargé?.Invoke(this, null);
	}
	protected void OnTiré()
	{
		Tiré?.Invoke(nbBallesTirées, null);
	}
	void RotationCamera()
	{
		rotationCamera += new Vector3(-Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity, Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity, 0);

		rotationCamera.x = Mathf.Clamp(rotationCamera.x, -70, 70); // Limiter langle de la camera en y

		camerafps.transform.rotation = Quaternion.Euler(rotationCamera);
	}

    internal void ChangerPeutMoveEtRotate(bool peutBouger)
    {
        peutSeMouvoir = peutBouger;
    }

    ///////////À voir si on veut garder ////////////////////


    //public bool PrendreDegat(int nbrDegat)
    //{
    //	pointsDeVie -= nbrDegat;
    //	barreDeVie.value = pointsDeVie / (float)pointsDeVieMax;
    //	return pointsDeVie > 0;
    //}
    //void GestionPause()
    //{
    //	if (Input.GetButtonDown("Pause"))
    //	{
    //		menuPause.Pause();
    //	}
    //}
}
