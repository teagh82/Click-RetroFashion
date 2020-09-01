using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour {

    public Image CursorGageImage;
    // 커서 이미지를 저장하는 변수

    public Text TextUI;
    // UI의 Text를 저장하는 변수

    private Cardboard MagnetButton;
    // Cardboard 스크립트를 저장하는 변수

    private Vector3 ScreenCenter; 
    // 카메라의 중앙 지점을 저장하는 변수

    private float GageTimer;
    // 커서 게이지를 3초간 1까지 증가시키기 위한 변수

    string dialogPath = "scriptFile/";
    private string[] dataLine;
   // float showTime = 3.0f;

    Sprite[] characterImg;
    public Text ScTextUI;
    int scIndex = 0;

    // Use this for initialization
    void Start () {
		ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        // 카메라 화면의 높이/2 = x좌표, 넓이/2 y좌표 = 카메라 중앙 좌표 
        MagnetButton = GetComponent<Cardboard>();
        // MagnetButton 변수에 현재 오브젝트가 가지고 있는 Cardboard 스크립트를 불러와 저장한다
        ScTextUI = GameObject.Find("Line").GetComponent<Text>();

        InitDialogues();
    }

    public void InitDialogues()
    {
        TextAsset dialogFile = Resources.Load(dialogPath+ SceneManager.GetActiveScene().name) as TextAsset;
        dataLine = dialogFile.text.Split('\n');

        showLine();
    }

        // Update is called once per frame
        void Update () {
        Ray ray = Camera.main.ScreenPointToRay(ScreenCenter);
        //카메라 중앙 좌표부터 레이를 생성
        RaycastHit hit;
        //ray가 충돌한 지점의 정보를 저장하는 변수

        CursorGageImage.fillAmount = GageTimer;
        //커서 게이지 이미지의 fillAmount의 값은 GageTimer의 값과 같게 한다.

        if (Physics.Raycast(ray, out hit, 100.0f))
            //ray를 100.0f 거리까지 쏘아서 충돌 상태를 확인한다.
        {
            if (hit.collider.CompareTag("Box"))
                //hit에 맞은 오브젝트의 Tag가 Box일 경우에만
            {
                GageTimer += 1.0f / 3.0f * Time.deltaTime;
                // 3초 동안 GageTimer을 1로 증가시킨다.
                if (GageTimer >= 1 || MagnetButton.Triggered)
                    //GageTimer이 1이상 이거나 자석버튼을 작동시키면
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("GameGuide");
                    //Application.LoadLevel(1);
                    //GameGuide씬을 불러온다.
                    GageTimer = 0;
                    //입력을 완료했으니 GageTimer를 0으로 한다.
                }
            }

            if (hit.collider.CompareTag("option"))
            //hit에 맞은 오브젝트의 Tag가 option일 경우에만
            {
                GageTimer += 1.0f / 3.0f * Time.deltaTime;
                // 3초 동안 GageTimer을 1로 증가시킨다.
                if (GageTimer >= 1 || MagnetButton.Triggered)
                //GageTimer이 1이상 이거나 자석버튼을 작동시키면
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("Option");
                    //환경설정 씬을 불러온다.
                    GageTimer = 0;
                    //입력을 완료했으니 GageTimer를 0으로 한다.
                }
            }

            if (hit.collider.CompareTag("Object"))
                //hit에 맞은 오브젝트의 Tag가 Object일 경우에만
            {
                GageTimer += 1.0f / 3.0f * Time.deltaTime;
                // 3초 동안 GageTimer을 1로 증가시킨다.
                if (GageTimer >= 1 || MagnetButton.Triggered)
                //GageTimer이 1이상 이거나 자석버튼을 작동시키면
                {
                    TextUI.text = hit.collider.GetComponent<ObjectText>().text;
                    //TextUI의 text를 hit한 콜라이더를 가진 오브젝트의 Text 컴포넌트의 text로 변경한다.
                    GageTimer = 0;
                    //입력을 완료했으니 GageTimer를 0으로 한다.
                }
            }

            if (hit.collider.CompareTag("Script"))
            //hit에 맞은 오브젝트의 Tag가 Object일 경우에만
            {
               GageTimer += 1.0f / 3.0f * Time.deltaTime;
                // 2초 동안 GageTimer을 1로 증가시킨다.
                if (GageTimer >= 1 || MagnetButton.Triggered)
                //GageTimer이 1이상 이거나 자석버튼을 작동시키면
                {
                    if(scIndex < dataLine.Length-1)
                        showLine();
                    //TextUI의 text를 hit한 콜라이더를 가진 오브젝트의 Text 컴포넌트의 text로 변경한다.
                    GageTimer = 0;
                    //입력을 완료했으니 GageTimer를 0으로 한다.
                }
            }
        }
        else
            GageTimer = 0;
        //ray에 아무것도 충돌하지 않으면 GageTimer를 0으로 한다.
	}

    public void showLine()
    {
        Text name = GameObject.Find("CharName").GetComponent<Text>();
       // Text line = GameObject.Find("Line").GetComponent<Text>();

        string[] dataSplit = dataLine[scIndex++].Split(',');
        name.text = dataSplit[0];
       // line.text = dataSplit[1];
        ChangeImg(dataSplit[0]);

        ScTextUI.text = dataSplit[1];
    }

    void ChangeImg(string name)
    {
        GameObject.Find("CharImage").GetComponent<Image>().sprite = Resources.Load("charImg/" + name.Trim(), typeof(Sprite)) as Sprite;
    }


}
