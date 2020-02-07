using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

    public void StartNewGame()
    {
        TerneebGameController tgc
         = GameObject.Find("Tarneeb Game").GetComponent<TerneebGameController>();
        StartCoroutine(tgc.StartGame());
        
    }
}
