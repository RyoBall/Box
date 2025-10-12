using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelyBox : Box
{
    Vector2 moveVec;
    public override bool GetPush(Vector2 vec)
    {
        moveVec = vec;
        SetTimeCount(1);
        EventManager.OnPlayerMov += CountTime;
        return false;
    }

    public override void Move(Vector2 vec)
    {
        transform.DOMove(transform.position + new Vector3(vec.x * Data.fixedLength, 0, vec.y * Data.fixedLength), Data.fixedMovTime).OnComplete(()=>ismoving=false);
    }

    public override bool CheckMove(Vector2 vec)
    {
        if (ismoving) //���û�����ƶ����Ͳ���
        {
            if (!CheckWithTag(vec, "Box"))
            {
                return base.CheckMove(vec);
            }
            ismoving = false;
        }
        return false;
    }
    void SetTimeCount(int count)
    {
        timeCount = count;
        if (timeCount == 1 && !CheckWithTag(moveVec,"Wall"))
            ismoving = true;
        text.UpdateText(count);
    }
    public void CountTime(PlayerMovEventData data)
    {
        SetTimeCount(timeCount - 1);
        if (timeCount <= 0)
        {
            CheckMove(moveVec);
        }
        EventManager.OnPlayerMov -= CountTime;
    }
}
