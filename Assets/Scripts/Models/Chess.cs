using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess
{
    public int playerId;
    
    /// <summary>
    /// 通过id生成chess对象
    /// </summary>
    /// <param name="id">棋子对应的playerId</param>
    public Chess(int id)
    {
        playerId = id;
    }
}
