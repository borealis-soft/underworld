using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProc : MonoBehaviour
{
    [SerializeField]
    private Tower towerScript;
    [SerializeField]
    private TowerFire fireScript;
    [SerializeField]
    private Material prevMaterial;

    public float timeToBuild;
    public int TowerCost;
    public AudioClip[] buildingAudios;
    public string Title;
    public Sprite SpritePrev;

    private float time;
    private MeshRenderer[] baseRenderers;
    private Material[] originalMaterials;

    void Start()
    {
        baseRenderers = towerScript.baseRenderers;
        originalMaterials = new Material[baseRenderers.Length];
        for (int i = 0; i < baseRenderers.Length; i++)
        {
            originalMaterials[i] = baseRenderers[i].material;
            baseRenderers[i].material = prevMaterial;
        }

        time = timeToBuild;
        for (int i = 0; i < buildingAudios.Length; i++)
            GetComponent<AudioSource>().PlayOneShot(buildingAudios[i]);
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            for (int i = 0; i < baseRenderers.Length; i++)
                baseRenderers[i].material = originalMaterials[i];
            GetComponent<AudioSource>().Stop();
            towerScript.enabled = true;
            fireScript.enabled = true;
            enabled = false;
            GetComponent<AudioSource>().Play();
        }
    }
}
