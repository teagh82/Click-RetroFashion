using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferManager : MonoBehaviour
{
    SplashManager splash;
    InteractionController theIC;
    // InteractionController IC;
    bool isChange = false;

    public static bool isFinished = true;

    string locationName;

    public string GetLocationName()
    {
        return locationName;
    }


    public IEnumerator Transfer(string p_SceneName, string p_LocationName)
    {
        isFinished = false;

        theIC.SettingUI(false);

        SplashManager.isFinished = false;

        StartCoroutine(splash.FadeOut(false, true));
        yield return new WaitUntil(() => SplashManager.isFinished);
        isChange = true;
        TransferSpawnManager.spawnTiming = true;
        SceneManager.LoadScene(p_SceneName);
    }

    public IEnumerator Done()
    {
        isChange = false;

        SplashManager.isFinished = false;

        StartCoroutine(splash.FadeIn(false, true));
        yield return new WaitUntil(() => SplashManager.isFinished);

        isFinished = true;
        theIC.SettingUI(true);

    }

    // Start is called before the first frame update
    void Start()
    {
        splash = this.gameObject.transform.GetComponent<SplashManager>();
        //  IC = this.gameObject.transform.GetComponent<InteractionController>();
        theIC = FindObjectOfType<InteractionController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isChange == true)
            StartCoroutine("Done");    
    }
}