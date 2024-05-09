using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public Transform chessContainer;
    public List<Transform> boardTransformPoints = new List<Transform>(); //棋盘两个角
    public Button btnReset;//重置摁钮
    public GameObject player0;
    public GameObject player1;
    public Text log;


    /// <summary>
    /// 重置棋盘画面和输出语句
    /// </summary>
    public void ResetBoardView()
    {
        for (int i = 0; i < chessContainer.childCount; i++)
        {
            Destroy(chessContainer.GetChild(i).gameObject);
        }

        log.text = "";
    }

    /// <summary>
    /// 更新落子画面
    /// </summary>
    /// <param name="chessPosition">落子的世界坐标</param>
    /// <param name="currentPlayer">当前玩家</param>
    public void UpdateBoard(Vector2 chessPosition,Player currentPlayer)
    {
        GameObject chessTemp;
        if (currentPlayer.id == 0)
        {
            chessTemp = Instantiate(player0, chessContainer);
        }
        else
        {
            chessTemp = Instantiate(player1, chessContainer);
        }

        chessTemp.transform.position = chessPosition;
    }

    /// <summary>
    /// 显示胜利语句
    /// </summary>
    /// <param name="currentPlayer">胜利玩家</param>
    public void ShowWinMessage(Player currentPlayer)
    {
        log.text = string.Format("玩家{0}获胜", currentPlayer.id);
    }

    /// <summary>
    /// 显示平局语句
    /// </summary>
    public void ShowDrawMessage()
    {
        log.text = "平局";
    }
}