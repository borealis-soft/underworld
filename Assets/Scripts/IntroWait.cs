using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroWait : MonoBehaviour
{
    public float WaitTime;

    void Awake()
    {
        StartCoroutine(WaitOfVideo());
    }

    private IEnumerator WaitOfVideo()
    {
        yield return new WaitForSeconds(WaitTime);
        SceneManager.LoadScene(1);
    }
}