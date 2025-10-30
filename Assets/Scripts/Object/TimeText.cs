using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour
{
    public TMP_Text text;
    [SerializeField] float offset=0;
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
        transform.localPosition = new Vector3(0, offset, 0);
    }
}
