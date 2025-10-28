using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static ICode;

public class IsCode : Box, ICode,IRecord<BaseData>
{
    ICode.CodeType ICode.codeType { get; set; }
    string ICode.name { get; set; }
    Stack<BaseData> IRecord<BaseData>.stack { get; set; }

    public void Init(LevelChangeData data)
    {
        if (gameObject.layer == (int)data.layer)
        {
            ((IRecord<BaseData>)this).Init();
            EventManager.OnPlayerOverMov += HorizontalChec;
            EventManager.OnPlayerOverMov += VerticalChec;
        }
    }
    public void Quit(LevelChangeData data)
    {
        if (gameObject.layer != (int)data.layer)
        {
            EventManager.OnPlayerOverMov -= HorizontalChec;
            EventManager.OnPlayerOverMov -= VerticalChec;
        }
    }
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
    public void HorizontalChec()
    {
        Debug.Log("IsChec");
        CheckObject box;
        ICode main = null;
        GuestCode guest = null;
        if (CheckWithTag<CheckObject>(Vector2.up, "Box", out box))
        {
            ICode code = box.GetComponent<ICode>();
            if (code != null && code.codeType == ICode.CodeType.Main)
            {
                main = box.GetComponent<ICode>();
            }
        }
        if (CheckWithTag<CheckObject>(Vector2.down, "Box", out box))
        {
            ICode code = box.GetComponent<ICode>();
            if (code != null && code.codeType == ICode.CodeType.Guest)
            {
                guest = box.GetComponent<GuestCode>();
            }
        }
        Effect(main, guest);
    }
    public void VerticalChec()
    {
        CheckObject box;
        ICode main = null;
        GuestCode guest = null;
        if (CheckWithTag<CheckObject>(Vector2.left, "Box", out box))
        {
            ICode code = box.GetComponent<ICode>();
            if (code != null && code.codeType == ICode.CodeType.Main)
            {
                main = box.GetComponent<ICode>();
            }
        }
        if (CheckWithTag<CheckObject>(Vector2.right, "Box", out box))
        {
            ICode code = box.GetComponent<ICode>();
            if (code != null && code.codeType == ICode.CodeType.Guest)
            {
                guest = box.GetComponent<GuestCode>();
            }
        }
        Effect(main, guest);
    }
    public void Effect(ICode main, GuestCode guest)
    {
        if (main != null && guest != null)
        {
            guest.Effect(main.name);
        }
    }

    protected override void Start()
    {
        EventManager.OnLevelChange += Init;
        EventManager.OnLevelChange += Quit;
        ((ICode)this).codeType = ICode.CodeType.Is;
        base.Start();
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
