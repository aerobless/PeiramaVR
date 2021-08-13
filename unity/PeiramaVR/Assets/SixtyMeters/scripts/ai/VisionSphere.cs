using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Utils;
using UnityEngine;

public class VisionSphere : MonoBehaviour
{

    public Transform headTransform;
    public Transform aimTargetTransform;
    
    public float visionRadius;
    public float lerpSpeed;

    private PointOfInterest _pointOfInterest;
    private IkControl _ikControl;
    
    // Start is called before the first frame update
    void Start()
    {
        _ikControl = GetComponent<IkControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(headTransform.position + transform.forward, visionRadius);

        _pointOfInterest = null;
        
        foreach (var collider in colliders)
        {
            if (collider.GetComponent<PointOfInterest>())
            {
                _pointOfInterest = collider.GetComponent<PointOfInterest>();
                break;
            }
        }

        Vector3 targetPosition;
        if (_pointOfInterest != null)
        {
            _ikControl.headIkActive = true;
            targetPosition = _pointOfInterest.transform.position;
            
            float speed = lerpSpeed / 10;
            aimTargetTransform.position = Vector3.Lerp(aimTargetTransform.position, targetPosition, Time.deltaTime * speed);
        }
        else
        {
            _ikControl.headIkActive = false;
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(headTransform.position+transform.forward, visionRadius);
    }
}
