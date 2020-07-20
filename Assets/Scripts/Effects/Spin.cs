﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] float spinSpeed;
    [SerializeField] Vector3 spinDir;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(spinDir * spinSpeed * Time.deltaTime);
    }
   
}
