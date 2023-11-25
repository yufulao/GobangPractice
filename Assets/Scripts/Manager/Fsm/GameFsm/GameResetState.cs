using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetState : IFsmState
{
    public void OnEnter()
    {
        GameController.Instance.ResetGame();
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }
}
