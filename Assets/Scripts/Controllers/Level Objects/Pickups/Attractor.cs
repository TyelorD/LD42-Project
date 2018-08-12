using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour {

    public float dampTime = 3f;
    public float maxVelocity = 1f;
    public float acceleration = 0.1f;

    private Vector3 _curVelocity = new Vector3();
    private Transform curTarget;

    private void OnTriggerEnter(Collider other)
    {
        if(curTarget == null && other.GetComponent<PlayerController>())
        {
            curTarget = other.transform;

            //Debug.Log("Player Enter");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (curTarget && other.GetComponent<PlayerController>())
        {
            curTarget = null;
            _curVelocity = Vector3.zero;

            //Debug.Log("Player Exit");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(curTarget)
        {
            transform.parent.position = Vector3.SmoothDamp(transform.position, curTarget.position, ref _curVelocity, dampTime, maxVelocity, acceleration);

            //Debug.Log("Player Stay");
        }
    }

}
