using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitPlayersController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerResourses.Singleton.ImReadyServerRpc();
            MainMenuControls.Instance.WaitPlayersPanel.GetComponentInChildren<Text>().color = Color.green;
        }
    }
}
