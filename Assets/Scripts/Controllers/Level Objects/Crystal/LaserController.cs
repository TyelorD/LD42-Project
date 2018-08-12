using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    public LineRenderer lineRenderer;
    public GameObject impact;

    public float growthDamage = 0.01f;
    public float meltDamage = 0.0001f;

    public float maxDistance = 100f;

    public ParticleSystem particleSpray;
    public ParticleSystem particleGlow;
    public GameObject light;

    private void Start()
    {

    }

    public void SetLaserStart(Vector3 startPoint)
    {
        lineRenderer.SetPosition(0, startPoint);
    }

    public void SetLaserEnd(Vector3 endPoint, Vector3 hitNormal)
    {
        lineRenderer.SetPosition(1, endPoint);

        float angle = Mathf.Atan2(hitNormal.y, hitNormal.x) * Mathf.Rad2Deg;

        impact.transform.SetPositionAndRotation(endPoint, Quaternion.AngleAxis(angle, Vector3.forward));
    }

    public void FireLaser(Vector3 startPoint, Vector3 direction)
    {
        SetLaserStart(startPoint);

        RaycastHit hit;
        if (Physics.Raycast(startPoint, direction, out hit, maxDistance, LayerMask.GetMask("Default")))
        {
            if (hit.collider)
            {
                PlayerController player = hit.collider.GetComponent<PlayerController>();
                IceCubeController iceCube = hit.collider.GetComponent<IceCubeController>();

                if (player)
                    player.Grow(growthDamage);

                if (iceCube)
                    iceCube.Shrink(meltDamage);

                SetLaserEnd(hit.point, hit.normal);
                SetActiveParticles(true);
            }
        }
        else
        {
            SetLaserEnd(transform.forward * maxDistance, Vector3.zero);

            SetActiveParticles(false);
        }
    }

    public void SetActive(bool active)
    {
        SetActiveParticles(active);

        lineRenderer.enabled = active;
        //gameObject.SetActive(active);
    }

    public void SetActiveParticles(bool active)
    {
        particleGlow.gameObject.SetActive(active);
        light.SetActive(active);

        if (active && !particleSpray.isPlaying)
        {
            particleSpray.Play();
        }
        else if(!active && !particleSpray.isPaused)
        {
            particleSpray.Stop();
        }
    }

}
