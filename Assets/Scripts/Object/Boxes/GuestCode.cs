using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuestCode : Box,ICode
{
    public ICode.CodeType codeType { get; set; }

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
    public virtual void Effect(string name) 
    {
        ;
    }

    protected override void Start()
    {
        base.Start();
    }
}
