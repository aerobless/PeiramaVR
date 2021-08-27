using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Components;
using UnityEngine;

public class BeerLever : MonoBehaviour
{
    public ParticleSystem beerParticles;
    public AudioSource pouringSound;

    public bool isDispensing = false;

    private HVRRotationTracker _rotationTracker;

    // Start is called before the first frame update
    void Start()
    {
        _rotationTracker = GetComponent<HVRRotationTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rotationTracker.ClampedAngle > 120f)
        {
            beerParticles.Play();

            if (!pouringSound.isPlaying)
            {
                pouringSound.Play();
            }

            isDispensing = true;
        }
        else
        {
            beerParticles.Stop();
            pouringSound.Stop();
            isDispensing = false;
        }
    }
}