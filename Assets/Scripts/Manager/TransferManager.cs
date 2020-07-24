using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    public IEnumerator Transfer(string p_SceneName, string p_LocationName)
    {
        yield return null;

        SceneManager.LoadScene(p_SceneName);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
