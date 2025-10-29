using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    public TMP_Text text;

    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TMP_Text>();
        DelayTextManager.instance.texts.Add(text);
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateText(int count) 
    {
        text.text = count.ToString();     
    }
}
