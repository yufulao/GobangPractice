using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitState : IGameState
{
    public void OnInit(GamingFsmManager fsmManager)
    {
        fsmManager.gameController.InitGame();
        fsmManager.SetCurrentState(GamingStateEnum.GameStart);
    }
}
