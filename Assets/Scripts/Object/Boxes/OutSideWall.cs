using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutSideWall : Box
{
    protected override void Start()
    {
        pushable = false;
        type = Type.Wall;
    }
}
