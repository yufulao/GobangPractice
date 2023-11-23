using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartState : IGameState
{
    public void OnInit(GamingFsmManager fsmManager)
    {
        fsmManager.gameController.SetCurrentPlayer(1);
        
        fsmManager.SetCurrentState(GamingStateEnum.GamePlaying);
    }
}
