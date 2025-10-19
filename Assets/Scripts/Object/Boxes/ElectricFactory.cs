using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFactory : MonoBehaviour
{
    private void Start()
    {
        EventManager.OnPlayerOverMov += ChecUp;
    }
    private void Update()
    {
        
    }
    void ChecUp() 
    {
        RaycastHit[] hits= Physics.RaycastAll(transform.position-new Vector3(0,0.1f,0), Vector3.up, .5f);
        foreach (RaycastHit hit in hits) 
        {
            if (hit.collider.GetComponent<PlayerController>() != null) 
            {
                hit.collider.GetComponent<PlayerController>().inPower = true;
            }
            else if(hit.collider.GetComponent<Battery>() != null) 
            {
                hit.collider.GetComponent<Battery>().inPower = true;
            }
            else if (hit.collider.GetComponent<CPU>() != null) 
            {
                hit.collider.GetComponent<CPU>().BurnOut();
            }
        }
    }
}
