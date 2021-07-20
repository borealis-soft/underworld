using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanel : MonoBehaviour
{
    public MainMenuControls menuControls;

    private void Awake()
    {
        Time.timeScale = 0;
        menuControls.enabled = false;
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.SetActive(false);
            menuControls.enabled = true;
            Time.timeScale = 1;
        }
    }
}
