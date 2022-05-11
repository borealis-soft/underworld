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
        bool PushStartBtn = false;
#if UNITY_STANDALONE
        PushStartBtn = Input.GetKey(KeyCode.Space);
#endif
#if UNITY_ANDROID
        PushStartBtn = Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
#endif
        if (PushStartBtn)
        {
            gameObject.SetActive(false);
            menuControls.enabled = true;
            Time.timeScale = 1;
        }
    }
}
