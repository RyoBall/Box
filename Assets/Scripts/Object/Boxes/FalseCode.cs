using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseCode : GuestCode
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
                //Ê§°Ü
                break;
            case "Jump":
                PlayerController.instance.jumpSkill = false;
                break;
            case "Delay":
                PlayerController.instance.jumpSkill = false;
                break;
            case "Loop":
                Level6SpecialManager.instance.loopSkill = true;
                Level6SpecialManager.instance.loopBug = false;
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
