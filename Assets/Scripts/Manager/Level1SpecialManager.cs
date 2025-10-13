using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1SpecialManager : MonoBehaviour//用这个管理器专门实现第一关的虚化效果
{
    public static Level1SpecialManager instance;
    public GameObject badInsect;
    public Material material;
    [SerializeField] private Color clarityColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private GameObject wallToDestroy;
    private CPU cPU;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        material.color = normalColor;
        EventManager.OnCPUEnterTarget += CPUEnterSpecialEvent;
    }
    public void CPUEnterSpecialEvent(CPUEnterTargetEventData data) 
    {
        data.cpu.transform.position = badInsect.transform.position;
        PlayerController.instance.jump = true;
        material.color = clarityColor;
        Destroy(wallToDestroy);
    }
    public void HitBadInsectEvent() 
    {
        GameManager.instance.Win(new CPUEnterTargetEventData());
    }
}
