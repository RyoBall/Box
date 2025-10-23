using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Loop : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Box"&&Level6SpecialManager.instance.loopSkill)
        {
            EventManager.LoopEnter(new LoopEnterEventData(collision.gameObject));
            GameObject box=Instantiate(collision.collider.gameObject, collision.collider.transform.position + new Vector3(collision.collider.GetComponent<Box>().currentMoveVec.x, 0, collision.collider.GetComponent<Box>().currentMoveVec.y), Quaternion.identity);
            MapManager.instance.tmpObjects.Add(box);
        }
    }
}
