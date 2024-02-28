using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class JouerVideo : MonoBehaviour
{
    // Start is called before the first frame update
    VideoPlayer vidéo;
    MeshRenderer mesh;

    void Start()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
        mesh.enabled = false;
        vidéo = GetComponent<VideoPlayer>();
        vidéo.Play();
        vidéo.Pause();//pour éviter d'avoir un rectangle gris en plein centre de l'écran
    }
    
    // Update is called once per frame
    void Update()
    {
    }
    public void DégoulinerSang()
    {
        mesh.enabled = true;
        vidéo.Play();
    }
    public void PauserVideo()
    {
        vidéo.Pause();
        mesh.enabled = false;
    }
}
