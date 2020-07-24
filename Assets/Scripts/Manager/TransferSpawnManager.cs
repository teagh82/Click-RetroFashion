using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

[System.Serializable]
public class Location
{
    public string name;
    public Transform tf_Spawn;
}


public class TransferSpawnManager : MonoBehaviour
{
    [SerializeField] Location[] locations;
    Dictionary<string, Transform> locationDic = new Dictionary<string, Transform>();

    public static bool spawnTiming = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < locations.Length; i++)
        {
            locationDic.Add(locations[i].name, locations[i].tf_Spawn);
        }

        if (spawnTiming)
        {
            TransferManager theTM = FindObjectOfType<TransferManager>();
            string t_LocationName = theTM.GetLocationName();
            Transform t_Spawn = locationDic[t_LocationName];
            //PlayerController.instance.transform.position = t_Spawn.position;

            spawnTiming = false;

            StartCoroutine(theTM.Done());
        }
    }


}