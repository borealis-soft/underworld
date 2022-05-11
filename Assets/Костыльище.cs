using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Костыльище : MonoBehaviour
{
    public GameObject Pref;
    void Start()
    {
        MainMenuControls.Instance.ResetAll += GetComponent<Tower>().Reset;
    }

    private void OnDestroy()
    {
        Instantiate(Pref, transform.position, transform.rotation, transform.parent);
    }
}
