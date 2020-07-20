using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disappear : MonoBehaviour
{
    [SerializeField] float disappearTime; //언제 사라질지 설정하는 변수
    
    void onEnable() //Disappear 객체가 활성화 될때마다 호출되는 함수
    {
        StartCoroutine(DiappearCoroutine());//활성화 될때마다 Coroutine 실행
    }

    IEnumerator DiappearCoroutine() {
        yield return new WaitForSeconds(disappearTime);// 몇 초 동안 대기시키기 
        gameObject.SetActive(false);
    } //병렬처리 
}//반복적으로 Coroutine()를 실행하고 사라지게 만든다.
