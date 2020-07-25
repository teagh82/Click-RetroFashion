using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Color;

public class InteractionController : MonoBehaviour
{
    [SerializeField] Camera cam; //카메라
    RaycastHit hitInfo; //레이저 정보
    [SerializeField] GameObject go_NormalCrosshair;
    [SerializeField] GameObject go_InteractiveCrosshair;
    [SerializeField] GameObject go_Crosshair;
    //[SerializeField] GameObject go_Cursor;
    [SerializeField] GameObject go_TargetNameBar;
    [SerializeField] Text txt_TargetName;

    bool isContact = false;//중복 실행 막기
    public static bool isInteract = false; //인터렉트 함수에서 쓰임 "상호작용 중이다 아니다 구분"
    [SerializeField] ParticleSystem ps_QuestionEffect; //투사체 관리

    [SerializeField] Image img_Interaction;
    [SerializeField] Image img_InteractionEffect;

    DialogueManager theDM;

    public void SettingUI(bool p_flag)
    {
        go_Crosshair.SetActive(p_flag);
        //go_Cursor.SetActive(p_flag);


        if (!p_flag)
        {
            StopCoroutine("Interaction");
            Color color = img_Interaction.color;
            color.a = 0;
            img_Interaction.color = color;
            go_TargetNameBar.SetActive(false);
        }
        else
        { go_NormalCrosshair.SetActive(true);
            go_InteractiveCrosshair.SetActive(false);
        }

        isInteract = !p_flag;
    }

        void Start() {
        theDM = FindObjectOfType<DialogueManager>();
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteract)
        {
            CheckObject(); //객체 확인용
            ClickLeftBtn();//마우스 좌클릭 강제하는 함수
        }
    }

    void CheckObject() {

        Vector3 t_MousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        //마우스 위치값 저장
        if(Physics.Raycast(cam.ScreenPointToRay(t_MousePos), out hitInfo,100)){
            Contact();
        }
        else {
            NotContact();
        }
    }
    void Contact() {
        if (hitInfo.transform.CompareTag("Interaction"))
        {//충돌 객체 분석
            go_TargetNameBar.SetActive(true);
            txt_TargetName.text = hitInfo.transform.GetComponent<InteractionType>().GetName();
            if (!isContact) {
                isContact = true; //최초 실행시, true로 바꾼다.
                go_InteractiveCrosshair.SetActive(true);
                go_NormalCrosshair.SetActive(false);
                StopCoroutine("Interaction");
                StopCoroutine("InteractionEffect");
                StartCoroutine("Interaction", true);
                StartCoroutine("InteractionEffect");
            }
            
        }
        else {
            NotContact();
        }
    }
    void NotContact() {
        go_TargetNameBar.SetActive(false);
        if (isContact) {
            go_TargetNameBar.SetActive(false);
            isContact = false;
            go_InteractiveCrosshair.SetActive(false);
            go_NormalCrosshair.SetActive(true);
            StopCoroutine("Interaction");
            StartCoroutine("Interaction", false);
        }
       
    }


    IEnumerator Interaction(bool p_Appear)
    {
        Color color = img_Interaction.color;
        if (p_Appear)
        {
            color.a = 0;        //0은 투명
            while(color.a < 1)  //1은 불투명
            {
                color.a += 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        } else
        {
            while (color.a > 0)
            {
                color.a -= 0.1f;
                img_Interaction.color = color;
                yield return null;
            }
        }
    }

    IEnumerator InteractionEffect()
    {
        while (isContact && !isInteract) //상호작용 가능한 객체위에 있으면서 마우스 좌클릭 안함
        {
            Color color = img_InteractionEffect.color;
            color.a = 0.5f; //반투명

            img_InteractionEffect.transform.localScale = new Vector3(1, 1, 1);
            Vector3 t_scale = img_InteractionEffect.transform.localScale;

            while (color.a > 0)
            {
                color.a -= 0.01f; //천천히 투명해짐
                img_InteractionEffect.color = color;
                t_scale.Set(t_scale.x + Time.deltaTime, t_scale.y + Time.deltaTime, t_scale.z + Time.deltaTime);
                img_InteractionEffect.transform.localScale = t_scale;
                yield return null;
                //투명해지면서 커짐
            }
            yield return null;
        }
    }


    void ClickLeftBtn() {
        if (!isInteract){ //상호작용 중에는 클릭이 불가능 해야한다.
            if (Input.GetMouseButtonDown(0))
            {
                if (isContact)
                {
                    Interact();
                }
            }
        }//클릭을 강제함-이것은 마우스 좌클릭을 인식.
    }
    void Interact() {
        isInteract = true; //상호작용 중이다. 

        StopCoroutine("Interaction");
        Color color = img_Interaction.color;
        color.a = 0;
        img_Interaction.color = color;

        ps_QuestionEffect.gameObject.SetActive(true); //활성화
        Vector3 t_targetPos = hitInfo.transform.position;
        ps_QuestionEffect.GetComponent<Question>().SetTarget(t_targetPos); //목적지 설정
        ps_QuestionEffect.transform.position = cam.transform.position;//투사체 위치를 카메라의 위치로 - 자기 자신이 던지는 것 처럼 보이기 위해

        StartCoroutine(WaitCollision());
    }

    IEnumerator WaitCollision(){ //대화창 띄우기위해 상호작용 이펙트가 대상에 충돌할때까지 기다린다.- 대기시키는 기능 
        yield return new WaitUntil(()=>Question.isCollide); //WaitUntill은 조건 전까지 계속 대기시킴(bool isCollide). 특정조건은 QuestionEffect에서 만들어줌
        Question.isCollide = false;//충돌했다면 다시 원래의 값으로 

        InteractionEvent t_Event = hitInfo.transform.GetComponent<InteractionEvent>();

        //경우에 따라서 장소 이동
        if(hitInfo.transform.GetComponent<InteractionType>().isObject)
        {
            DialogueCall(t_Event);
        }
        else
        {
            TransferCall();
        }

        //theDM.ShowDialogue();
    }
    
    void TransferCall()
    {
        string t_SceneName = hitInfo.transform.GetComponent<InteractionDoor>().GetSceneName();
        string t_LocationName = hitInfo.transform.GetComponent<InteractionDoor>().GetLocationName();
        StartCoroutine(FindObjectOfType<TransferManager>().Transfer(t_SceneName, t_LocationName));
    }

    void DialogueCall(InteractionEvent p_Event) //윗부분 함수로 뺌
    {
        if (DatabaseManager.instance.eventFlags[p_Event.GetEventNumber()])
        {
            theDM.SetNextEvent(p_Event.GetNextEvent());
            if (p_Event.GetAppearType() == AppearType.Appear) theDM.SetAppearObjects(p_Event.GetTargets());
            else if (p_Event.GetAppearType() == AppearType.Disappear) theDM.SetDisappearObjects(p_Event.GetTargets());

        }
        theDM.ShowDialogue(p_Event.GetDialogue());
    }
}

