using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InnCustomerAI : MonoBehaviour
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _nextCheck;
    private GameObject _nextTarget;

    public GameObject waypoint1;
    public GameObject waypoint2;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _nextCheck = Time.time;
        _nextTarget = waypoint1;
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {

        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = true;

        transform.position = new Vector3(transform.position.x, _navMeshAgent.nextPosition.y, transform.position.z);
        _navMeshAgent.nextPosition = transform.position;

        if (targetReached())
        {
            _animator.SetBool("Walk", false);
        }
        else
        {
            _animator.SetBool("Walk", true);
            _navMeshAgent.SetDestination(_nextTarget.transform.position);  
        }

        if (Time.time > _nextCheck)
        {
            NextCheckInSeconds(10);
            _nextTarget = _nextTarget == waypoint1 ? waypoint2 : waypoint1;
        }
    }

    private bool targetReached()
    {
        return Vector3.Distance(transform.position, _nextTarget.transform.position) <= 2;
    }

    //Between 0 - 1
    private float GetNavMeshVelocity()
    {
        return _navMeshAgent.velocity.magnitude / _navMeshAgent.speed;
    }

    private void NextCheckInSeconds(float seconds)
    {
        _nextCheck = Time.time + seconds;
    }
}