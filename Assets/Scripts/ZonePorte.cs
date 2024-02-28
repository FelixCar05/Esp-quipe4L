using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonePorte : MonoBehaviour
{
    [SerializeField]
    Animator animationPorte;

    // Start is called before the first frame update
    void Awake()
    {
        animationPorte = GetComponentInParent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Joueur") || collision.CompareTag("Entité"))
        animationPorte.SetBool("open", true);
    }
}
