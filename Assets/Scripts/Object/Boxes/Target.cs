using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : Wall
{
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
    private void Start()
    {
        ;
    }
}
