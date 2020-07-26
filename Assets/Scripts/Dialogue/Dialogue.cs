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
    AppearSlideCG,
    DisappearSlideCG,
    ChangeSlideCG,
}

public enum AppearType
{
    None,
    Appear,
    Disappear,
}


[System.Serializable]
public class EventTiming {
    public int eventNum;
    public int[] eventConditions;
    public bool conditionFlag;
    public int eventEndNum;
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
    public EventTiming eventTiming;

    public Vector2 line;    //대사를 추출해서 꺼내옴
    public Dialogue[] dialogues;

    [Space]
    public Vector2 lineB;
    public Dialogue dialoguesB;

    [Space]
    public AppearType appearType;
    public GameObject[] go_Targets;
    [Space]
    public GameObject go_NextEvent;

    public bool isSame;

}
