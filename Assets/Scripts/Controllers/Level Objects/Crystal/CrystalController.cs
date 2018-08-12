using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CrystalController : MonoBehaviour {
    
    public GameObject laserPrefabRed;
    public GameObject laserPrefabGreen;
    public Vector3 firingPoint;

    public GameObject firingGlow;

    public List<Laser> lasers = new List<Laser>();

    public float maxLaserDistance = 50f;

    void Awake()
    {
        foreach (Laser laser in lasers)
        {
            if(laser.growthDamage >= 0)
                laser.InitLaser(laserPrefabRed, transform);
            else
                laser.InitLaser(laserPrefabGreen, transform);
        }
        
    }
    
    void Update()
    {
        Vector3 startPoint = transform.position + firingPoint;

        bool laserFiring = false;

        foreach (Laser laser in lasers)
            laserFiring = laser.UpdateLaser(startPoint) || laserFiring;

        firingGlow.SetActive(laserFiring);
    }

    [Serializable]
    public class Laser {

        public Vector3 direction;
        public float growthDamage;
        public float meltDamage;
        public float rotationSpeed;
        public float onInterval;
        public float offInterval;
        public float nextToggle;
        public float curRotation = 0;
        public float lastToggle;

        public LaserController laserController { get; private set; }

        private bool isOn = false;

        public void InitLaser(GameObject laserPrefab, Transform parent)
        {
            laserController = Instantiate(laserPrefab, parent).GetComponent<LaserController>();
            laserController.growthDamage = growthDamage;
            laserController.meltDamage = meltDamage;

            lastToggle = Time.time;
            nextToggle += Time.time;
        }

        public bool UpdateLaser(Vector3 startPoint)
        {
            if (onInterval > 0f)
            {
                if (Time.time > nextToggle)
                {
                    if (offInterval > 0f)
                    {
                        isOn = !isOn;
                        lastToggle = Time.time;

                        nextToggle = (isOn) ? lastToggle + onInterval : lastToggle + offInterval;
                    }
                    else
                        isOn = false;
                }
            }
            else
                isOn = true;

            SetActive(isOn);

            if (isOn)
            {
                laserController.FireLaser(startPoint, Quaternion.Euler(0, 0, curRotation) * direction);
            }

            curRotation += rotationSpeed;

            return isOn;
        }

        public void SetActive(bool active)
        {
            laserController.SetActive(active);
            
        }

    }
}