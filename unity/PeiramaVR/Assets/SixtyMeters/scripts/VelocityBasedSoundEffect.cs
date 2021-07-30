using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A sound plays if a configurable amount of velocity is reached.
 */
public class VelocityBasedSoundEffect : MonoBehaviour
{
    private Rigidbody _rigidbody;

    // The amount of velocity that needs to be reached before a sound plays
    public float velocityThreshold;
    public AudioSource audioSource;
    
    // Stop playing the audio if on the next update the velocity drops below the threshold
    public bool stopIfVelocityDrops;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            Debug.LogError("A VelocityBasedSoundEffect can only be applied to a game object with a _rigidbody_");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_rigidbody.velocity.magnitude >= velocityThreshold && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        if (audioSource.isPlaying && stopIfVelocityDrops && _rigidbody.velocity.magnitude < velocityThreshold)
        { // e.g. stops a door from creaking when it's no longer being moved
            audioSource.Stop();
        }
    }
}
