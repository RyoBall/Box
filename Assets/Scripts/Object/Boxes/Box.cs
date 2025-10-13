using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Box : CheckObject
{
    public enum Type {Battery,CPU,Target,Other }
    public Type type=Type.Other;
    public bool pushable;
    public int timeCount;
    public TimeText text;
    public bool ismoving;//ֻ����Relaybox�вŻ�ʹ��
    public List<Vector2> moveVec;
    bool delayMovSubscribePlayerMov=false;//代表DelayMov函数是否监听玩家移动事件，找不到检测是否监听的函数先代替一下
    // Start is called before the first frame update
    void Start()
    {
       EventManager.OnPlayerExitDelay+=DelayMoveSubscribePlayerMov;//让延迟移动函数在脱离ExitDelay后订阅玩家移动事件
       EventManager.OnPlayerExitDelay+=DisactiveText;
        text = GetComponentInChildren<TimeText>();
        pushable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual bool GetPush(Vector2 vec) //�����Ƿ�ɹ��ƶ�
    {
        return CheckMove(vec);
    }
    public virtual void GetDelayPush(Vector2 vec) 
    {
        moveVec.Add(vec);
        SetTextCount();
    }
    #region 为了防止重复订阅专门写的让delaymove订阅和取消订阅玩家移动事件的函数
    void DelayMoveSubscribePlayerMov(PlayerExitDelayEventData data) 
    {
        if (!delayMovSubscribePlayerMov) 
        {
            EventManager.OnPlayerMov += DelayMove;
            delayMovSubscribePlayerMov = true;
        }
    }
    void DelayMoveDisSubscribePlayerMov() 
    {
        EventManager.OnPlayerMov -= DelayMove;
        delayMovSubscribePlayerMov = false;
    }
    #endregion
    public void DelayMove(PlayerMovEventData data) 
    {
        if (moveVec.Count > 0) 
        {
            CheckMove(moveVec[0]);
            moveVec.RemoveAt(0);
            SetTextCount();
        }
        else
            DelayMoveDisSubscribePlayerMov();
    }
    public void SetTextCount()
    {
        text.UpdateText(moveVec.Count);
        if (moveVec.Count <= 0 && !DelayStateManager.instance.inDelayAction) 
        {
            text.text.enabled = false;
        }
    }
    public void DisactiveText(PlayerExitDelayEventData data) 
    {
        if(moveVec.Count <= 0) 
        {
            text.text.enabled = false;
        }
    }
    public virtual bool CheckMove(Vector2 vec)//�ƶ��������Ƿ�ɹ��ƶ�
    {
        Box box;
        if (!CheckWithTag(vec, "Box", out box)) 
        {
            Move(vec);
            return true;
        }
        else
        {
            if (box.ismoving) //比如relybox正在移动
            {
                Move(vec);//自己移动
                return true;
            }
            else if (box.pushable)//自己和箱子都可以移动
            {
                if (box.GetPush(vec)) //������Ӳ��ƶ���һ�����ƶ�����
                {
                    Move(vec);
                    return true;
                }
            }
            return false;
        }
    }

}
