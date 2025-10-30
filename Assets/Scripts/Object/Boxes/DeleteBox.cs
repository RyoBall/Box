using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BaseData 
{
    public Vector3 position;
    public List<Vector2> moveVec;
    public BaseData(Vector3 position, List<Vector2> moveVec)
    {
        this.position = position;
        this.moveVec = moveVec;
    }
}

public class DeleteBox : Box,IRecord<BaseData>
{
    Stack<BaseData> IRecord<BaseData>.stack { get ; set; }

    public override bool CheckMove(Vector2 vec)
    {
        StartCoroutine(SendEvent());
        Box box;
        if (CheckWithTag(vec, "Box", out box) && box.type == Type.Wall) 
        {
            gameObject.SetActive(false);
            box.gameObject.SetActive(false);
            MapManager.instance.delObjects.Add(gameObject);
            MapManager.instance.delObjects.Add(box.gameObject);
            return true;
        }
        return base.CheckMove(vec);
    }
    IEnumerator SendEvent() 
    {
        yield return new WaitForSeconds(0);
        EventManager.DeleteBoxGetPush(gameObject);    
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

    protected override void Start()
    {
        base.Start();
        EventManager.OnLevelChange += Init;
    }
    public override void Init(LevelChangeData data)
    {
        if (gameObject.layer == (int)data.layer)
        {
            ((IRecord<BaseData>)this).Init();
        }
    }
    void IRecord<BaseData>.Record()
    {
        List<Vector2> Vecs = new List<Vector2>();
        for (int i = 0; i < moveVec.Count; i++)
        {
            Vecs.Add(moveVec[i]);
        }
        ((IRecord<BaseData>)this).stack.Push(new BaseData(transform.position, Vecs));
    }

    void IRecord<BaseData>.BackEffectInClass(BaseData data)
    {
        transform.position = data.position;
        moveVec = data.moveVec;
        SetTextCount();
    }
}
