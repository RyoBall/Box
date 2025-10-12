using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Box : CheckObject
{
    public bool pushable;
    public int timeCount;
    public TimeText text;
    public bool ismoving;//ֻ����Relaybox�вŻ�ʹ��
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual bool GetPush(Vector2 vec) //�����Ƿ�ɹ��ƶ�
    {
        return CheckMove(vec);
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
