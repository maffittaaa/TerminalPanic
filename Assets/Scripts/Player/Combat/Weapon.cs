using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletsHolder;
    [SerializeField] private GameObject shootingSpawn;
    [SerializeField] private GameObject shootingPoint;

    [SerializeField] private ParticleSystem particleSystem;

    [SerializeField] private int maxBullets;
    [SerializeField] private int currentBullets;
    [SerializeField] private float shootingDelay;
    private float currentShootingDelay = 0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanShoot())
        {
            Shoot();
        }
    }

    void FixedUpdate()
    {
        currentShootingDelay += Time.fixedDeltaTime;
    }

    private bool CanShoot()
    {
        if(currentBullets <= 0 || currentShootingDelay < shootingDelay)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Shoot()
    {
        particleSystem.Play();
        GameObject newBullet = Instantiate(bulletPrefab, shootingSpawn.transform.position, Quaternion.identity);
        newBullet.transform.parent = bulletsHolder.transform;
        newBullet.GetComponent<Bullet>().shootingPoint = shootingPoint.transform.position;
        currentShootingDelay = 0;
        currentBullets--;
    }

    public void AddBullets(int amount)
    {
        currentBullets += amount;
    }

}
