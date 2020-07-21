using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Question : MonoBehaviour
{
    [SerializeField] float moveSpeed; //투사체 속도 변수
    Vector3 targetPos = new Vector3(); //목표물의 위치

    [SerializeField] ParticleSystem ps_Effect; //부딪힌 효과를 담을 변수

    public static bool isCollide = false; //InteractionController에서 참조함.

    public void SetTarget(Vector3 _target) {
        targetPos = _target;
    }//타겟의 위치를 설정할 함수

    // Update is called once per frame
    void Update()
    {
        if (targetPos != Vector3.zero) {
            if ((transform.position - targetPos).sqrMagnitude >= 0.1f)
            { //sqrMagnitude : 두 거리간의 거리차의 제곱값(거리차가 2라면 4리턴)
                transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed); //Lerp : 거리차를 n분의 1씩 좁혀나가는 방식 (0.5f경우, 2분의 1씩 거리를 좁혀나감)
            }
            else
            {
                ps_Effect.gameObject.SetActive(true);
                ps_Effect.transform.position = transform.position;//충돌체 위치 바꾸기-현재 자기 자신의 위치로 옮겨준다.
                ps_Effect.Play();
                isCollide = true;
                targetPos = Vector3.zero;//목적지에 도달해서 이제 필요없음
                this.gameObject.SetActive(false);//비활성화
            }//충돌함
            }
        }//목표물의 위치값을 알아냈을 경우
    }

