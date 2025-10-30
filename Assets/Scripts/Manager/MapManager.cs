using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MapData 
{
    public List<GameObject> tmpObjects = new List<GameObject>();
    public List<GameObject> delObjects = new List<GameObject>();
    public MapData(List<GameObject> tmpObjects, List<GameObject> delObjects)
    {
        this.tmpObjects = tmpObjects;
        this.delObjects = delObjects;
    }
}
public class MapManager : MonoBehaviour,IRecord<MapData>
{
    public static MapManager instance;
    public static event EventHandler OnReset;
    
    private Dictionary<Transform, TransformData> originalTransforms = new Dictionary<Transform, TransformData>();
    public List<GameObject> tmpObjects = new List<GameObject>();
    public List<GameObject> delObjects = new List<GameObject>();
    public bool playerJumpSkill = false;
    public bool playerDelaySkill = false;

    Stack<MapData> IRecord<MapData>.stack { get; set; }

    public class TransformData
    {
        public Vector3 position;
    }
    // Start is called before the first frame update
    void Start()//这里用Dictionary保存物体初始位置，后续需要保存其他数据可以调整
    {
        instance = this;
        int currentLayer = LayerMask.NameToLayer("Level1"); 
        ResaveTransformAndResetPlayer(currentLayer);
        ((IRecord<MapData>)this).Init();
        EventManager.OnPlayerReadyToMov += Record;
    }

    public void ResaveTransformAndResetPlayer(LayerMask layer)//进入新一关需要使用这个函数来保存新一关的初始位置
    {
        var allObject = FindObjectsOfType<Transform>();
        foreach (Transform t in allObject)
        {
            if (t.hideFlags == HideFlags.None && (t.gameObject.layer == layer || t.CompareTag("Player")))
            {
                var data = new TransformData()
                {
                    position = t.position,
                };
                originalTransforms[t] = data;
            }
        }

        PlayerController.instance.jumpCount = 0;
        PlayerController.instance.canJump = true;
        PlayerController.instance.onBox = false;
    }

    public void Reset()//所有物体回到原点
    {
        DelayStateManager.instance.inDelayAction = false;
        EventManager.LevelReset();
        foreach (var t in originalTransforms)
        {
            t.Key.position = t.Value.position;
        }
        foreach(GameObject obj in tmpObjects) 
        {
            if (obj.GetComponent<Box>() != null)
                OnReset -= obj.GetComponent<Box>().ResetDealyBox;
            Destroy(obj);
        }       
        foreach(GameObject obj in delObjects) 
        {
            obj.SetActive(true);
        }
        tmpObjects.Clear();
        delObjects.Clear();
        if (PlayerController.instance.jumpSkill)
        {
            PlayerController.instance.jumpCount = 0;
            PlayerController.instance.canJump = true;
            PlayerController.instance.onBox=false;
        }
        if(PlayerController.instance.delaySkillUnlock)
        PlayerController.instance.delay = false;
        OnReset?.Invoke(this, EventArgs.Empty);
    }

    void IRecord<MapData>.Record()
    {
        ;
    }

    void IRecord<MapData>.BackEffectInClass(MapData data)
    {
        foreach(GameObject tmpObject in tmpObjects) 
        {
            if (!data.tmpObjects.Contains(tmpObject)) 
            {
                Destroy(tmpObject);
            }
        }
        foreach(GameObject delObject in delObjects) 
        {
            if (!data.delObjects.Contains(delObject)) 
            {
                delObject.SetActive(true);
            }
        }
        tmpObjects = data.tmpObjects;
        delObjects = data.delObjects;
    }
    void Record() 
    {
        List<GameObject> tmpObjectsToStole = new List<GameObject>();
        List<GameObject> delObjectsToStole = new List<GameObject>();
        for (int i = 0; i < tmpObjects.Count; i++)
        {
            tmpObjectsToStole.Add(tmpObjects[i]);
        }
        for (int i = 0; i < delObjects.Count; i++)
        {
            delObjectsToStole.Add(delObjects[i]);
        }
        ((IRecord<MapData>)this).stack.Push(new MapData(tmpObjectsToStole, delObjectsToStole));
    }
}
