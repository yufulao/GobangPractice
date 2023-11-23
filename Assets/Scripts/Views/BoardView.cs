using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardView : MonoBehaviour
{
    public Transform chessContainer;
    public GameObject player0;
    public GameObject player1;
    public Text log;


    public void ResetBoardView()
    {
        for (int i = 0; i < chessContainer.childCount; i++)
        {
            Destroy(chessContainer.GetChild(i).gameObject);
        }

        log.text = "";
    }

    public void UpdateBoard(Vector2 chessPosition, int currentPlayer)
    {
        GameObject chessTemp;
        if (currentPlayer == 0)
        {
            chessTemp = Instantiate(player0, chessContainer);
        }
        else
        {
            chessTemp = Instantiate(player1, chessContainer);
        }

        chessTemp.transform.position = chessPosition;
    }

    public void ShowWinMessage(int result)
    {
        log.text = string.Format("玩家{0}获胜", result);
    }
}