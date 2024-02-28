using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutonChangementDeScn : MonoBehaviour
{
    

    // Start is called before the first frame update

    public void BoutonChangerScene()
    {
        StartCoroutine(Attendre());
       
    }

   IEnumerator Attendre()
    {
        yield return new
        WaitForSeconds(9);
        SceneManager.LoadScene("SceneTest", LoadSceneMode.Single);
    }


}
