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
    public static Action OnPlayerOverMov;
    public static Action<CPUEnterTargetEventData> OnCPUEnterTarget;
    public static EventManager instance;

    private void Start()
    {
        instance = this;
    }
    public static void PlayerMov(PlayerMovEventData data) 
    {
        OnPlayerMov?.Invoke(data);
    }
    public static void PlayerOverMov() 
    {
        OnPlayerOverMov?.Invoke();
    }
    public static void PlayerExitDelay(PlayerExitDelayEventData data) 
    {
        OnPlayerExitDelay?.Invoke(data);
    }   
    public static void PlayerEnterDelay() 
    {
        OnPlayerEnterDelay?.Invoke();
    }
    public static void CPUEnterTarget(CPUEnterTargetEventData data) 
    {
        OnCPUEnterTarget?.Invoke(data);
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
public class CPUEnterTargetEventData 
{
    public CPU cpu;
    public CPUEnterTargetEventData(CPU cpu) 
    {
        this.cpu=cpu;
    }    
    public CPUEnterTargetEventData() 
    {
        this.cpu=null;
    }
}