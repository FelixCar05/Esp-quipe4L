using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class JouerVideo : MonoBehaviour
{
    // Start is called before the first frame update
    VideoPlayer vid�o;
    MeshRenderer mesh;

    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.enabled = false;
        vid�o = GetComponent<VideoPlayer>();
        vid�o.Play();
        vid�o.Pause();//pour �viter d'avoir un rectangle gris en plein centre de l'�cran
    }
    
    // Update is called once per frame
    void Update()
    {
    }
    public void D�goulinerSang()
    {
        mesh.enabled = true;
        vid�o.Play();
    }
    public void PauserVideo()
    {
        vid�o.Pause();
        mesh.enabled = false;
    }
}
