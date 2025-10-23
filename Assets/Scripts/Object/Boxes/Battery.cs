using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : Box
{
    public bool inPower;
    public Material material;
    private Color originColor;
    public void FindPlayer() 
    {
        
        if (PlayerController.instance.inPower&&ChecDistance()) 
        {
            PlayerController.instance.inPower = false;
            inPower = true;
            material.color = Color.red;
        }
    }
    bool ChecDistance() { return PlayerController.instance.transform.position.x - transform.position.x <= 1 && PlayerController.instance.transform.position.x - transform.position.x >= -1 && PlayerController.instance.transform.position.z - transform.position.z <= 1 && PlayerController.instance.transform.position.z - transform.position.z >= -1; }
    
    public override bool CheckMove(Vector2 vec)
    {
        Box box;
        BatteryHouse house;
        if(CheckWithTag(vec,"Box",out box)) 
        {
            if (box.TryGetComponent<BatteryHouse>(out house)) 
            {
                Move(vec);
                return true;
            }
        }
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

    public void ResetPower(object sender, EventArgs e)
    {
        this.inPower = false;
        material.color = originColor;
    }

    protected override void Start()
    {
        base.Start();
        Renderer renderer = GetComponent<Renderer>();
        material =new Material(material);
        renderer.material = material;
        EventManager.OnPlayerOverMov += FindPlayer;
        originColor = material.color;
        MapManager.OnReset += ResetPower;
    }
}
