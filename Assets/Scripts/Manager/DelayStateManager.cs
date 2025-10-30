using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RecordData 
{
    public bool inDelayAction;
    public RecordData(bool inDelayAction) 
    {
        this.inDelayAction = inDelayAction;
    }
}
public class DelayStateManager : MonoBehaviour,IRecord<RecordData>
{
    public static DelayStateManager instance;
    public bool inDelayAction = false;

    Stack<RecordData> IRecord<RecordData>.stack { get ; set; }

    private void Awake()
    {
        instance = this;
    }
    void IRecord<RecordData>.BackEffectInClass(RecordData data)
    {
        inDelayAction = data.inDelayAction;
    }

    void IRecord<RecordData>.Record()
    {
        ((IRecord<RecordData>)this).stack.Push(new RecordData(inDelayAction));
    }

    private void Start()
    {
        EventManager.OnPlayerEnterDelay += () => inDelayAction = true;
        EventManager.OnPlayerExitDelay += (PlayerExitDelayEventData data) => inDelayAction = false;
        ((IRecord<RecordData>)this).Init();
    }
}
