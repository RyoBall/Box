using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    enum State
    {
        OPEN,
        ONE,
        TWO,
        THREE,
        END
    }

    private State state;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        state = State.ONE;//暂时没有做开头
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            MapManager.instance.Reset();
        }
    }
}
