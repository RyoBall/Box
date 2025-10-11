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

    public override bool TryMove(Vector2 vec)
    {
        if (!ismoving) 
        {
            Box box;
            ChecBox(vec, out box);
            box.GetPush(vec);
            return false;
        }
        return base.TryMove(vec);
    }
    void SetTimeCount(int count)
    {
        timeCount = count;
        if (timeCount == 1 && ChecMoving())
            ismoving = true;
        text.UpdateText(count);
    }
    public void CountTime(PlayerMovEventData data)
    {
        SetTimeCount(timeCount - 1);
        if (timeCount <= 0)
        {
            TryMove(moveVec);
        }
        EventManager.OnPlayerMov -= CountTime;
    }
    public bool ChecMoving()
    {
        Box box;
        ChecBox(moveVec, out box);
        if (box != null)
            return false;
        else
            return true;
    }
}
