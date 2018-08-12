using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitterController : MonoBehaviour {

    public GameObject pickupPrefab;

    private bool notPickedUp = true;

    private void OnTriggerStay(Collider other)
    {
        if (notPickedUp)
        {
            PlayerController player = other.GetComponent<PlayerController>();

            if (player)
            {
                notPickedUp = false;

                gameObject.SetActive(false);

                player.Split();

                Instantiate(pickupPrefab, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }
}
