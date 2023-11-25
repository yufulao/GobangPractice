using System;
using UnityEngine;
public class BaseMonoSingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            //实例不存在
            if (_instance != null) return _instance;
            //场景里找
            var obj = FindObjectOfType<T>(true);
            if (!obj)
            {
                throw new Exception($"场景中找不到单例物体 name:{typeof(T)}");
            }

            //创建个新的
            _instance = obj;
            return _instance;
        }
    }
}