using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BadInsect : CheckObject
{
    private void Start()
    {
        EventManager.OnPlayerOverMov += BadInsectChec;
    }
    public void BadInsectChec() 
    {
        if (PlayerController.instance.transform.position.x == transform.position.x && PlayerController.instance.transform.position.z == transform.position.z)
        {
            Level1SpecialManager.instance.HitBadInsectEvent();
        }
    }
}
