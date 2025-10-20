using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Box
{
    
    public override bool CheckMove(Vector2 vec)
    {
        return false;
    }

    public override void GetDelayPush(Vector2 vec)
    {
        
    }

    public override bool GetPush(Vector2 vec)
    {
        return false;
    }

    public override void Move(Vector2 vec)
    {
        ;
    }

    protected override void Start()
    {
        pushable = false;
        type = Type.Other;
    }

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        
    }
}
