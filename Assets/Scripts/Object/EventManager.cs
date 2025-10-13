using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action<PlayerMovEventData> OnPlayerMov;
    public static Action<PlayerExitDelayEventData> OnPlayerExitDelay;
    public static Action OnPlayerEnterDelay;
    public static EventManager instance;

    private void Start()
    {
        instance = this;
    }
    public static bool ChecSubscribe(Action action,MethodInfo method) 
    {
        if (action == null)
            return false;

        Delegate[] delegates = action.GetInvocationList();
        foreach (Delegate del in delegates)
        {
            if (del.Method == method)
                return true;
        }
        return false;
    }
    public static void PlayerMov(PlayerMovEventData data) 
    {
        OnPlayerMov?.Invoke(data);
    }
    public static void PlayerExitDelay(PlayerExitDelayEventData data) 
    {
        OnPlayerExitDelay?.Invoke(data);
    }   
    public static void PlayerEnterDelay() 
    {
        OnPlayerEnterDelay?.Invoke();
    }
    
}

public class PlayerMovEventData 
{
    public PlayerMovEventData() 
    {
        ;
    }
}
public class PlayerExitDelayEventData 
{
    public PlayerExitDelayEventData() 
    {
        ;
    }
}