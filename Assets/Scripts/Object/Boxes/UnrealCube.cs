using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnrealCube : Wall
{
    // Start is called before the first frame update
    void Start()
    {
        type = Type.Unreal;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Box box))
        {
            if (box.TryGetComponent(out Battery battery))
            {
                if (battery.inPower)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
