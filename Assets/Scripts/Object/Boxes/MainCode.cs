using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCode : Box,ICode,IRecord<BaseData>
{
    [SerializeField]string codeName;
    ICode.CodeType ICode.codeType { get; set; }
    string ICode.name { get; set; }
    Stack<BaseData> IRecord<BaseData>.stack { get; set; }

    public override bool CheckMove(Vector2 vec)
    {
        return base.CheckMove(vec);
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
        ((ICode)this).codeType = ICode.CodeType.Main;
        ((ICode)this).name = codeName;
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
