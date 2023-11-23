using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    private Board _board;
    public BoardView view;
    [SerializeField] private List<Transform> _boardTransformPoints = new List<Transform>(); //棋盘两个角
    [SerializeField] private int currentPlayer; //记录当前玩家

    private GamingFsmManager _gamingFsmManager;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        _gamingFsmManager.OnUpdate();
    }

    private void StartGame()
    {
        _gamingFsmManager = new GamingFsmManager(this);
        _gamingFsmManager.OnInit();
        //开始游戏流程
        _gamingFsmManager.SetCurrentState(GamingStateEnum.GameIniting);
    }

    public void InitGame()
    {
        _board = new Board();
        _board.InitBoard(_boardTransformPoints);
        this.view.ResetBoardView();
    }

    public void ResetGame()
    {
        _board.ResetBoard();
        view.ResetBoardView();
    }

    public void SetCurrentPlayer(int currentPlayerT)
    {
        currentPlayer = currentPlayerT; //玩家1先手
    }

    public void InputUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouse0Down();
        }
    }

    public void ResetGameBtn()
    {
        _gamingFsmManager.SetCurrentState(GamingStateEnum.GameReseting);
    }

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

    private void CheckChessResult(Vector2 xyPoint) //检查结果
    {
        int x = (int)xyPoint.x;
        int y = (int)xyPoint.y;
        int result = _board.CheckReslut(x, y, currentPlayer);
        //Debug.Log(result);
        if (result == -1) //游戏继续
        {
            //切换玩家
            currentPlayer = currentPlayer == 0 ? 1 : 0;
        }
        else //游戏结束
        {
            view.ShowWinMessage(result);
            _gamingFsmManager.SetCurrentState(GamingStateEnum.GameEnd);
        }
    }
}