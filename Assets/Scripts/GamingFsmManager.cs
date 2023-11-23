using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamingFsmManager
{
    public GameController gameController;
    private IGameState _currentGameState;
    private Dictionary<GamingStateEnum, IGameState> _stateHolder = new Dictionary<GamingStateEnum, IGameState>();
    private GamingStateEnum _currentStateEnum;

    public GamingFsmManager(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void OnInit()
    {
        _stateHolder.Add(GamingStateEnum.GameStart,new GameStartState());
        _stateHolder.Add(GamingStateEnum.GameIniting,new GameInitState());
        _stateHolder.Add(GamingStateEnum.GamePlaying,new GamePlayingState());
        _stateHolder.Add(GamingStateEnum.GameEnd,new GameEndState());
        _stateHolder.Add(GamingStateEnum.GameReseting,new GameResetState());

        _currentGameState = null;
    }

    public void OnUpdate()
    {
        _currentGameState?.OnUpdate(this);
    }

    public void SetCurrentState(GamingStateEnum stateEnum)
    {
        _currentGameState?.OnClear(this);
        _currentStateEnum = stateEnum;
        _currentGameState = _stateHolder[_currentStateEnum];
        _currentGameState.OnInit(this);
    }

    public GamingStateEnum GetCurrentState()
    {
        return _currentStateEnum;
    }
}
