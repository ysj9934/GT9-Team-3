using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSave : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Plus()
    {
        SaveManager.Instance.data.gold += 100;
        SaveManager.Instance.Save();
        Debug.Log("골드 증가: " + SaveManager.Instance.data.gold);
    }
}
