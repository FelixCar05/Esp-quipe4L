using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PorteGen : MonoBehaviour
{
    [SerializeField]
    BoxCollider ZoneTrigger1;
    [SerializeField]
    BoxCollider ZoneTrigger2;
    [SerializeField] private bool ouvert;

    public bool Ouvert
    {
        get
        {
            return ouvert;
        }
        set
        {
            ouvert = value;
            if (!value)
            {
                ZoneTrigger1.isTrigger = false;
                ZoneTrigger2.isTrigger = false;
            }
        }
    }
    SalleGen scriptDeSalle;

    [SerializeField] Vector3 Position { get; set; }
    void Awake()
    {
        Ouvert = false;
        scriptDeSalle = GetComponentInParent<SalleGen>();
        scriptDeSalle.AjouterPortes(this);
        Position = transform.position;
    }
    public void OuvrirPorte()
    {
        Ouvert = true;
    }
}
