using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level4SpecialManager : MonoBehaviour
{
    public static Level4SpecialManager instance;
    public List<Vector3> MovPositions;
    int currentPosId=1;
    public Target target;
    bool say=false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        EventManager.OnLevelReset += ResetPosId;
    }
    public void Init() 
    {
        EventManager.OnCPUEnterTarget += ChangeCPU;
        EventManager.OnBatteryEnterHouse += EndChangePos;
    }
    public void Quit() 
    {

    }
    void ResetPosId() 
    {
        currentPosId = 1;
    }
    public void EndChangePos() 
    {
        EventManager.OnCPUEnterTarget -= ChangeCPU;
        EventManager.OnCPUEnterTarget += GameManager.instance.Win;
    }
    public void ChangeCPU(CPUEnterTargetEventData data) 
    {
        int randPos = currentPosId;
        while (randPos == currentPosId) 
        {
            randPos = Random.Range(1, 4);
        }
        target.transform.position = MovPositions[randPos - 1];
        currentPosId = randPos;
        if (!say) 
        {
            Dialogue.instance.ShowText();
            say = true;
        }
    }
}
