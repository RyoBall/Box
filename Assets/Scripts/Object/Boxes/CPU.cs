using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPU : Box
{
    public override bool CheckMove(Vector2 vec)
    {
        Box box;
        if (CheckWithTag(vec, "Box", out box))
        {
            if (box.GetComponent<Box>().type == Box.Type.Target)
            {
                Debug.Log("Enter Target");
                Move(vec);
                StartCoroutine(CPUEnterTarget());
                return true;
            }
            else if (box.type == Type.Unreal)
            {
                return false;
            }
        }
        return base.CheckMove(vec);
    }
    public void BurnOut() 
    {
        pushable = false;//ʧ���¼�
    }
    IEnumerator CPUEnterTarget() //�������Ӧ�����ƶ��������������
    {
        yield return new WaitForSeconds(Data.fixedMovTime);
        EventManager.CPUEnterTarget(new CPUEnterTargetEventData(this));
    }
    public override void GetDelayPush(Vector2 vec)
    {
        base.GetDelayPush(vec);
    }

    public override bool GetPush(Vector2 vec)
    {
        return base.GetPush(vec);
    }

    public override void Move(Vector2 vec)
    {
        base.Move(vec);
    }
}
