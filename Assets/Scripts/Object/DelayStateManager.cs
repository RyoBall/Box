using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayStateManager : MonoBehaviour
{
    public static DelayStateManager instance;
    public bool inDelayAction = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EventManager.OnPlayerEnterDelay += () => inDelayAction = true;
        EventManager.OnPlayerExitDelay += (PlayerExitDelayEventData data) => inDelayAction = false;
    }
}
