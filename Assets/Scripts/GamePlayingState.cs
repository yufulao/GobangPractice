using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayingState : IGameState
{
    public void OnUpdate(GamingFsmManager fsmManager)
    {
        fsmManager.gameController.InputUpdate();
    }
}
