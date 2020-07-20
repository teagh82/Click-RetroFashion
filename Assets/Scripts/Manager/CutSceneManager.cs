using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutSceneManager : MonoBehaviour
{
    public static bool isFinished = false;

    SplashManager theSplashManager;
    CameraController theCam;

    [SerializeField] Image img_CutScene;

    // Start is called before the first frame update
    void Start()
    {
        theSplashManager = FindObjectOfType<SplashManager>();
        theCam = FindObjectOfType<CameraController>();
    }

    public bool CheckCutScene(){
        return img_CutScene.gameObject.activeSelf;
    }

    public IEnumerator CutSceneCoroutine(string p_CutSceneName, bool p_isShow){
        SplashManager.isFinished = false;
        StartCoroutine(theSplashManager.FadeOut(true, false));
        yield return new WaitUntil(()=>SplashManager.isFinished);

        if(p_isShow){
            Sprite t_Sprite = Resources.Load<Sprite>("CutScenes/" + p_CutSceneName);
            if(t_Sprite != null){
                img_CutScene.gameObject.SetActive(true);
                img_CutScene.sprite = t_Sprite;
                theCam.CameraTargetting(null, 0.1f, true, false);
            }else{
                Debug.LogError("잘못된 컷신 CG 파일 이름입니다.");
            }
        }else{
            img_CutScene.gameObject.SetActive(false);
        }

        SplashManager.isFinished = false;
        StartCoroutine(theSplashManager.FadeIn(true, false));
        yield return new WaitUntil(()=>SplashManager.isFinished);

        //잠깐 시간 지연을 줌
        yield return new WaitForSeconds(0.5f);

        isFinished = true;
    }
}