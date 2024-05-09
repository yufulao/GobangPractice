using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    private void Start()
    {
        GameController.Instance.OnInit();
    }

    private void Update()
    {
        GameController.Instance.OnUpdate();
    }
}
