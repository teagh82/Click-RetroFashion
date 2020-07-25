using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform tf_Crosshair;
 // public static Transform instance;

    // Update is called once per frame

    private void Awake()
    {
      DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        CrosshairMoving();
    }


    void CrosshairMoving()
    {
        tf_Crosshair.localPosition = new Vector2(Input.mousePosition.x - (Screen.width / 2),
                                                 Input.mousePosition.y - (Screen.height / 2));
        //float t_cursorPosX = tf_Crosshair.localPosition.x;
        //float t_cursorPosY = tf_Crosshair.localPosition.y;

        //t_cursorPosX = Mathf.Clamp(t_cursorPosX, (-Screen.width / 2 + 3), (Screen.width / 2 - 3));
        //t_cursorPosY = Mathf.Clamp(t_cursorPosY, (-Screen.height / 2 + 3), (Screen.height / 2 - 3));

        //tf_Crosshair.localPosition = new Vector2(t_cursorPosX, t_cursorPosY);
    }
}

