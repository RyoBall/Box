using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5SpecialManager : MonoBehaviour
{
    public static Level5SpecialManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void Init() 
    {
        EventManager.OnDeleteBoxGetPush += DeleteBoxFirstGetPush;
    }

    public void DeleteBoxFirstGetPush() 
    {
        MapManager.instance.Reset();
        MapManager.instance.Reset();
        EventManager.OnDeleteBoxGetPush -= DeleteBoxFirstGetPush;
    }
}
