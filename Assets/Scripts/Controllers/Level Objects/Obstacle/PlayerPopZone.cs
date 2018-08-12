using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPopZone : MonoBehaviour {

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<PlayerController>())
            other.GetComponent<PlayerController>().Pop();
    }

}
