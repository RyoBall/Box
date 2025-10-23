using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3SpecialManager : MonoBehaviour
{
    public static Level3SpecialManager instance;
    public List<Box> DelayBoxes;
    public GameObject unrealBox;
    private void Awake()
    {
        instance = this;
    }
    public void Init()
    {
        EventManager.PlayerEnterDelay();
        PlayerController.instance.delay = true;
        EventManager.OnPlayerMov += ChecDelayBoxes;
        EventManager.OnBatteryEnterHouse += DestroyUnrealBox;
    }
    public void Quit()
    {
        EventManager.OnPlayerMov -= ChecDelayBoxes;
        EventManager.PlayerExitDelay(null);
        PlayerController.instance.delay = false;
    }
    public void DestroyUnrealBox() 
    {
        Destroy(unrealBox);
    }
    public void ChecDelayBoxes(PlayerMovEventData data) 
    {
        foreach(Box box in DelayBoxes) 
        {
            if (box.moveVec.Count >= 2) 
            {
                EventManager.OnPlayerMov += box.DelayMove;
            }
            else if(box.moveVec.Count <= 0) 
            {
                EventManager.OnPlayerMov -= box.DelayMove;
            }
        }
    }
}
