using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Board
{
    [HideInInspector] public Vector2 boardSize;
    private int[,] _board; // 二维数组表示棋盘状态，0表示空，1表示黑子，2表示白子
    private int _totalSites = 0; //一共多少个位，优化判断平局
    private int _currentTotalSet = 0; //当前一共下了多少个棋子

    public void InitBoard() //初始化棋盘
    {
        boardSize = new Vector2(14, 14);
        _board = new int[14, 14];
        _totalSites = 14 * 14;
        ResetBoard();
    }

    public void ResetBoard()
    {
        //重置棋盘
        for (var i = 0; i < _board.GetLength(0); i++)
        {
            for (var j = 0; j < _board.GetLength(1); j++)
            {
                _board[i, j] = 0;
            }
        }
    }

    public bool SetChess(int x, int y, int player) //落子
    {
        if (_board[x, y] == 0) //该位置为空
        {
            _board[x, y] = player;
            _currentTotalSet++;
            return true;
        }
        else
        {
            return false;
        }
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

    private bool CheckDraw() //检测平局
    {
        return _currentTotalSet >= _totalSites;
    }

    private bool CheckWin(int x, int y, int player) //检测胜利
    {
        if (CheckWinByX(x, y, player)
            || CheckWinByY(x, y, player)
            || CheckWinByLeftRow(x, y, player)
            || CheckWinByRightRow(x, y, player))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckWinByX(int x, int y, int player) //水平检测，返回水平连续棋子个数
    {
        int currentCount = 1;
        CheckWinByXUp(x, y, player, ref currentCount);
        CheckWinByXDown(x, y, player, ref currentCount);
        return currentCount >= 5;
    }
    private void CheckWinByXUp(int x, int y, int player, ref int currentCount) //水平向上检测
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i < boardSize.x && _board[x + i, y] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
    private void CheckWinByXDown(int x, int y, int player, ref int currentCount) //水平向下检测
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i >= 0 && _board[x - i, y] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
    private bool CheckWinByY(int x, int y, int player) //垂直检测
    {
        int currentCount = 1;
        CheckWinByYUp(x, y, player, ref currentCount);
        CheckWinByYDown(x, y, player, ref currentCount);

        return currentCount >= 5;
    }
    private void CheckWinByYUp(int x, int y, int player, ref int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (y + i < boardSize.y && _board[x, y + i] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
    private void CheckWinByYDown(int x, int y, int player, ref int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (y - i >= 0 && _board[x, y - i] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
    private bool CheckWinByLeftRow(int x, int y, int player) //左斜检测
    {
        int currentCount = 1;
        CheckWinByLeftRowUp(x, y, player, ref currentCount);
        CheckWinByLeftRowDown(x, y, player, ref currentCount);


        return currentCount >= 5;
    }
    private void CheckWinByLeftRowUp(int x, int y, int player, ref int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i < boardSize.x && y + i < boardSize.y && _board[x + i, y + i] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
    private void CheckWinByLeftRowDown(int x, int y, int player, ref int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i >= 0 && y - i >= 0 && _board[x - i, y - i] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
    private bool CheckWinByRightRow(int x, int y, int player) //右斜检测
    {
        int currentCount = 1;
        CheckWinByRightRowUp(x, y, player, ref currentCount);
        CheckWinByRightRowDown(x, y, player, ref currentCount);

        return currentCount >= 5;
    }
    private void CheckWinByRightRowUp(int x, int y, int player, ref int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x - i >= 0 && y + i < boardSize.y && _board[x - i, y + i] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
    private void CheckWinByRightRowDown(int x, int y, int player, ref int currentCount)
    {
        for (int i = 1; i < 5; i++)
        {
            if (x + i < boardSize.x && y - i >= 0 && _board[x + i, y - i] == player)
            {
                currentCount++;
            }
            else
            {
                return;
            }
        }
    }
}