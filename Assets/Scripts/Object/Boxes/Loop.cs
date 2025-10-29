using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Loop : MonoBehaviour
{
    Collider collision=null;
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Box" && Level6SpecialManager.instance.loopSkill && collision.gameObject.GetComponent<Box>().cloneable)
        {
            Debug.Log(collision.name);
            this.collision = collision;
            EventManager.OnPlayerOverMov += LoopEnter;
        }
    }
    public void LoopEnter() 
    {
        EventManager.OnPlayerOverMov -= LoopEnter;
        GameObject box = Instantiate(collision.gameObject, collision.transform.position + new Vector3(collision.GetComponent<Box>().currentMoveVec.x, 0, collision.GetComponent<Box>().currentMoveVec.y), Quaternion.identity);
            box.GetComponent<Box>().cloneable = false;
        MapManager.instance.tmpObjects.Add(box);
        EventManager.LoopEnter(new LoopEnterEventData(collision.gameObject));
    }
}
