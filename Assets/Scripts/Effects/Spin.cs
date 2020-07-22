﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] Vector3 spinDir;

    public static bool isFinished = true;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinDir * spinSpeed * Time.deltaTime);
    }

    public IEnumerator SetAppearOrDisappear(bool p_Flag)
    {
        SpriteRenderer[] t_SpriteRenderer = GetComponentsInChildren<SpriteRenderer>();


        Color t_FrontColor = t_SpriteRenderer[0].color;
        Color t_RearColor = t_SpriteRenderer[1].color;

        if (p_Flag)
        {
            t_FrontColor.a = 0;  t_RearColor.a = 0;
            t_SpriteRenderer[0].color = t_FrontColor; t_SpriteRenderer[1].color = t_RearColor; 
        }

        float t_FadeSpeed = (p_Flag == true) ? 0.01f : -0.01f;

        yield return new WaitForSeconds(0.3f);

        while (true)
        {
            if (p_Flag && t_FrontColor.a >= 1) break;
            else if (!p_Flag && t_FrontColor.a <= 0) break;

            t_FrontColor.a += t_FadeSpeed; t_RearColor.a += t_FadeSpeed;
            t_SpriteRenderer[0].color = t_FrontColor; t_SpriteRenderer[1].color = t_RearColor;
            yield return null;
        }

        isFinished = true;
        gameObject.SetActive(p_Flag);
    }
   
}
