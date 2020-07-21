using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    [SerializeField] float fadeSpeed;

    bool CheckSameSprite(SpriteRenderer p_SpriteRenderer, Sprite p_Sprite)
    {
        if (p_SpriteRenderer.sprite == p_Sprite)
            return true;
        else
            return false;
    }

    public IEnumerator SpriteChangeCoroutine(Transform p_Target, string p_SpriteName) 
    {
        SpriteRenderer t_SpriteRenderer = p_Target.GetComponentInChildren<SpriteRenderer>();

       // string path = "Characters/" + p_SpriteName;

        Sprite t_sprite = Resources.Load(p_SpriteName, typeof(Sprite)) as Sprite; // TODO: 경로에서 이미지 로드를 못해오는 듯     
        Debug.Log(t_sprite);

        if (!CheckSameSprite(t_SpriteRenderer, t_sprite)) {

            Color t_color = t_SpriteRenderer.color;
            t_color.a = 0;
            t_SpriteRenderer.color = t_color;
            t_SpriteRenderer.sprite = t_sprite;

            while (t_color.a < 1) {
                t_color.a += fadeSpeed;
                t_SpriteRenderer.color = t_color;
                yield return null;
                 
            }
        }
    }

}
