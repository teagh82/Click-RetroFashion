using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Text클래스 사용을 위해 추가

public class DialogueManager : MonoBehaviour
{
    [SerializeField] GameObject go_DialogueBar;
    [SerializeField] GameObject go_DialogueNameBar;

    //대화를 위한 변수
    [SerializeField] Text txt_Dialogue;
    [SerializeField] Text txt_Name;

    Dialogue[] dialogues;

    bool isDialogue = false; //대화 판별 여부
    bool isNext = false; //특정 키 입력 대기 

    [Header("텍스트 출력 딜레이.")]
    [SerializeField] float textDelay;

    int lineCount = 0; //대화 카운트 
    int contextCount = 0; //대사 카운트 

    InteractionController theIC;
    CameraController theCam;
    SpriteManager theSpriteManager;
    SplashManager theSplashManager;
    CutSceneManager theCutSceneManager;

    void Start() {
        theIC = FindObjectOfType<InteractionController>(); //IC로 interaction 참조 가능
        theCam = FindObjectOfType<CameraController>();
        theSpriteManager = FindObjectOfType<SpriteManager>();
        theSplashManager = FindObjectOfType<SplashManager>();
        theCutSceneManager = FindObjectOfType<CutSceneManager>();
    }

    void Update() //대화 넘기기 
    {
        if (isDialogue)
        {
            if (isNext)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isNext = false;
                    txt_Dialogue.text = "";
                    if(++contextCount < dialogues[lineCount].contexts.Length)
                    {
                        StartCoroutine(TypeWriter());
                    }
                    else
                    {
                        contextCount = 0;
                        if(++lineCount < dialogues.Length)
                        {
                            StartCoroutine(CameraTargettingType());
                        }
                        else
                        {
                            StartCoroutine(EndDialogue());
                        }
                    }
                }
            }
        }
    }
   
    public void ShowDialogue(Dialogue[] p_dialogues) {
        isDialogue = true;
        txt_Dialogue.text = "";//텍스트 빈 값으로 초기화
        txt_Name.text = "";
        theIC.SettingUI(false); //InteractionController에 있음.
        dialogues = p_dialogues;
        theCam.CamOriginSetting();

        StartCoroutine(CameraTargettingType());


    }

    IEnumerator CameraTargettingType()
    {
        switch (dialogues[lineCount].cameraType)
        {
            case CameraType.FadeIn:
                SettingUI(false);
                SplashManager.isFinished = false; 
                StartCoroutine(theSplashManager.FadeIn(false, true)); 
                yield return new WaitUntil(() => SplashManager.isFinished); break;
            case CameraType.FadeOut:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(theSplashManager.FadeOut(false, true));
                yield return new WaitUntil(() => SplashManager.isFinished); break;
            case CameraType.FlashIn:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(theSplashManager.FadeIn(true, true));
                yield return new WaitUntil(() => SplashManager.isFinished); break;
            case CameraType.FlashOut:
                SettingUI(false);
                SplashManager.isFinished = false;
                StartCoroutine(theSplashManager.FadeOut(true, true));
                yield return new WaitUntil(() => SplashManager.isFinished); break;

            case CameraType.ObjectFront: theCam.CameraTargetting(dialogues[lineCount].tf_Target); break;
            case CameraType.Reset: theCam.CameraTargetting(null, 0.05f, true, false); break;

            case CameraType.ShowCutScene: SettingUI(false); CutSceneManager.isFinished = false; StartCoroutine(theCutSceneManager.CutSceneCoroutine(dialogues[lineCount].spriteName[contextCount], true)); yield return new WaitUntil(()=>CutSceneManager.isFinished); break;
            case CameraType.HideCutScene:
                SettingUI(false); CutSceneManager.isFinished = false;
                StartCoroutine(theCutSceneManager.CutSceneCoroutine(null, false));
                yield return new WaitUntil(()=>CutSceneManager.isFinished);
                theCam.CameraTargetting(dialogues[lineCount].tf_Target);
                break;
        }

        StartCoroutine(TypeWriter());
    }

    IEnumerator EndDialogue() //대화 종료 
    {
        if(theCutSceneManager.CheckCutScene()){
            CutSceneManager.isFinished = false;
            StartCoroutine(theCutSceneManager.CutSceneCoroutine(null, false));
            yield return new WaitUntil(()=>CutSceneManager.isFinished);
        }
        isDialogue = false;
        contextCount = 0;
        lineCount = 0;
        dialogues = null;
        isNext = false;
        theCam.CameraTargetting(null, 0.05f, true, true);
        
        SettingUI(false);
    }

    void ChangeSprite() {
        if(dialogues[lineCount].tf_Target != null){
            if (dialogues[lineCount].spriteName[contextCount] != "") {
                StartCoroutine(theSpriteManager.SpriteChangeCoroutine(
                                                dialogues[lineCount].tf_Target, 
                                                dialogues[lineCount].spriteName[contextCount]));
            }
        }
    }

    void PlaySound(){
        if(dialogues[lineCount].VoiceName[contextCount] != ""){
            SoundManager.instance.PlaySound(dialogues[lineCount].VoiceName[contextCount], 2);
        }
    }

    IEnumerator TypeWriter() //대화내용 불러와서 출력 
    {

        SettingUI(true); //처음은 활성화
        ChangeSprite();
        PlaySound();

        string t_ReplaceText = dialogues[lineCount].contexts[contextCount]; //대화 한 줄이 들어감.
        t_ReplaceText = t_ReplaceText.Replace("'", ","); //'(싱글코트)를 ,로 바꿈 -> 치환됨
        t_ReplaceText = t_ReplaceText.Replace("\\n", "\n");

        

        bool t_white = false, t_yellow = false, t_cyan = false;
        bool t_ignore = false;

        for(int i = 0; i < t_ReplaceText.Length; i++)
        {
            switch (t_ReplaceText[i])
            {
                case 'ⓦ': t_white = true; t_yellow = false; t_cyan = false; t_ignore = true; break;
                case 'ⓨ': t_white = false; t_yellow = true; t_cyan = false; t_ignore = true; break;
                case 'ⓒ': t_white = false; t_yellow = false; t_cyan = true;  t_ignore = true; break;
                case '①' : StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("Emotion1", 1); t_ignore = true; break;
                case '②' : StartCoroutine(theSplashManager.Splash()); SoundManager.instance.PlaySound("Emotion2", 1); t_ignore = true; break;
            }

            string t_letter = t_ReplaceText[i].ToString();

            if (!t_ignore)
            {
                if (t_white) { t_letter = "<color=#ffffff>" + t_letter + "</color>"; }
                else if(t_yellow) { t_letter = "<color=#FFFA43>" + t_letter + "</color>"; }
                else if (t_cyan) { t_letter = "<color=#42DEE3>" + t_letter + "</color>"; }
                txt_Dialogue.text += t_letter;
            }
            t_ignore = false;
        
            yield return new WaitForSeconds(textDelay);
        }

        isNext = true;
        
    }


    void SettingUI(bool p_flag) { //UI활성화, 비활성화 함수
        go_DialogueBar.SetActive(p_flag);

        if (p_flag)
        {
            if (dialogues[lineCount].name == "")
            {
                go_DialogueNameBar.SetActive(false);
            }
            else
            {
                go_DialogueNameBar.SetActive(true);
                txt_Name.text = dialogues[lineCount].name;
            }
        }
        else 
        {
            go_DialogueNameBar.SetActive(false);
        }

        

    }
}
