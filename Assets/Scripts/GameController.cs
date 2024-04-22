using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private List<GameObject> buttonList = new List<GameObject>();

    [HideInInspector] public Button[][] buttons = new Button[3][];

    
    void Start()
    {
        for (int i = ;)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
