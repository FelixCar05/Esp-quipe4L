using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClignoterLumière : MonoBehaviour
{
    [SerializeField]
    Light lumière;
    [SerializeField]
    Material lumiereAllume;
    [SerializeField]
    Material lumiereFerme;
    [SerializeField]
    float tempsLibreMin = 0.04f;
    [SerializeField]
    float tempsLibreMax = 0.4f;
    MeshRenderer mesh;
    private bool estAllumé = true;
    private float tempPassé = 0;
    private float intervalTemps;
    Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        intervalTemps = UnityEngine.Random.Range(tempsLibreMin, tempsLibreMax);
        mesh = gameObject.GetComponent<MeshRenderer>();
        materials = mesh.materials; 
    }

    // Update is called once per frame
    void Update()
    {
        tempPassé += Time.deltaTime;
        if (tempPassé >= intervalTemps)
        {
            lumière.enabled = !estAllumé;
            estAllumé = !estAllumé;
            materials[1] = lumière.enabled ? lumiereAllume : lumiereFerme;
            mesh.materials = materials;
            intervalTemps = UnityEngine.Random.Range(tempsLibreMin, tempsLibreMax);
            tempPassé = 0;
        }

    }
}
