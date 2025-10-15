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
                Move(vec);
                StartCoroutine(CPUEnterTarget());
                return true;
            }
        }
        return base.CheckMove(vec);
    }
    public void BurnOut() 
    {
        ;//失败事件
    }
    IEnumerator CPUEnterTarget() //进入结算应该在移动动画结束后进行
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
