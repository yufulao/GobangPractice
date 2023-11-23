using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    private Board _board;
    public BoardView view;
    private int _currentPlayer; //记录当前玩家

    public List<Transform> boardTransformPoints = new List<Transform>();
    private Vector2 _chessSiteSize; //每个格子大小

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnMouse0Down(Input.mousePosition);
        }
    }

    private void Start()
    {
        StartGame(new Board(), view);
    }

    private void StartGame(Board boardT, BoardView viewT)
    {
        _board = boardT;
        this.view = viewT;
        
        _board.InitBoard();
        this.view.ResetBoardView();
        
        _chessSiteSize = new Vector2(
            (boardTransformPoints[1].position.x - boardTransformPoints[0].position.x) / _board.boardSize.x
            , (boardTransformPoints[1].position.y - boardTransformPoints[0].position.y) / _board.boardSize.y);
        //Debug.Log(_chessSiteSize);
        
        _currentPlayer = 1; //玩家1先手
    }

    private void ResetGame()
    {
        _board.ResetBoard();
    }

    private void OnMouse0Down(Vector3 clickPoint)
    {
        //Debug.Log(clickPoint);
        //Debug.Log(boardTransformPoints[0].position+"   "+boardTransformPoints[1].position);
        //根据格子大小计算鼠标点击位置对应的棋盘坐标
        int x = Mathf.FloorToInt((clickPoint.x-boardTransformPoints[0].position.x) / _chessSiteSize.x);
        int y = Mathf.FloorToInt((clickPoint.y-boardTransformPoints[0].position.y) / _chessSiteSize.y);
        Vector2 chessPosition = new Vector2(x * _chessSiteSize.x+boardTransformPoints[0].position.x+_chessSiteSize.x*0.5f
            , y * _chessSiteSize.y+boardTransformPoints[0].position.y+_chessSiteSize.y*0.5f); //落子的位置
        //Debug.Log(clickPoint.x+"   "+boardTransformPoints[0].position.x+"   "+_chessSiteSize.x);
        //Debug.Log(new Vector2(x,y)+"   "+chessPosition);
        
        if (_board.SetChess(x, y, _currentPlayer)) //检测是否成功落子
        {
            view.UpdateBoard(chessPosition, _currentPlayer); //如果成功落子，更新视图显示
            CheckChessReslt(x, y); //检查结果
        }
    }

    private void CheckChessReslt(int x, int y) //检查结果
    {
        int result = _board.CheckReslut(x, y, _currentPlayer);
        Debug.Log(result);
        if (result == -1) //游戏继续
        {
            //切换玩家
            _currentPlayer = _currentPlayer == 0 ? 1 : 0;
        }
        else //游戏结束
        {
            view.ShowWinMessage(result);
        }
    }
}