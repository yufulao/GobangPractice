using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : BaseMonoSingleTon<GameController>
{
    private Board _board;
    public BoardView view;
    [SerializeField]private List<Player> _players;
    public List<Transform> boardTransformPoints = new List<Transform>(); //棋盘两个角
    private Player _currentPlayer; //记录当前玩家

    private GamingFsmManager _gamingFsmManager;

    private void Start()
    {
        EnterGame();
    }

    private void Update()
    {
        _gamingFsmManager.OnUpdate();
    }
    
    /// <summary>
    /// 初始化游戏
    /// </summary>
    public void InitGame()
    {
        _board = new Board();
        _board.InitBoard(boardTransformPoints);
        view.ResetBoardView();
        _gamingFsmManager.ChangeFsmState(FsmStateEnum.GameStartState);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        _currentPlayer = _players[1];//设置玩家1先手
        _gamingFsmManager.ChangeFsmState(FsmStateEnum.GamePlayingState);
    }


    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void ResetGame()
    {
        _board.ResetBoard();
        view.ResetBoardView();
        _gamingFsmManager.ChangeFsmState(FsmStateEnum.GameStartState);
    }

    /// <summary>
    /// 游戏过程中update
    /// </summary>
    public void UpdateGame()
    {
        InputUpdate();
    }

    /// <summary>
    /// 重新开始游戏的摁钮绑定事件
    /// </summary>
    public void ResetGameBtn()
    {
        _gamingFsmManager.ChangeFsmState(FsmStateEnum.GameResetState);
    }
    
    /// <summary>
    /// 程序入口
    /// </summary>
    private void EnterGame()
    {
        SetGamingFsm();
    }

    /// <summary>
    /// 设置gamingFsm状态机
    /// </summary>
    private void SetGamingFsm()
    {
        _gamingFsmManager = FsmManager.Instance.GetFsmByName<GamingFsmManager>("GamingFsmManager") as GamingFsmManager;
        Dictionary<FsmStateEnum, IFsmState> states = new Dictionary<FsmStateEnum, IFsmState>();
        states.Add(FsmStateEnum.GameInitState,new GameInitState());
        states.Add(FsmStateEnum.GameStartState,new GameStartState());
        states.Add(FsmStateEnum.GamePlayingState,new GamePlayingState());
        states.Add(FsmStateEnum.GameEndState,new GameEndState());
        states.Add(FsmStateEnum.GameResetState,new GameResetState());
        _gamingFsmManager.SetFsm(states);
        //开始游戏流程
        _gamingFsmManager.ChangeFsmState(FsmStateEnum.GameInitState);
    }

    /// <summary>
    /// Update检测输入
    /// </summary>
    private void InputUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouse0Down();
        }
    }
    
    /// <summary>
    /// 鼠标左键摁下事件
    /// </summary>
    private void OnMouse0Down()
    {
        //Debug.Log("OnMouse0Down");
        //检测是否成功落子
        _board.SetChess(Input.mousePosition,_currentPlayer, (chessPosition, xyPoint) =>
        {
            view.UpdateBoard(chessPosition,_currentPlayer);
            CheckChessResult(xyPoint);
        });
    }
    
    /// <summary>
    /// 检测下棋结果
    /// </summary>
    /// <param name="xyPoint">棋子在board中的索引坐标</param>
    private void CheckChessResult(Vector2 xyPoint)
    {
        int x = (int)xyPoint.x;
        int y = (int)xyPoint.y;
        SetChessResultEnum result = _board.CheckReslut(x, y,_currentPlayer);
        switch (result)
        {
            case SetChessResultEnum.Continue:
                //切换玩家
                _currentPlayer = _currentPlayer.id == 0 ? _players[1] : _players[0];
                break;
            case SetChessResultEnum.GameDraw:
                view.ShowDrawMessage();
                break;
            case SetChessResultEnum.GameWin:
                view.ShowWinMessage(_currentPlayer);
                _gamingFsmManager.ChangeFsmState(FsmStateEnum.GameEndState);
                break;
        }
    }

    
    
}