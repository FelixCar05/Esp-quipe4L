using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QTEManager : MonoBehaviour
{

    private static QTEManager instance;
    public static QTEManager Instance { get { return instance; } }//singleton !!! pour que �a soit accessible facilement, une seule instance du script.


    [SerializeField] List<GameObject> BoutonsFleches; //les deux boutons du UI
    [SerializeField] GameObject BoutonsEmptyObject;//juste leur parent pour les activer et les desactiver
    //[SerializeField] Slider sliderTemps; //slider dont la valeur diminue avec le temps, d�comtpe pour le QTE
    [SerializeField] List<Sprite> imagesBoutonsIdle; //sprites pour les boutons valides
    [SerializeField] List<Sprite> imagesBoutonsErreur; //sprites pour les boutons quand la mauvaise touche est appuy�e
    [SerializeField] GameObject joueur;
    private List<float> rotationsBoutons; //rotations des fl�ches (il y a juste un seul sprite qu'on tourne pour faire up down left right)
    public List<float> entr�esJoueur; //inputs du joueur (selon les rotations)
    private List<float> s�quenceValide; //s�quence d�terminer � chaque association (en rotation)
    //private const float sliderTauxDescente = .001f; //valeur qui fait diminuer le slider selon un temps donn�
    private CharacterController1 characterController; //le script character controller du joeur (c'est l'ancien, � mettre � jour)
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
        rotationsBoutons = new() { 0, 90, 180, 270 }; //en haut, � gauche, en bas, � droite
        entr�esJoueur = new();
        s�quenceValide = new();
    }


    public void StartQTE(GameObject Ennemi)
    {
        //sliderTemps.gameObject.SetActive(true);
        Debug.Log("lets goooo");
        QTEActif = true;
        characterController.ChangerPeutMoveEtRotate(false);//bloque les actions du joueur
        AssignerFLeches();
        BoutonsEmptyObject.gameObject.SetActive(true);
        StartCoroutine(�changerSpritesRoutine()); //pour l'esth�tique mwahaha
    }
    public void EndQTE()
    {
        // d�bloque les mouvements du personnage
        QTEActif = false;
        BoutonsEmptyObject.gameObject.SetActive(false);
        characterController.ChangerPeutMoveEtRotate(true);
        //sliderTemps.gameObject.SetActive(false);
        //if (animateurCrawler.GetCurrentAnimatorStateInfo(0).IsName("Mordre"))//si le joueur ne s'est pas �loign�
        //    StartQTE(animateurCrawler.gameObject);
    }

    private void AssignerFLeches() //m�thode qui donne d�termine la s�quence appropri�e et l'affiche sur les boutons (en les tournant)
    {
        s�quenceValide.Clear();
        entr�esJoueur.Clear();

        foreach (GameObject bouton in BoutonsFleches)
        {
            bouton.GetComponent<Button>().interactable = true;
            bouton.GetComponent<Image>().sprite = imagesBoutonsIdle[0];
            bouton.transform.rotation = Quaternion.Euler(bouton.transform.rotation.x, bouton.transform.rotation.y, //on donne un rotation au hasard entre celles dans la liste
                                                         rotationsBoutons[Random.Range(0, 4)]);
            s�quenceValide.Add(bouton.transform.rotation.eulerAngles.z);
        }
    }

    private void Update() //D�tection des inputs du joueur
    {
        //G�rerSliderD�compte();


        if (QTEActif)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                entr�esJoueur.Add(rotationsBoutons[0]);
                V�rifierS�quence();
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                entr�esJoueur.Add(rotationsBoutons[1]);
                V�rifierS�quence();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                entr�esJoueur.Add(rotationsBoutons[2]);
                V�rifierS�quence();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                entr�esJoueur.Add(rotationsBoutons[3]);
                V�rifierS�quence();
            }
        }

    }

    //private void G�rerSliderD�compte()
    //{
    //    if (sliderTemps.value > 0)
    //        sliderTemps.value -= sliderTauxDescente * Time.deltaTime;
    //    else
    //        sliderTemps.value = 0;
    //}

    private void V�rifierS�quence()//v�rifie si les inputs correspondent avec la s�quence
    {
        for (int compteur = 0; compteur < entr�esJoueur.Count; compteur++)
        {
            if (entr�esJoueur[compteur] != s�quenceValide[compteur]) //si l'entr�e (dans le bon ordre) ne correspond pas avec son twin dans la s�quence valide
            {
                BoutonsFleches[compteur].GetComponent<Image>().sprite = (compteur == 0) ? imagesBoutonsErreur[0] : imagesBoutonsErreur[1]; //rend le bouton rouge !
                StartCoroutine(G�rerErreur());
                return;//quitte la boucle
            }
            else
            {
                BoutonsFleches[compteur].GetComponent<Button>().interactable = false; //rend le bouton gris (le cursor est locked donc le joueur ne pe pas les cliquer anyway,
                                                                                      //juste pour l'esth�tique
                if (compteur == s�quenceValide.Count - 1) //si la bonne s�quence a �t� cliqu�e au complet
                {
                    //AssignerFLeches();
                    EndQTE();
                }

            }

        }
    }


    // Coroutine !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    private IEnumerator G�rerErreur()
    {
        yield return new WaitForSeconds(.5f);
        AssignerFLeches();
    }


    private IEnumerator �changerSpritesRoutine()
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
