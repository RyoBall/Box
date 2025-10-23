using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrueCode : GuestCode
{
    public override bool CheckMove(Vector2 vec)
    {
        return base.CheckMove(vec);
    }

    public override void Effect(string name)
    {
        switch (name)
        {
            case "Bug":
                //好像什么也不用做
                break;
            case "Jump":
                PlayerController.instance.jumpSkill = true;
                break;
            case "Delay":
                PlayerController.instance.jumpSkill = true;
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
    }
}
