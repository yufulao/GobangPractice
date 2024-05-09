using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : BaseSingleTon<GameController>
{
    private GameModel _model;
    private GameView _view;
    private GamingFsmManager _fsm;

    public void OnUpdate()
    {
        _fsm?.OnUpdate();
    }
    
    /// <summary>
    /// 程序入口
    /// </summary>
    public void OnInit()
    {
        InitGamingFsm();
        //开始游戏流程
        _fsm.ChangeFsmState(FsmStateEnum.GameInitState);
    }

    /// <summary>
    /// 初始化游戏状态的Enter
    /// </summary>
    public void GameInitStateEnter()
    {
        _model = new GameModel();
        _view = GameObject.Find("GameView").GetComponent<GameView>();
        _model.Init(_view.boardTransformPoints);
        _view.ResetBoardView();
        //绑定view摁钮事件
        _view.btnReset.onClick.AddListener(ResetGame);
        
        _fsm.ChangeFsmState(FsmStateEnum.GamePlayingState); //开始游戏
    }
    
    /// <summary>
    /// 游戏过程转态中update
    /// </summary>
    public void GamePlayingStateUpdate()
    {
        //检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("OnMouse0Down");
            //检测是否成功落子
            _model.SetChess(Input.mousePosition, (chessPosition, xyPoint) =>
            {
                _view.UpdateBoard(chessPosition, _model.GetCurrentPlayer());
                CheckChessResult(xyPoint);
            });
        }
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    private void ResetGame()
    {
        _model.ResetBoard();
        _view.ResetBoardView();
        _fsm.ChangeFsmState(FsmStateEnum.GamePlayingState);
    }

    /// <summary>
    /// 设置gamingFsm状态机
    /// </summary>
    private void InitGamingFsm()
    {
        _fsm = FsmManager.Instance.GetFsmByName<GamingFsmManager>("GamingFsmManager") as GamingFsmManager;
        Dictionary<FsmStateEnum, IFsmState> states = new Dictionary<FsmStateEnum, IFsmState>
        {
            {FsmStateEnum.GameInitState, new GameInitState()},
            {FsmStateEnum.GamePlayingState, new GamePlayingState()},
            {FsmStateEnum.GameEndState, new GameEndState()},
        };
        if (_fsm == null)
        {
            Debug.Log("状态机为空");
            return;
        }

        _fsm.SetFsm(states);
    }

    /// <summary>
    /// 检测下棋结果
    /// </summary>
    /// <param name="xyPoint">棋子在board中的索引坐标</param>
    private void CheckChessResult(Vector2 xyPoint)
    {
        int x = (int) xyPoint.x;
        int y = (int) xyPoint.y;
        DefineChessResultEnum result = _model.CheckResult(x, y);
        switch (result)
        {
            case DefineChessResultEnum.Continue:
                _model.SwitchCurrentPlayer(); //切换玩家
                break;
            case DefineChessResultEnum.GameDraw:
                _view.ShowDrawMessage();
                _fsm.ChangeFsmState(FsmStateEnum.GameEndState);
                break;
            case DefineChessResultEnum.GameWin:
                _view.ShowWinMessage(_model.GetCurrentPlayer());
                _fsm.ChangeFsmState(FsmStateEnum.GameEndState);
                break;
        }
    }
}