using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] bool isAutoEvent = false; //TRUE이면 함수 Update 에서 자동으로 호출되게 만들것이다. 

    [SerializeField] DialogueEvent dialogueEvent;
    public Dialogue[] GetDialogue()
    {
        DialogueEvent t_DialogueEvent = new DialogueEvent();
        t_DialogueEvent.dialogues = DatabaseManager.instance.GetDialogue((int)dialogueEvent.line.x, (int)dialogueEvent.line.y);
        for(int i = 0; i < dialogueEvent.dialogues.Length; i++)
        {
            t_DialogueEvent.dialogues[i].tf_Target = dialogueEvent.dialogues[i].tf_Target;
            t_DialogueEvent.dialogues[i].cameraType = dialogueEvent.dialogues[i].cameraType;
        }

        dialogueEvent.dialogues = t_DialogueEvent.dialogues;

        return dialogueEvent.dialogues;
    }

    public AppearType GetAppearType()
    {
        return dialogueEvent.appearType;
    }

    public GameObject[] GetTargets()
    {
        return dialogueEvent.go_Targets;
    }

    public GameObject GetNextEvent(){ //다음 이벤트에 대한 정보를 받게된다.
        return dialogueEvent.go_NextEvent;

    }

    void Update() {
        if (isAutoEvent && DatabaseManager.isFinish) {
            DialogueManager theDM = FindObjectOfType<DialogueManager>();
            if (GetAppearType() == AppearType.Appear) theDM.SetAppearObjects(GetTargets());
            else if (GetAppearType() == AppearType.Disappear) theDM.SetDisappearObjects(GetTargets());
            //캐릭터 등장 , 없애기 - InteractionController에서 복붙함.
            DialogueManager.isWaiting = true;
            theDM.SetNextEvent(GetNextEvent());
            theDM.ShowDialogue(GetDialogue());
            
            gameObject.SetActive(false);
        }
    }
}
