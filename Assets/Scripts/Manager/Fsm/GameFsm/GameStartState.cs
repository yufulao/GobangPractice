using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartState : IFsmState
{
    public void OnEnter()
    {
        GameController.Instance.StartGame();
    }

    public void OnUpdate()
    {
    }

    public void OnExit()
    {
    }
}
