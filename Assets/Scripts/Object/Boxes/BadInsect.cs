using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BadInsect : CheckObject
{
    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.transform.position.x >= transform.position.x)
        {
            Level1SpecialManager.instance.HitBadInsectEvent();
        }
    }
    
}
