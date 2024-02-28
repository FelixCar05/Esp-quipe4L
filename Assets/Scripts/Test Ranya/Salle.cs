using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Salle : MonoBehaviour
{
    public bool d�couverte; /*{ get; private set; }*/
    public List<GameObject> listeEnnemisDansSalle{ get; private set; }
    
    void Start()
    {
        d�couverte = false; 
        listeEnnemisDansSalle = new List<GameObject>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Joueur"))
            d�couverte = true; 
        else if (other.gameObject.CompareTag("Ennemi") && !listeEnnemisDansSalle.Contains(other.gameObject))
        {
            listeEnnemisDansSalle.Add(other.gameObject);


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ennemi"))
            listeEnnemisDansSalle.Remove(other.gameObject);
    }
}
