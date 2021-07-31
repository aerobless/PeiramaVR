using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cleanable : MonoBehaviour
{

	public int secondsUntilClean;

	private float _timePassedInCleaningCycle;
	private bool _isCleaning;
	private Cleaner _cleaner;
	
	// Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void Update()
    {
	    if (_isCleaning)
	    {
		    _timePassedInCleaningCycle += Time.deltaTime;
	    }
	    
	    if (_timePassedInCleaningCycle >= secondsUntilClean)
	    {
		    _cleaner.cleaningEffect.Stop();
		    Destroy(this.transform.parent.gameObject);
	    }
    }
    
    void OnTriggerEnter(Collider col)
    {
	    if (isCleaner(col))
	    {
		    _cleaner = col.gameObject.GetComponent<Cleaner>();
		    _cleaner.cleaningEffect.Play();
		    _isCleaning = true;
	    }
    }

    private static bool isCleaner(Collider col)
    {
	    return col.gameObject.GetComponent<Cleaner>() != null;
    }

    void OnTriggerExit(Collider col)
    {
	    if (isCleaner(col))
	    {
		    col.gameObject.GetComponent<Cleaner>().cleaningEffect.Stop();
		    _timePassedInCleaningCycle = 0;
		    _isCleaning = false;
	    }
    }
}
