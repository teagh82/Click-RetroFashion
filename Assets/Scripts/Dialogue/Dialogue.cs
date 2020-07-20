using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraType
{
    ObjectFront,
    Reset,
    FadeOut,
    FadeIn,
    FlashOut,
    FlashIn,
    ShowCutScene,
    HideCutScene,
}

[System.Serializable]
public class Dialogue
{
    [Header("카메라가 타겟팅할 대상")]
    public CameraType cameraType;
    public Transform tf_Target;

    [HideInInspector]
    public string name;

    [HideInInspector]
    public string[] contexts;

    [HideInInspector]
    public string[] spriteName;

    [HideInInspector]
    public string[] VoiceName;
}

[System.Serializable]
public class DialogueEvent
{
    public string name; //어떤 이벤트인지 알도록. 기능적의미X

    public Vector2 line;    //대사를 추출해서 꺼내옴
    public Dialogue[] dialogues;
}
