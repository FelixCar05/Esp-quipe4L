using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscilliation : MonoBehaviour
{
    [SerializeField]
    int fréquence = 1;
    [SerializeField]
    int amplitude = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x,amplitude+  (Mathf.Sin(Time.time*fréquence)*amplitude), transform.position.z);
    }
}
