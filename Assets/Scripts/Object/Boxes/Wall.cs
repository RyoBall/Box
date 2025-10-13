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

    // Start is called before the first frame update
    void Start()
    {
        pushable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
