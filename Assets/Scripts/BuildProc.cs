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

    private void Awake()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 20, LayerMask.GetMask("Road"));
        if (hitColliders.Length != 0)
        {
            float minDistance = float.MaxValue;
            GameObject minObj = null;
            for (int i = 0; i < hitColliders.Length; i++)
            {
                GameObject obj = hitColliders[i].gameObject;
                float distance = Vector3.Distance(obj.transform.position, transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    minObj = obj;
                }
            }
            Vector3 lookAt = minObj.transform.position;
            lookAt.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(lookAt - transform.position);
        }
    }

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
            if (towerScript) towerScript.enabled = true;
            if (fireScript)
                fireScript.enabled = true;
            enabled = false;
            GetComponent<AudioSource>().Play();
        }
    }
}
