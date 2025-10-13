using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DelayTextManager : MonoBehaviour
{
    public List<TMP_Text> texts;
    public static DelayTextManager instance;
    private void Awake()
    {
        instance = this;
    }
    void DelayTextActive() 
    {
        foreach(TMP_Text text in texts) 
        {
            text.enabled=true;
        }   
    }
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnPlayerEnterDelay += DelayTextActive;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
