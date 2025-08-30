using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PresentationScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int stageID = StageManager.Instance.SelectedStageID;
        Debug.Log("---------------------------- presentationScene에서도 Stage_ID: " + stageID + "----------------------------");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
