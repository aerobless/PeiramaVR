using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IkControl : MonoBehaviour
{
    private Animator _animator;

    public Transform lookAtControl = null;
    public Transform rightHandControl = null;
    public Transform leftHandControl = null;

    public bool headIkActive = false;
    public bool rightHandIkActive = false;
    public bool leftHandIkActive = false;

    public float handLerpSpeed;

    private Transform _rightHandFollow = null;
    private bool _rightHandStopFollow = false;

    private Transform _leftHandFollow = null;
    private bool _leftHandStopFollow = false;

    float _elapsedTime = 0;
    private const float TimeReaction = 2f;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_rightHandFollow)
        {
            Vector3 goalPos = _rightHandFollow.position + Vector3.up * 0.3f;
            rightHandControl.position =
                Vector3.Lerp(rightHandControl.position, goalPos, Time.deltaTime * handLerpSpeed);
        }


        if (_leftHandFollow)
        {
            Vector3 goalPos = _leftHandFollow.position + Vector3.up * 0.3f;
            leftHandControl.position =
                Vector3.Lerp(leftHandControl.position, goalPos, Time.deltaTime * handLerpSpeed);
        }
    }

    public void RightHandFollow(Transform gameObject)
    {
        _rightHandFollow = gameObject;
        rightHandIkActive = true;
    }

    public void LeftHandFollow(Transform gameObject)
    {
        _leftHandFollow = gameObject;
        leftHandIkActive = true;
    }

    public void LeftHandStopFollowing()
    {
        //leftHandIkActive = false;
        _leftHandStopFollow = true;
        _leftHandFollow = null;
    }

    public void RightHandStopFollowing()
    {
        //rightHandIkActive = false;
        _rightHandStopFollow = true;
        _rightHandFollow = null;
    }

    void OnAnimatorIK()
    {
        if (_animator)
        {
            // HEAD IK -----------------------------------------------------------------------
            if (headIkActive)
            {
                // Set the look target position, if one has been assigned
                if (lookAtControl != null)
                {
                    _animator.SetLookAtWeight(1);
                    _animator.SetLookAtPosition(lookAtControl.position);
                }
            }
            else
            {
                _animator.SetLookAtWeight(0);
            }

            // RIGHT HAND IK -----------------------------------------------------------------
            if (rightHandIkActive)
            {
                if (rightHandControl != null)
                {
                    _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    _animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandControl.position);
                    _animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandControl.rotation);
                }
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }

            // LEFT HAND IK ------------------------------------------------------------------
            if (leftHandIkActive)
            {
                if (leftHandControl != null)
                {
                    _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                    _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                    _animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandControl.position);
                    _animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandControl.rotation);
                }
            }
            else
            {
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }


            if (_rightHandStopFollow)
            {
                _elapsedTime += Time.deltaTime;
                var weight = Mathf.Lerp(1, 0, _elapsedTime / TimeReaction);
                _animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
                _animator.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
                if (weight <= 0)
                {
                    rightHandIkActive = false;
                }
            }

            if (_leftHandStopFollow)
            {
                _elapsedTime += Time.deltaTime;
                var weight = Mathf.Lerp(1, 0, _elapsedTime / TimeReaction);
                _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
                _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
                if (weight <= 0)
                {
                    leftHandIkActive = false;
                }
            }
        }
    }
}