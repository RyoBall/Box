using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHouse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnPlayerMov += ChecBattery;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChecBattery(PlayerMovEventData data) 
    {
        RaycastHit[] hits= Physics.RaycastAll(transform.position,new Vector3(0,1,0),.1f);
        foreach(RaycastHit hit in hits) 
        {
            if (hit.collider.tag=="Box"&&hit.collider.gameObject.GetComponent<Box>().type == Box.Type.Battery)
            {
                if (hit.collider.gameObject.GetComponent<Battery>().inPower) 
                {
                    Effect();   
                }
            }
        }
    }
    public void Effect() 
    {
        
    }
}
