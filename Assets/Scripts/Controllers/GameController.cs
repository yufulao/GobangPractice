using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    private Board _board;
    public BoardView view;
    [SerializeField]private List<Player> _players;
    [SerializeField] private List<Transform> _boardTransformPoints = new List<Transform>(); //棋盘两个角
    private Player currentPlayer; //记录当前玩家

    private GamingFsmManager _gamingFsmManager;

    private void Start()
    {
        StartGame();
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
        _board.InitBoard(_boardTransformPoints);
        this.view.ResetBoardView();
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void ResetGame()
    {
        _board.ResetBoard();
        view.ResetBoardView();
    }

    /// <summary>
    /// 设置当前玩家
    /// </summary>
    /// <param name="currentPlayerIndex">当前玩家id</param>
    public void SetCurrentPlayer(int currentPlayerIndex)
    {
        currentPlayer = _players[currentPlayerIndex]; //玩家1先手
    }

    /// <summary>
    /// Update检测输入
    /// </summary>
    public void InputUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouse0Down();
        }
    }

    /// <summary>
    /// 重新开始游戏的摁钮绑定事件
    /// </summary>
    public void ResetGameBtn()
    {
        _gamingFsmManager.SetCurrentState(GamingStateEnum.GameReseting);
    }

    /// <summary>
    /// 程序入口
    /// </summary>
    private void StartGame()
    {
        _gamingFsmManager = new GamingFsmManager(this);
        _gamingFsmManager.OnInit();
        //开始游戏流程
        _gamingFsmManager.SetCurrentState(GamingStateEnum.GameIniting);
    }
    
    /// <summary>
    /// 鼠标左键摁下事件
    /// </summary>
    private void OnMouse0Down()
    {
        //Debug.Log("OnMouse0Down");
        //检测是否成功落子
        _board.SetChess(Input.mousePosition, currentPlayer, (chessPosition, xyPoint) =>
        {
            view.UpdateBoard(chessPosition, currentPlayer);
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
        SetChessResultEnum result = _board.CheckReslut(x, y, currentPlayer);
        switch (result)
        {
            case SetChessResultEnum.Continue:
                //切换玩家
                currentPlayer = currentPlayer.id == 0 ? _players[1] : _players[0];
                break;
            case SetChessResultEnum.GameDraw:
                view.ShowDrawMessage();
                break;
            case SetChessResultEnum.GameWin:
                view.ShowWinMessage(currentPlayer);
                _gamingFsmManager.SetCurrentState(GamingStateEnum.GameEnd);
                break;
        }
    }
}