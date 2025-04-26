using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WorldInteractions worldInteractions;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject shootingLight;
    [SerializeField] private float lightLifeSec;
    [SerializeField] private ParticleSystem particleSystems;
    [SerializeField] private int maxBullets;
    [SerializeField] private int currentBullets;
    [SerializeField] private int bulletsShoot;
    [SerializeField] private float shootingDelay;
    [SerializeField] private AudioManager audioManager;
    private float currentShootingDelay = 0f;

    [SerializeField] private GameObject ticketPrefab;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
    }

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
        audioManager.shoot.Play();
        currentShootingDelay = 0;
        currentBullets--;
        if (worldInteractions.potencialEnemy.CompareTag("Enemy"))
        {
            Destroy(worldInteractions.potencialEnemy);
        }
        else if (worldInteractions.potencialEnemy.CompareTag("Thief"))
        {
            Instantiate(ticketPrefab, worldInteractions.transform.position, ticketPrefab.transform.rotation);
            Destroy(worldInteractions.potencialEnemy);
        }
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