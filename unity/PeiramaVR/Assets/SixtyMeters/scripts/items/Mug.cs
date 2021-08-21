using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mug : MonoBehaviour
{
    //0 - 100
    public int fillContent = 0;

    public ParticleSystem beerParticles;
    public GameObject drinkContent;
    private MeshRenderer _drinkContentMesh;

    // Start is called before the first frame update
    void Start()
    {
        _drinkContentMesh = drinkContent.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fillContent >= 100)
        {
            beerParticles.Play();
            _drinkContentMesh.enabled = true;
        }
        else
        {
            beerParticles.Stop();
            _drinkContentMesh.enabled = false;
        }
    }

    public void FillMugByIncrement()
    {
        if (fillContent <= 100)
        {
            fillContent += 1;
        }
    }
}