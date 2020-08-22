using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	// Use this for initialization
	void Start () {
		ScreenCenter = new Vector3(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        // 카메라 화면의 높이/2 = x좌표, 넓이/2 y좌표 = 카메라 중앙 좌표 
        MagnetButton = GetComponent<Cardboard>();
        // MagnetButton 변수에 현재 오브젝트가 가지고 있는 Cardboard 스크립트를 불러와 저장한다
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
                    Application.LoadLevel(1);
                    //1번 Scene을 불러온다.
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
        }
        else
            GageTimer = 0;
        //ray에 아무것도 충돌하지 않으면 GageTimer를 0으로 한다.
	}
}
