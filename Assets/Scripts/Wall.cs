using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : Box
{
    public override void Move(Vector2 vec)
    {
        ;
    }

    public override bool TryMove(Vector2 vec)
    {
        return false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
