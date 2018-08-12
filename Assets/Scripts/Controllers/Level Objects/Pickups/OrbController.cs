using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour {

    public float growthAmount = 0.25f;
    public float growthRate = 0.01f;

    public GameObject pickupPrefab;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if (player)
        {
            player.StartCoroutine(player.GrowOverTime(growthAmount / growthRate, growthRate));

            Instantiate(pickupPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
    }

}
