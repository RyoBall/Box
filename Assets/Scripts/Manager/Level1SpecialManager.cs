using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1SpecialManager : MonoBehaviour//�����������ר��ʵ�ֵ�һ�ص��黯Ч��
{
    public static Level1SpecialManager instance;
    public GameObject badInsect;
    public Material material;
    [SerializeField] private Color clarityColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private GameObject wallToDestroy;
    private CPUEnterTargetEventData data;
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
        data.cpu.transform.position = badInsect.transform.position + new Vector3(0,-0.2f,0);
        PlayerController.instance.jumpSkill = true;
        material.color = clarityColor;
        Destroy(wallToDestroy);
        this.data = data;
        EventManager.OnCPUEnterTarget -= CPUEnterSpecialEvent;
        Dialogue.instance.ShowText();
    }
    public void HitBadInsectEvent() 
    {
        GameManager.instance.Win(data);
    }
}
