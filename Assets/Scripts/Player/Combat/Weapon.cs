using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject bulletsHolder;
    [SerializeField] private GameObject shootingSpawn;
    [SerializeField] private GameObject shootingPoint;

    [SerializeField] private GameObject shootingLight;
    [SerializeField] private float lightLifeSec;

    [SerializeField] private ParticleSystem particleSystems;

    [SerializeField] private int maxBullets;
    [SerializeField] private int currentBullets;
    [SerializeField] private int bulletsShoot;
    [SerializeField] private float shootingDelay;
    private float currentShootingDelay = 0f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanShoot())
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            bulletsShoot = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Debug.Log("Bullets in mag: " + CurrentBulletsInMag());
            Debug.Log("Total bullets stored: " + TotalStoredBullets());
        }
    }

    void FixedUpdate()
    {
        currentShootingDelay += Time.fixedDeltaTime;
    }

    private bool CanShoot()
    {
        if(currentBullets <= 0 || currentShootingDelay < shootingDelay || bulletsShoot >= maxBullets)
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
        StartCoroutine(ShootingLight());
        bulletsShoot++;
        particleSystems.Play();
        GameObject newBullet = Instantiate(bulletPrefab, shootingSpawn.transform.position, Quaternion.identity);
        newBullet.transform.parent = bulletsHolder.transform;
        newBullet.GetComponent<Bullet>().shootingPoint = shootingPoint.transform.position;
        currentShootingDelay = 0;
        currentBullets--;
    }

    private IEnumerator ShootingLight()
    {
        shootingLight.SetActive(true);
        yield return new WaitForSeconds(lightLifeSec);
        shootingLight.SetActive(false);
    }

    public void AddBullets(int amount)
    {
        currentBullets += amount;
    }

    public int CurrentBulletsInMag()
    {
        return maxBullets - bulletsShoot;
    }

    public int TotalStoredBullets()
    {
        return currentBullets - CurrentBulletsInMag();
    }
}