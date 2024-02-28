using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalleGen : MonoBehaviour
{
    public  Vector3 Position  // La postition de la salle 
	{
		get { return transform.position; }
	}
	bool EstVert = true;
	public float DistanceDepuisDepart { get; private set; } // Garder une distance depuis la premi�re salle 
	private bool empiler { get; set; }
	//Collision derni�reCollision;
	public bool collision1 = false;
	public List<PorteGen> Portes // La liste des portes que la salle poss�de
	{ get; private set; }
	BoxCollider boxCollider;
	bool m_Started = false;
	// Start is called before the first frame update
	void Awake()
    {
		Portes = new List<PorteGen>();
        boxCollider = GetComponent<BoxCollider>();

        m_Started = true;
    }


    public void AjouterPortes(PorteGen nouvellePorte)
	{
		Portes.Add(nouvellePorte);
	}
	public void ModifierDistance (Vector3 positionSalleDepart) 
	{
		DistanceDepuisDepart = Vector3.Distance(transform.position, positionSalleDepart);

	}


	public bool EstPositionLibre(GameObject sallePr�c�dente)
	{
		// BoxCollider boxColliderSallePr�c�dente = sallePr�c�dente.GetComponent<BoxCollider>();
		bool EstLibre = true;

		// theorie peut etre envoyer un raycast dans chacun des coin de la salle 2 fois donc 8 rayon qui regarde la distance de collision et si la collision est plus grand que 1 ou qqch dememe le champ est libre
		//return !boxCollider.bounds.Intersects(sallePr�c�dente.GetComponent<BoxCollider>().bounds);
		LayerMask layerMask = LayerMask.GetMask("Default") | LayerMask.GetMask("TransparentFX") | LayerMask.GetMask("Ignore Raycast") | LayerMask.GetMask("Joueur");
		BoxCollider colliderSallePr�c�dente = sallePr�c�dente.GetComponent<BoxCollider>();
		Collider[] ToutesCollisions = Physics.OverlapBox(transform.position, boxCollider.size / 2, Quaternion.identity, ~layerMask);
		foreach (Collider collision in ToutesCollisions)
		{

			if (collision != colliderSallePr�c�dente && collision != boxCollider)
			{
				
                EstVert = false;

                return false;
			}

		}
		return EstLibre;
	}

}
