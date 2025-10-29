using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

public class Level6SpecialManager : MonoBehaviour
{
    public List<bool> allTargetsGetIn;
    public List<Target> targets;
    public static Level6SpecialManager instance;
    public bool loopSkill = true;
    public bool loopBug = true;
    const float genTime = .2f;
    bool say = false;
    private void Awake()
    {
        instance = this;
    }
    public void Init()
    {
        EventManager.OnCPUEnterTarget -= GameManager.instance.Win;
        EventManager.OnPlayerOverMov += TargetChecUp;
        EventManager.OnLevelReset += LevelReset;
        EventManager.OnLoopEnter += LoopBug;
    }
    public void LoopBug(LoopEnterEventData data)
    {
        if (loopBug)
        {
            if (!say)
            {
                Dialogue.instance.ShowText();
                say = true;
            }
            StartCoroutine(LoopGenerate(20, data.gameObject));
        }
    }
    IEnumerator LoopGenerate(int time, GameObject gameObject)
    {
        yield return new WaitForSeconds(.2f);
        if (gameObject != null)
        {
            GameObject obj = Instantiate(gameObject, gameObject.transform.position + new Vector3(Random.Range(0, 8f), 0, Random.Range(0, 8f)), Quaternion.identity);
            MapManager.instance.tmpObjects.Add(obj);
            if (time > 0)
                StartCoroutine(LoopGenerate(time - 1, gameObject));
        }
    }
    public void LevelReset()
    {
        loopSkill = true;
        loopBug = true;
    }
    public void TargetChecUp()
    {
        for (int i = 0; i < 2; i++)
        {
            bool getCPU=false;
            RaycastHit[] hits = Physics.RaycastAll(targets[i].transform.position - new Vector3(0, 0.6f, 0), Vector3.up, .5f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.GetComponent<CPU>() != null)
                {
                    getCPU = true;
                    allTargetsGetIn[i] = true;
                }
            }
            if(!getCPU)
            allTargetsGetIn[i] = false;
        }
        if (allTargetsGetIn[0] && allTargetsGetIn[1])
            GameManager.instance.Win(null);
    }
}
