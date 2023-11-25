using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayingState : IFsmState
{
    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        GameController.Instance.UpdateGame();
    }

    public void OnExit()
    {
    }

    
}
