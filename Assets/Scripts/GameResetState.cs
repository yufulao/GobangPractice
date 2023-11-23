using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetState : IGameState
{
    public void OnInit(GamingFsmManager fsmManager)
    {
         fsmManager.gameController.ResetGame();
         fsmManager.SetCurrentState(GamingStateEnum.GameStart);
    }
}
