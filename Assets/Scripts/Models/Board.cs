using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board
{
    [HideInInspector] public Vector2 boardSize;
    private int[,] _board; // 二维数组表示棋盘状态，-1表示空，0表示黑子，1表示白子
    private int _totalSites = 0; //一共多少个位，优化判断平局
    private int _currentTotalSet = 0; //当前一共下了多少个棋子

    private List<Transform> _boardTransformPoints = new List<Transform>();
    private Vector2 _chessSiteSize; //每个格子大小

    public void InitBoard(List<Transform> boardTransformPoints) //初始化棋盘
    {
        SetBoardParams(boardTransformPoints);
        ResetBoard();
    }

    public void ResetBoard()
    {
        //重置棋盘
        for (var i = 0; i < _board.GetLength(0); i++)
        {
            for (var j = 0; j < _board.GetLength(1); j++)
            {
                _board[i, j] = -1;
            }
        }

        _currentTotalSet = 0;
    }

    public void SetChess(Vector3 clickPoint, int player,Action<Vector2,Vector2> callback=null) //落子
    {
        if (!CheckClickPointValid(clickPoint))
        {
            return;
        }

        Vector2 xyPoint = ClickPositionTranslateToXyPoint(clickPoint);
        int x = (int)xyPoint.x;
        int y = (int)xyPoint.y;
        
        if (_board[x, y] != -1) //该位置有棋子
        {
            return;
        }
        
        Vector2 chessPosition = XyPointTranslateToChessPosition(xyPoint);
        SetChessData(x, y, player);
        callback?.Invoke(chessPosition,new Vector2(x,y));
    }

    private void SetChessData(int x, int y, int player)
    {
        _board[x, y] = player;
        _currentTotalSet++;
    }

    public int CheckReslut(int x, int y, int player) //落子后检测，返回值-1为继续，0为玩家0胜利，1同理，3为平局
    {
        if (CheckWin(x, y, player))
        {
            return player;
        }
        else if (CheckDraw())
        {
            return 3;
        }

        return -1;
    }

    private Vector2 ClickPositionTranslateToXyPoint(Vector3 clickPoint)
    {
        //根据格子大小计算鼠标点击位置对应的棋盘坐标
        return new Vector2(Mathf.FloorToInt((clickPoint.x - _boardTransformPoints[0].position.x) / _chessSiteSize.x),
            Mathf.FloorToInt((clickPoint.y - _boardTransformPoints[0].position.y) / _chessSiteSize.y));
    }

    private Vector2 XyPointTranslateToChessPosition(Vector2 xyPoint)
    {
        return new Vector2(
            xyPoint.x * _chessSiteSize.x + _boardTransformPoints[0].position.x + _chessSiteSize.x * 0.5f
            , xyPoint.y * _chessSiteSize.y + _boardTransformPoints[0].position.y + _chessSiteSize.y * 0.5f); //落子的位置
    }

    private bool CheckClickPointValid(Vector3 clickPoint) //判断落子是否越界
    {
        return clickPoint.x >= _boardTransformPoints[0].position.x
               && clickPoint.x <= _boardTransformPoints[1].position.x
               && clickPoint.y <= _boardTransformPoints[0].position.y
               && clickPoint.y >= _boardTransformPoints[1].position.y;
    }

    private void SetBoardParams(List<Transform> boardTransformPoints)
    {
        _boardTransformPoints = boardTransformPoints;
        boardSize = new Vector2(14, 14);
        _board = new int[14, 14];
        _totalSites = 14 * 14;
        SetChessSiteSize();
    }

    private void SetChessSiteSize()
    {
        _chessSiteSize = new Vector2(
            (_boardTransformPoints[1].position.x - _boardTransformPoints[0].position.x) / boardSize.x
            , (_boardTransformPoints[1].position.y - _boardTransformPoints[0].position.y) / boardSize.y);
    }


    private bool CheckDraw() //检测平局
    {
        return _currentTotalSet >= _totalSites;
    }

    private bool CheckWin(int x, int y, int player) //检测胜利
    {
        return CheckWinByX(x, y, player)
               || CheckWinByY(x, y, player)
               || CheckWinByLeftRow(x, y, player)
               || CheckWinByRightRow(x, y, player);
    }

    private bool CheckWinByX(int x, int y, int player) //水平检测，返回水平连续棋子个数
    {
        int currentCount = 1;
        currentCount = CheckWinByXRight(x, y, player, currentCount);
        currentCount = CheckWinByXLeft(x, y, player, currentCount);
        return currentCount >= 5;
    }

    private int CheckWinByXRight(int x, int y, int player, int currentCount) //水平向上检测
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i < boardSize.x && _board[x + i, y] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }

    private int CheckWinByXLeft(int x, int y, int player, int currentCount) //水平向下检测
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i >= 0 && _board[x - i, y] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }

    private bool CheckWinByY(int x, int y, int player) //垂直检测
    {
        int currentCount = 1;
        currentCount = CheckWinByYUp(x, y, player, currentCount);
        currentCount = CheckWinByYDown(x, y, player, currentCount);

        return currentCount >= 5;
    }

    private int CheckWinByYUp(int x, int y, int player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (y + i < boardSize.y && _board[x, y + i] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }

    private int CheckWinByYDown(int x, int y, int player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (y - i >= 0 && _board[x, y - i] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }

    private bool CheckWinByLeftRow(int x, int y, int player) //左斜检测
    {
        int currentCount = 1;
        currentCount = CheckWinByLeftRowUp(x, y, player, currentCount);
        currentCount = CheckWinByLeftRowDown(x, y, player, currentCount);


        return currentCount >= 5;
    }

    private int CheckWinByLeftRowUp(int x, int y, int player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i < boardSize.x && y + i < boardSize.y && _board[x + i, y + i] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }

    private int CheckWinByLeftRowDown(int x, int y, int player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i >= 0 && y - i >= 0 && _board[x - i, y - i] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }

    private bool CheckWinByRightRow(int x, int y, int player) //右斜检测
    {
        int currentCount = 1;
        currentCount = CheckWinByRightRowUp(x, y, player, currentCount);
        currentCount = CheckWinByRightRowDown(x, y, player, currentCount);

        return currentCount >= 5;
    }

    private int CheckWinByRightRowUp(int x, int y, int player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i >= 0 && y + i < boardSize.y && _board[x - i, y + i] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }

    private int CheckWinByRightRowDown(int x, int y, int player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i < boardSize.x && y - i >= 0 && _board[x + i, y - i] == player)
            {
                currentCount++;
            }
            else
            {
                return currentCount;
            }
        }

        return currentCount;
    }
}