using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<PlayerMovEventData> OnPlayerMov;
    public static EventManager instance;

    private void Start()
    {
        instance = this;
    }

    public static void PlayerMov(PlayerMovEventData data) 
    {

        OnPlayerMov?.Invoke(data);
    }
    
}

public class PlayerMovEventData 
{
    public PlayerMovEventData() 
    {
        ;
    }
}