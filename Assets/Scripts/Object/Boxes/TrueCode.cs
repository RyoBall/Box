using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueCode : GuestCode, IRecord<BaseData>
{
    Stack<BaseData> IRecord<BaseData>.stack { get; set; }

    public override bool CheckMove(Vector2 vec)
    {
        return base.CheckMove(vec);
    }

    public override void Effect(string name)
    {
        Debug.Log(1);
        switch (name)
        {
            case "Bug":
                //����ʲôҲ������
                break;
            case "Jump":
                PlayerController.instance.jumpSkill = true;
                break;
            case "Delay":
                PlayerController.instance.delaySkillUnlock = true;
                break;
            case "Loop":
                Level6SpecialManager.instance.loopSkill = true;
                break;
        }
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
        ((ICode)this).codeType = ICode.CodeType.Guest;
        EventManager.OnLevelChange += Init;
    }
    void Init(LevelChangeData data)
    {
        if (gameObject.layer == (int)data.layer)
        {
            ((IRecord<BaseData>)this).Init();
        }
    }
    void IRecord<BaseData>.Record(PlayerMovEventData data)
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
