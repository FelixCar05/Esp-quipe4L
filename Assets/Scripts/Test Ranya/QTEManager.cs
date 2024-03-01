using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QTEManager : MonoBehaviour
{

    private static QTEManager instance;
    public static QTEManager Instance { get { return instance; } }//singleton !!! pour que ça soit accessible facilement, une seule instance du script.


    [SerializeField] List<GameObject> BoutonsFleches; //les deux boutons du UI
    [SerializeField] GameObject BoutonsEmptyObject;//juste leur parent pour les activer et les desactiver
    //[SerializeField] Slider sliderTemps; //slider dont la valeur diminue avec le temps, décomtpe pour le QTE
    [SerializeField] List<Sprite> imagesBoutonsIdle; //sprites pour les boutons valides
    [SerializeField] List<Sprite> imagesBoutonsErreur; //sprites pour les boutons quand la mauvaise touche est appuyée
    [SerializeField] GameObject joueur;
    private List<float> rotationsBoutons; //rotations des flèches (il y a juste un seul sprite qu'on tourne pour faire up down left right)
    public List<float> entréesJoueur; //inputs du joueur (selon les rotations)
    private List<float> séquenceValide; //séquence déterminer à chaque association (en rotation)
    //private const float sliderTauxDescente = .001f; //valeur qui fait diminuer le slider selon un temps donné
    private CharacterController1 characterController; //le script character controller du joeur (c'est l'ancien, à mettre à jour)
    private bool QTEActif = false;

    private void Awake()//pour le singleton!!!
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
        characterController = joueur.GetComponent<CharacterController1>();
        rotationsBoutons = new() { 0, 90, 180, 270 }; //en haut, à gauche, en bas, à droite
        entréesJoueur = new();
        séquenceValide = new();
    }


    public void StartQTE(GameObject Ennemi)
    {
        //sliderTemps.gameObject.SetActive(true);
        Debug.Log("lets goooo");
        QTEActif = true;
        characterController.ChangerPeutMoveEtRotate(false);//bloque les actions du joueur
        AssignerFLeches();
        BoutonsEmptyObject.gameObject.SetActive(true);
        StartCoroutine(ÉchangerSpritesRoutine()); //pour l'esthétique mwahaha
    }
    public void EndQTE()
    {
        // débloque les mouvements du personnage
        QTEActif = false;
        BoutonsEmptyObject.gameObject.SetActive(false);
        characterController.ChangerPeutMoveEtRotate(true);
        //sliderTemps.gameObject.SetActive(false);
        //if (animateurCrawler.GetCurrentAnimatorStateInfo(0).IsName("Mordre"))//si le joueur ne s'est pas éloigné
        //    StartQTE(animateurCrawler.gameObject);
    }

    private void AssignerFLeches() //méthode qui donne détermine la séquence appropriée et l'affiche sur les boutons (en les tournant)
    {
        séquenceValide.Clear();
        entréesJoueur.Clear();

        foreach (GameObject bouton in BoutonsFleches)
        {
            bouton.GetComponent<Button>().interactable = true;
            bouton.GetComponent<Image>().sprite = imagesBoutonsIdle[0];
            bouton.transform.rotation = Quaternion.Euler(bouton.transform.rotation.x, bouton.transform.rotation.y, //on donne un rotation au hasard entre celles dans la liste
                                                         rotationsBoutons[Random.Range(0, 4)]);
            séquenceValide.Add(bouton.transform.rotation.eulerAngles.z);
        }
    }

    private void Update() //Détection des inputs du joueur
    {
        //GérerSliderDécompte();


        if (QTEActif)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                entréesJoueur.Add(rotationsBoutons[0]);
                VérifierSéquence();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                entréesJoueur.Add(rotationsBoutons[1]);
                VérifierSéquence();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                entréesJoueur.Add(rotationsBoutons[2]);
                VérifierSéquence();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                entréesJoueur.Add(rotationsBoutons[3]);
                VérifierSéquence();
            }
        }

    }

    //private void GérerSliderDécompte()
    //{
    //    if (sliderTemps.value > 0)
    //        sliderTemps.value -= sliderTauxDescente * Time.deltaTime;
    //    else
    //        sliderTemps.value = 0;
    //}

    private void VérifierSéquence()//vérifie si les inputs correspondent avec la séquence
    {
        for (int compteur = 0; compteur < entréesJoueur.Count; compteur++)
        {
            if (entréesJoueur[compteur] != séquenceValide[compteur]) //si l'entrée (dans le bon ordre) ne correspond pas avec son twin dans la séquence valide
            {
                BoutonsFleches[compteur].GetComponent<Image>().sprite = (compteur == 0) ? imagesBoutonsErreur[0] : imagesBoutonsErreur[1]; //rend le bouton rouge !
                StartCoroutine(GérerErreur());
                return;//quitte la boucle
            }
            else
            {
                BoutonsFleches[compteur].GetComponent<Button>().interactable = false; //rend le bouton gris (le cursor est locked donc le joueur ne pe pas les cliquer anyway,
                                                                                      //juste pour l'esthétique
                if (compteur == séquenceValide.Count - 1) //si la bonne séquence a été cliquée au complet
                {
                    //AssignerFLeches();
                    EndQTE();
                }

            }

        }
    }


    // Coroutine !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private IEnumerator GérerErreur()
    {
        yield return new WaitForSeconds(.5f);
        AssignerFLeches();
    }


    private IEnumerator ÉchangerSpritesRoutine()
    {
        int indexSprite = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            indexSprite = (indexSprite + 1) % 2;

            foreach (GameObject bouton in BoutonsFleches)
            {
                bouton.GetComponent<Image>().sprite = (indexSprite == 0) ? imagesBoutonsIdle[0] : imagesBoutonsIdle[1];
            }
        }
    }

}
