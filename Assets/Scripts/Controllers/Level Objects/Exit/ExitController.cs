using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitController : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.GetComponent<PlayerController>().isLevelExit = true;
            other.GetComponent<PlayerController>().levelExitPos = transform.parent.position;
        }
    }

}
