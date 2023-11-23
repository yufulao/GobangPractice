using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board
{
    [HideInInspector] public Vector2 boardSize;
    private Chess[,] _board; // 二维数组表示棋盘状态，-1表示空，0表示黑子，1表示白子
    private int _totalSites = 0; //一共多少个位，优化判断平局
    private int _currentTotalSet = 0; //当前一共下了多少个棋子

    private List<Transform> _boardTransformPoints = new List<Transform>();
    private Vector2 _chessSiteSize; //每个格子大小

    /// <summary>
    /// 初始化棋盘
    /// </summary>
    /// <param name="boardTransformPoints">棋盘的左上角和右下角点位</param>
    public void InitBoard(List<Transform> boardTransformPoints) //初始化棋盘
    {
        SetBoardParams(boardTransformPoints);
        ResetBoard();
    }

    /// <summary>
    /// 重置棋盘
    /// </summary>
    public void ResetBoard()
    {
        //重置棋盘
        for (var i = 0; i < _board.GetLength(0); i++)
        {
            for (var j = 0; j < _board.GetLength(1); j++)
            {
                _board[i, j] = null;
            }
        }

        _currentTotalSet = 0;
    }

    /// <summary>
    /// 落子事件
    /// </summary>
    /// <param name="worldPoint">点击的世界坐标</param>
    /// <param name="player">当前玩家</param>
    /// <param name="callback">如果成功下棋的回调</param>
    public void SetChess(Vector3 worldPoint, Player player, Action<Vector2, Vector2> callback = null)
    {
        Vector3 clickPoint = Camera.main.ScreenToWorldPoint(worldPoint);

        if (!CheckClickPointValid(clickPoint))
        {
            return;
        }

        Vector2 xyPoint = ClickPositionTranslateToXyPoint(clickPoint);
        int x = (int)xyPoint.x;
        int y = (int)xyPoint.y;

        if (_board[x, y] != null) //该位置有棋子
        {
            return;
        }

        Vector2 chessPosition = XyPointTranslateToChessPosition(xyPoint);
        SetChessData(x, y, player);
        callback?.Invoke(chessPosition, new Vector2(x, y));
    }

    /// <summary>
    /// 检测落子后的结果
    /// </summary>
    /// <param name="x">落子坐标x</param>
    /// <param name="y">落子坐标y</param>
    /// <param name="player">当前玩家</param>
    /// <returns>落子后的结果</returns>
    public SetChessResultEnum CheckReslut(int x, int y, Player player) //落子后检测，返回值-1为继续，0为玩家0胜利，1同理，3为平局
    {
        if (CheckWin(x, y, player))
        {
            return SetChessResultEnum.GameWin;
        }

        if (CheckDraw())
        {
            return SetChessResultEnum.GameDraw;
        }

        return SetChessResultEnum.Continue;
    }

    /// <summary>
    /// 设置落子数据
    /// </summary>
    /// <param name="x">落子坐标x</param>
    /// <param name="y">落子坐标y</param>
    /// <param name="player">当前玩家</param>
    private void SetChessData(int x, int y, Player player)
    {
        _board[x, y] = new Chess(player.id);
        _currentTotalSet++;
    }

    /// <summary>
    /// 点击的世界坐标转为棋盘索引坐标
    /// </summary>
    /// <param name="clickPoint">点击的世界坐标</param>
    /// <returns></returns>
    private Vector2 ClickPositionTranslateToXyPoint(Vector3 clickPoint)
    {
        //根据格子大小计算鼠标点击位置对应的棋盘坐标
        return new Vector2(Mathf.FloorToInt((clickPoint.x - _boardTransformPoints[0].position.x) / _chessSiteSize.x),
            Mathf.FloorToInt((clickPoint.y - _boardTransformPoints[0].position.y) / _chessSiteSize.y));
    }

    /// <summary>
    /// 棋盘索引坐标转为棋子落点的位置
    /// </summary>
    /// <param name="xyPoint">棋子的棋盘索引坐标</param>
    /// <returns></returns>
    private Vector2 XyPointTranslateToChessPosition(Vector2 xyPoint)
    {
        return new Vector2(
            xyPoint.x * _chessSiteSize.x + _boardTransformPoints[0].position.x + _chessSiteSize.x * 0.5f
            , xyPoint.y * _chessSiteSize.y + _boardTransformPoints[0].position.y + _chessSiteSize.y * 0.5f); //落子的位置
    }

    /// <summary>
    /// 检测棋子落点是否在board里
    /// </summary>
    /// <param name="clickPoint">点击的世界坐标</param>
    /// <returns></returns>
    private bool CheckClickPointValid(Vector3 clickPoint) //判断落子是否越界
    {
        return clickPoint.x >= _boardTransformPoints[0].position.x
               && clickPoint.x <= _boardTransformPoints[1].position.x
               && clickPoint.y <= _boardTransformPoints[0].position.y
               && clickPoint.y >= _boardTransformPoints[1].position.y;
    }

    /// <summary>
    /// 设置棋盘参数
    /// </summary>
    /// <param name="boardTransformPoints">棋盘的左上角和右下角点位</param>
    private void SetBoardParams(List<Transform> boardTransformPoints)
    {
        _boardTransformPoints = boardTransformPoints;
        boardSize = new Vector2(14, 14);
        _board = new Chess[14, 14];
        _totalSites = 14 * 14;
        SetChessSiteSize();
    }

    /// <summary>
    /// 赋值格子大小
    /// </summary>
    private void SetChessSiteSize()
    {
        _chessSiteSize = new Vector2(
            (_boardTransformPoints[1].position.x - _boardTransformPoints[0].position.x) / boardSize.x
            , (_boardTransformPoints[1].position.y - _boardTransformPoints[0].position.y) / boardSize.y);
    }

    /// <summary>
    /// 检测平局
    /// </summary>
    /// <returns></returns>
    private bool CheckDraw()
    {
        return _currentTotalSet >= _totalSites;
    }

    /// <summary>
    /// 检测胜利
    /// </summary>
    /// <param name="x">棋盘索引坐标x</param>
    /// <param name="y">棋盘索引坐标y</param>
    /// <param name="player">当前玩家</param>
    /// <returns></returns>
    private bool CheckWin(int x, int y, Player player)
    {
        return CheckWinByX(x, y, player)
               || CheckWinByY(x, y, player)
               || CheckWinByLeftRow(x, y, player)
               || CheckWinByRightRow(x, y, player);
    }

    private bool CheckWinByX(int x, int y, Player player) //水平检测，返回水平连续棋子个数
    {
        int currentCount = 1;
        currentCount = CheckWinByXRight(x, y, player, currentCount);
        currentCount = CheckWinByXLeft(x, y, player, currentCount);
        return currentCount >= 5;
    }

    private int CheckWinByXRight(int x, int y, Player player, int currentCount) //水平向上检测
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i >= boardSize.x || _board[x + i, y] == null)
            {
                break;
            }

            if (_board[x + i, y].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }

    private int CheckWinByXLeft(int x, int y, Player player, int currentCount) //水平向下检测
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i < 0 || _board[x - i, y] == null)
            {
                break;
            }

            if (_board[x - i, y].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }

    private bool CheckWinByY(int x, int y, Player player) //垂直检测
    {
        int currentCount = 1;
        currentCount = CheckWinByYUp(x, y, player, currentCount);
        currentCount = CheckWinByYDown(x, y, player, currentCount);

        return currentCount >= 5;
    }

    private int CheckWinByYUp(int x, int y, Player player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (y + i >= boardSize.y || _board[x, y + i] == null)
            {
                break;
            }

            if (_board[x, y + i].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }

    private int CheckWinByYDown(int x, int y, Player player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (y - i < 0 || _board[x, y - i] == null)
            {
                break;
            }

            if (_board[x, y - i].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }

    private bool CheckWinByLeftRow(int x, int y, Player player) //左斜检测
    {
        int currentCount = 1;
        currentCount = CheckWinByLeftRowUp(x, y, player, currentCount);
        currentCount = CheckWinByLeftRowDown(x, y, player, currentCount);


        return currentCount >= 5;
    }

    private int CheckWinByLeftRowUp(int x, int y, Player player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i >= boardSize.x || y + i >= boardSize.y || _board[x + i, y + i] == null)
            {
                break;
            }

            if (_board[x + i, y + i].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }

    private int CheckWinByLeftRowDown(int x, int y, Player player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i < 0 || y - i < 0 || _board[x - i, y - i] == null)
            {
                break;
            }

            if (_board[x - i, y - i].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }

    private bool CheckWinByRightRow(int x, int y, Player player) //右斜检测
    {
        int currentCount = 1;
        currentCount = CheckWinByRightRowUp(x, y, player, currentCount);
        currentCount = CheckWinByRightRowDown(x, y, player, currentCount);

        return currentCount >= 5;
    }

    private int CheckWinByRightRowUp(int x, int y, Player player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i < 0 || y + i >= boardSize.y || _board[x - i, y + i] == null)
            {
                break;
            }

            if (_board[x - i, y + i].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }

    private int CheckWinByRightRowDown(int x, int y, Player player, int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i >= boardSize.x || y - i < 0 || _board[x + i, y - i] == null)
            {
                break;
            }

            if (_board[x + i, y - i].playerId == player.id)
            {
                currentCount++;
            }
        }

        return currentCount;
    }
}