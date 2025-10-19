using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHouse : Wall
{
    private void Start()
    {
        EventManager.OnPlayerOverMov += ChecBattery;
    }
    void ChecBattery() 
    {
        RaycastHit[] hits= Physics.RaycastAll(transform.position-new Vector3(0,1,0),new Vector3(0,1,0),1f);
        foreach(RaycastHit hit in hits) 
        {
            Debug.Log(1);
            if (hit.collider.gameObject.TryGetComponent(out Box box))
            {
                if (box.type == Box.Type.Battery)
                {
                    if (box.GetComponent<Battery>().inPower) 
                    {
                        Debug.Log(1);
                        Effect();   
                    }
                }
            }
        }
    }
    public void Effect() 
    {
        Debug.Log(1);
        EventManager.BatteryEnterHouse();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+new Vector3(0,0,1));
    }

    public override string ToString()
    {
        return base.ToString();
    }

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
}
