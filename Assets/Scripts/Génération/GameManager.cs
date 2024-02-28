using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Point de départ
    Vector3 pointDépart;
    // Salles à générer
    [SerializeField]
    List<GameObject> sallesDisponible = new List<GameObject>(); // Prefab des salles

    [SerializeField]
    GameObject salleBoss;

    private List<GameObject> sallesCourrantes = new List<GameObject>(); // Salle déja generer

    //Random Salle

    int NbrSalle;

    //SalleEnTout
    [SerializeField]
    int nbrSalleLimite = 20;



    //Vector3 PositionAncienneSalle;

    private int i = 0;

    void Start()
    {
        pointDépart = transform.position; // Point de depart
        nbrSalleLimite -= 1; // Pour la salle de boss tjrs garder un spot libre
        GénérerPremièreSalle();
        StartCoroutine(ChargerSalle(0)); // S'assurer que ca commence a la premiere salle
        
        //NbrSalle = sallesDisponible.Count; // Le nombre de prefab de salle
        

    }
    

    private void GénérerPremièreSalle()
    {
        sallesCourrantes.Add(Instantiate(sallesDisponible[0], pointDépart, Quaternion.identity)); // Instancier la salle de depart
        //PositionAncienneSalle = pointDépart;
    }
   
    IEnumerator GénérerDernièreSalle()
    {
        yield return new WaitForSeconds(3f);
        //D'abord retirer dans salles disponibles toutes les salles sauf la salle a une porte et ajouter la salle de boss
        sallesDisponible.RemoveRange(1, sallesDisponible.Count-1);
        sallesDisponible.Add(salleBoss);
        //Ensuite augmenter de un le nombre de salle limite pour rentrer la salle du boss
        nbrSalleLimite+= 1;
        //Ensuite appeler la méthode ChargerSalle qui va générer la salle de bosse à un endroit précis toutefois rajouter a la fin 
        //de la liste salleCourantes la salle la plus loin pour s'assurer que la salle de boss soit loin
        int indice = TrouverSallePlusLoin();
        //Générer la salle en envoyant en paramètre l'indice de la salle la plus éloignée
        StartCoroutine(ChargerSalle(indice));
        Debug.Log(indice);
        Debug.Log(sallesCourrantes.Count);
    }
    private IEnumerator ChargerSalle(int i)
    {
        do
        {
			//créer la liste des portes disponibles pour la salle actuelle
			Debug.Log(i);
			List<PorteGen> ListeDesPortes = sallesCourrantes[i].GetComponent<SalleGen>().Portes;


            for (int j = 0; j < ListeDesPortes.Count; j++)
            {
                // chercher chacune des porte dans la salle actuelle
                if (sallesCourrantes.Count < nbrSalleLimite)
                {
                    //yield return new WaitForSeconds(0.1f);
					yield return new WaitForSeconds(4f);
					//StartCoroutine(PlacerSalle(ListeDesPortes[j], sallesCourrantes[i])); Version Officielle
					//PlacerSalleTest(ListeDesPortes[j], sallesCourrantes[i]); test
					StartCoroutine(PlacerSalle(ListeDesPortes[j], sallesCourrantes[i]));
                }
            }
            //yield return new WaitForSeconds(0.025f);
			yield return new WaitForSeconds(3f);

			// Une fois que toutes les portes ont une salle connectée on passe à la prochaine
			i++;
            //Debug.Log(i);

        } while (sallesCourrantes.Count < nbrSalleLimite);
        // Appeler fonction qui vient v/rifier si temps de placer la derniere salle sans le refaire plusieurs fois.
    }

    private IEnumerator PlacerSalle(PorteGen porte, GameObject sallePlacée)
    {
        if (!porte.Ouvert) // Si la porte n<est pas déjà utilisée
        {
            // Prendre la liste des différentes types de salle 
            List<GameObject> listeTypeSalleRestante = sallesDisponible.ToList();
            while (listeTypeSalleRestante.Count != 1) 
            {
                int indiceSalle = Random.Range(1, listeTypeSalleRestante.Count); // Type de salle random
                                                                                 //Potentiel if pour conditions + faudrait se faire une liste des salle plausible pour essayer chacune des salles
                                                                                 //choisir la salle à instancier  


                //GameObject salleAplacer = Instantiate(sallesDisponible[indiceSalle], porte.transform.position, Quaternion.identity);
                GameObject salleAplacer = Instantiate(listeTypeSalleRestante[indiceSalle], porte.transform.position, Quaternion.identity);
                bool EstsallePlacée = false; // Permettre de savoir si la salle a ete placer

                // Aller chercher le script de la salle a placer
                SalleGen salleAPlacerSalleGen = salleAplacer.GetComponent<SalleGen>();
                // Aller chercher la liste des portes de la salle a placer
                // Garder une liste de portes de la salle choisie pour tester toutes les possibilités
                List<PorteGen> listePortesDisponibles = salleAPlacerSalleGen.Portes;
                // Choisit une porte au hasard


                while (listePortesDisponibles.Count != 0)
                {
                    //Debug.Log($"nbr de portes {listePortesDisponibles.Count} de la salle {salleAplacer}");
                    PorteGen porteSalleAPlacer = listePortesDisponibles[Random.Range(0, listePortesDisponibles.Count)];
                    //changer la rotation de la salle
                    float DiffRotation = Mathf.Round(porteSalleAPlacer.transform.rotation.eulerAngles.y - porte.transform.rotation.eulerAngles.y);
                    switch (DiffRotation)
                    {
                        case (90f):
                            salleAplacer.transform.Rotate(Vector3.up * DiffRotation);
                            break;
                        case (-90f):
                            salleAplacer.transform.Rotate(Vector3.up * DiffRotation);
                            break;
                        case (0f):
                            salleAplacer.transform.Rotate(Vector3.up * 180);
                            break;
                        case (270f):
                            salleAplacer.transform.Rotate(Vector3.up * -90);
                            break;

                        case (-270f):
                            salleAplacer.transform.Rotate(Vector3.up * 90);
                            break;
                    }

                    //Changement de position pour fiter avec la salle
                    Vector3 bouger = porteSalleAPlacer.transform.position - porte.transform.position;
                    salleAplacer.transform.position -= bouger;
                    //yield return new WaitForSeconds(0.025f);
					yield return new WaitForSeconds(3f);
					//StartCoroutine(Attendre(salleAPlacerSalleGen,sallePlacée));
					// Regarder si la salle n<entre pas en collision avec d<autres salles
					if (salleAPlacerSalleGen.EstPositionLibre(sallePlacée))
                    {
                        sallesCourrantes.Add(salleAplacer);
                        Destroy(porteSalleAPlacer.gameObject);
                        salleAPlacerSalleGen.Portes.Remove(porteSalleAPlacer);
                        //Debug.Log(porteSalleAPlacer.gameObject);
                        //porteSalleAPlacer.FermerPorte();
                        porte.OuvrirPorte();
                        salleAPlacerSalleGen.ModifierDistance(sallesCourrantes[0].transform.position);
                        EstsallePlacée = true;
                        break;
                    }
                    else
                    {
                        listePortesDisponibles.Remove(porteSalleAPlacer);
                    }
                }
                if (!EstsallePlacée)
                {
                    listeTypeSalleRestante.RemoveAt(indiceSalle);
                    Destroy(salleAplacer);
                }
                else
                {
                    break;
                }

            }
        }
    }
    private int TrouverSallePlusLoin()
    {
        SalleGen sallePlusLoinActuelle = sallesCourrantes[0].GetComponent<SalleGen>();
        int indice= 1;
        for(int i = 1; i < sallesCourrantes.Count; i++)
        {
            SalleGen salleActuelle = sallesCourrantes[i].GetComponent<SalleGen>();
            if (salleActuelle.DistanceDepuisDepart > sallePlusLoinActuelle.DistanceDepuisDepart && AuMoinsUnePorteLibre(salleActuelle))
            {
                sallePlusLoinActuelle = salleActuelle;
                indice= i;
            }
        }
        return indice;
    }
    // Regarder si il y a au moins une porte ouverte 
    private bool AuMoinsUnePorteLibre(SalleGen Salle)
    {
        for (int i = 0; i < Salle.Portes.Count; i++)
        {
            if (Salle.Portes[i].Ouvert) { return true; }
        }
        return false;
    }
}


//private IEnumerator GénérerDernièreSalle()
//{
//    GameObject salleplusloin =  sallesCourrantes[1];
//    float distancePlusLoin = salleplusloin.GetComponent<SalleGen>().DistanceDepuisDepart;
//    float distance;
//    foreach(GameObject salle in sallesCourrantes)
//    {
//        distance = salle.GetComponent<SalleGen>().DistanceDepuisDepart;
//        if (distance> distancePlusLoin)
//        {
//            salleplusloin = salle;
//        }
//    }
//    SalleGen salleBossGen = salleBoss.GetComponent<SalleGen>();
//    PorteGen porteSalleAPlacer = salleBossGen.Portes[0];

//    foreach (PorteGen porte in salleplusloin.GetComponent<SalleGen>().Portes)
//    {
//        if (porte.Ouvert)
//        {

//            //changer la rotation de la salle
//            float DiffRotation = Mathf.Round(porteSalleAPlacer.transform.rotation.eulerAngles.y - porte.transform.rotation.eulerAngles.y);
//            switch (DiffRotation)
//            {
//                case (90f):
//                    salleBoss.transform.Rotate(Vector3.up * DiffRotation);
//                    break;
//                case (-90f):
//                    salleBoss.transform.Rotate(Vector3.up * DiffRotation);
//                    break;
//                case (0f):
//                    salleBoss.transform.Rotate(Vector3.up * 180);
//                    break;
//                case (270f):
//                    salleBoss.transform.Rotate(Vector3.up * -90);
//                    break;

//                case (-270f):
//                    salleBoss.transform.Rotate(Vector3.up * 90);
//                    break;
//            }
//            //Changement de position pour fiter avec la salle
//            Vector3 bouger = porteSalleAPlacer.transform.position - porte.transform.position;
//            salleBoss.transform.position -= bouger;
//            yield return new WaitForSeconds(0.2f);
//            // Regarder si la salle n<entre pas en collision avec d<autres salles
//            if (salleBossGen.EstPositionLibre(salleplusloin))
//            {
//                sallesCourrantes.Add(salleBoss);
//                porteSalleAPlacer.FermerPorte();
//                porte.FermerPorte();
//                break;
//            }
//            else
//            {
//                Destroy(salleBoss);
//            }
//        }
//    }
//}
//Une coroutine pour generer chaque salle et s<assurer que les collisions marchent