using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Resources;
using UnityEngine;


public class DialogueParser : MonoBehaviour
{
    public Dialogue[] Parse(string _CSVFileName)
    {
        List<Dialogue> dialogueList = new List<Dialogue>(); //대사 리스트 생성
        TextAsset csvData = Resources.Load<TextAsset>(_CSVFileName);    //csv 파일 가져옴

        string[] data = csvData.text.Split(new char[] { '\n' });    //엔터 기준으로 구분

         //UnityEngine.Debug.Log("확인");

        for (int i = 1; i < data.Length;)
        {
            
            //UnityEngine.Debug.Log(data[i]);
            
            string[] row = data[i].Split(new char[] { ',' });
           

            Dialogue dialogue = new Dialogue(); //대사 리스트 생성
            dialogue.name = row[1];
            
            //UnityEngine.Debug.Log(row[1]);
            List<string> contextList = new List<string>();
            List<string> spriteList = new List<string>();
            List<string> voiceList = new List<string>();

            do
            {
                contextList.Add(row[2]); //UnityEngine.Debug.Log(row[2]);
                spriteList.Add(row[3]); UnityEngine.Debug.Log("add "+row[3]);
                voiceList.Add(row[4]);

                if (++i < data.Length)
                {
                    row = data[i].Split(new char[] { ',' });
                }
                else
                {
                    break;
                }
            } while (row[0].ToString() == "");

            dialogue.contexts = contextList.ToArray();
            dialogue.spriteName = spriteList.ToArray();
            dialogue.VoiceName = voiceList.ToArray();
            dialogueList.Add(dialogue);
          
   
        }
       
        return dialogueList.ToArray();
    }

}
