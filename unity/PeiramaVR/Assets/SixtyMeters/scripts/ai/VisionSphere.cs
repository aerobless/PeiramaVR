using System;
using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core.Utils;
using SixtyMeters.scripts.items;
using UnityEngine;

public class VisionSphere : MonoBehaviour
{
    public Transform headTransform;
    public Transform headTransformDefault;

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
        if (headTransform)
        {
            Collider[] colliders = Physics.OverlapSphere(headTransform.position + transform.forward, visionRadius);

            _pointOfInterest = null;

            foreach (var collider in colliders)
            {
                if (collider.GetComponent<PointOfInterest>())
                {
                    if (!(collider.GetComponent<UsableByNpc>() && collider.GetComponent<UsableByNpc>().isEquipped))
                    {
                        _pointOfInterest = collider.GetComponent<PointOfInterest>();
                        break;
                    }
                }
            }

            Vector3 targetPosition;
            float speed = lerpSpeed / 10;
            if (_pointOfInterest != null)
            {
                _ikControl.headIkActive = true;
                targetPosition = _pointOfInterest.transform.position;

                aimTargetTransform.position =
                    Vector3.Lerp(aimTargetTransform.position, targetPosition, Time.deltaTime * speed);
            }
            else
            {
                aimTargetTransform.position =
                    Vector3.Lerp(aimTargetTransform.position, headTransformDefault.transform.position,
                        Time.deltaTime * speed);
                StartCoroutine(TurnOffHeadIk());
            }
        }
    }

    IEnumerator TurnOffHeadIk()
    {
        //Gives the system enough time to return to neutral head position before turning IK off
        yield return new WaitForSeconds(3);
        if (_pointOfInterest == null)
        {
            _ikControl.headIkActive = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        if (headTransform)
        {
            Gizmos.DrawWireSphere(headTransform.position + transform.forward, visionRadius);
        }
    }
}