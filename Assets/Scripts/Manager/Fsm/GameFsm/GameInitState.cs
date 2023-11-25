using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitState : IFsmState
{
    public void OnEnter()
    {
        GameController.Instance.InitGame();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
    
    

}
