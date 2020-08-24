using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{
    public UnityEngine.UI.Text StartText = null;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("ChangeText", 5f);
    }

    void ChangeText()
    {
        StartText.text = "Chapter1. 빌리를 찾아서";
        Invoke("MoveScene", 2f);
    }

    void MoveScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Find Bill");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
