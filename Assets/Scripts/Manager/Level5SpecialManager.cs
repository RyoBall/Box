using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5SpecialManager : MonoBehaviour
{
    public static Level5SpecialManager instance;
    private void Awake()
    {
        instance = this;
    }
    public void Init() 
    {
        EventManager.OnDeleteBoxGetPush += DeleteBoxFirstGetPush;
    }

    public void DeleteBoxFirstGetPush(GameObject obj) 
    {
        PlayerController.instance.gameObject.SetActive(false);
        obj.SetActive(false);
        MapManager.instance.delObjects.Add(PlayerController.instance.gameObject);
        MapManager.instance.delObjects.Add(obj);
        StartCoroutine(ResetAfterTime());
    }
    IEnumerator ResetAfterTime() 
    {
        yield return new WaitForSeconds(1);
        MapManager.instance.Reset();
        EventManager.OnDeleteBoxGetPush -= DeleteBoxFirstGetPush;
    }
}
