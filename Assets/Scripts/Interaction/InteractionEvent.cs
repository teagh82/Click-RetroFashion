using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : MonoBehaviour
{
    [SerializeField] bool isAutoEvent = false; //TRUE이면 함수 Update 에서 자동으로 호출되게 만들것이다. 

    [SerializeField] DialogueEvent[] dialogueEvent;
    int currentCount;

    /*없는데 추가함 (기현)
    void Start(){
        bool t_Flag = CheckEvent();
        gameObject.SetActive(t_Flag);
    }
    */

    public Dialogue[] GetDialogue()
    {
        if(DatabaseManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventEndNum]){
            return null;
        }

        //DatabaseManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventNum] = true;

        if (!DatabaseManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventNum] || dialogueEvent[currentCount].isSame)
        {//상호작용 전 대화
            DatabaseManager.instance.eventFlags[dialogueEvent[currentCount].eventTiming.eventNum] = true;
            dialogueEvent[currentCount].dialogues = SettingDialogues(dialogueEvent[currentCount].dialogues, (int)dialogueEvent[currentCount].line.x, (int)dialogueEvent[currentCount].line.y);
            return dialogueEvent[currentCount].dialogues;
        }
        else { //상호작용 후 대화
            dialogueEvent[currentCount].dialogues = SettingDialogues(dialogueEvent[currentCount].dialogues, (int)dialogueEvent[currentCount].lineB.x, (int)dialogueEvent[currentCount].lineB.y);
            return dialogueEvent[currentCount].dialogues;
        }
           

        return dialogueEvent[currentCount].dialogues;
    }

    Dialogue[] SettingDialogues(Dialogue[] p_Dialog, int p_lineX, int p_lineY) {
      
        Dialogue[] t_dialogues = DatabaseManager.instance.GetDialogue((int)dialogueEvent[currentCount].line.x, (int)dialogueEvent[currentCount].line.y); ;
        
        for (int i = 0; i < dialogueEvent[currentCount].dialogues.Length; i++)
        {
            t_dialogues[i].tf_Target = dialogueEvent[currentCount].dialogues[i].tf_Target;
            t_dialogues[i].cameraType = dialogueEvent[currentCount].dialogues[i].cameraType;
        }

        return t_dialogues;
    }

    public int GetEventNumber()
    {
        CheckEvent();
        return dialogueEvent[currentCount].eventTiming.eventNum;
    }



    public AppearType GetAppearType()
    {
        return dialogueEvent[currentCount].appearType;
    }

    public GameObject[] GetTargets()
    {
        return dialogueEvent[currentCount].go_Targets;
    }

    public GameObject GetNextEvent(){ //다음 이벤트에 대한 정보를 받게된다.
        return dialogueEvent[currentCount].go_NextEvent;

    }

    private void Start()
    {
        bool t_Flag = CheckEvent();
        gameObject.SetActive(t_Flag);
    }

    bool CheckEvent() {

        bool t_Flag = true;

        for(int x = 0; x < dialogueEvent.Length; x++){
            t_Flag = true;

            //등장 조건 일치X, 등장 안시킴
            for (int i = 0; i < dialogueEvent[x].eventTiming.eventConditions.Length; i++) {
                if (DatabaseManager.instance.eventFlags[dialogueEvent[x].eventTiming.eventConditions[i]] != dialogueEvent[x].eventTiming.conditionFlag)
                    t_Flag = false;
                break;
            }

            //등장조건 관계X, 퇴장 조건과 일치 -> 무조건 등장X 
            if (DatabaseManager.instance.eventFlags[dialogueEvent[x].eventTiming.eventEndNum])
                t_Flag = false;

            if(t_Flag){
                currentCount = x;
                break;
            }
        }
            
        return t_Flag;
    }

    

    void Update() {
        if (isAutoEvent && DatabaseManager.isFinish && TransferManager.isFinished) {
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
