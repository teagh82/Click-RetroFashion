﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 originPos;
    Quaternion originRot;
    Coroutine coroutine;

    InteractionController theIC;
    void Start()
    {
        theIC = FindObjectOfType<InteractionController>();
    }

    public void CamOriginSetting()
    {
        originPos = transform.position;
        originRot = Quaternion.Euler(0, 0, 0);
    }
    
    public void CameraTargetting(Transform p_Target, float p_CamSpeed = 0.1f, bool p_isReset = false, bool p_isFinish = false)
    {
        
        if (!p_isReset)
        {
            if (p_Target != null) {
                StopAllCoroutines();
                coroutine = StartCoroutine(CameraTargettingCoroutine(p_Target, p_CamSpeed));
            }   
                         
        }
        else
        {
            if (coroutine != null)
                StopCoroutine(coroutine);

            StartCoroutine(CameraResetCoroutine(p_CamSpeed, p_isFinish));
        }    
      
    }

    IEnumerator CameraTargettingCoroutine(Transform p_Target, float p_CamSpeed = 0.1f)
    {
        Vector3 t_TargetPos = p_Target.position;
        Vector3 t_TargetFrontPos = t_TargetPos + (p_Target.forward * 1.3f); //카메라 너무확대되어 보일때 *로 조절
        Vector3 t_Direction = (t_TargetPos - t_TargetFrontPos).normalized;

        while(transform.position != t_TargetFrontPos || Quaternion.Angle(transform.rotation, Quaternion.LookRotation(t_Direction)) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, t_TargetFrontPos, p_CamSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(t_Direction), p_CamSpeed);
            yield return null;
        }
    }
    IEnumerator CameraResetCoroutine(float p_CamSpeed = 0.1f, bool p_isFinish = false)
    {
        yield return new WaitForSeconds(0.5f);

        while (transform.position != originPos || Quaternion.Angle(transform.rotation, originRot) >= 0.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, originPos, p_CamSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, originRot, p_CamSpeed);
            yield return null;
        }
        transform.position = originPos;

        if (p_isFinish)
        {
            //모든 대화가 끝났으면 리셋.
            theIC.SettingUI(true);
        }
    }
}
