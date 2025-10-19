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
    public static Action OnCPUGetPush;
    public static Action OnBatteryEnterHouse;
    public static Action OnLevelReset;
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
    public static void CPUGetPush() 
    {
        OnCPUGetPush?.Invoke();
    }
    public static void BatteryEnterHouse() 
    {
        OnBatteryEnterHouse?.Invoke();
    }
    public static void LevelReset() 
    {
        OnLevelReset?.Invoke();
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
}public class BatteryEnterHouseData
{
    int eventID;
    public BatteryEnterHouseData(int eventID) 
    {
        this.eventID=eventID;
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