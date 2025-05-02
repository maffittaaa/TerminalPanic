using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    private string _saveFilePath;
    [SerializeField] private GameManager gameManager;
    public string debug = "";

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _saveFilePath = Application.persistentDataPath + "/debug.txt";
        print(_saveFilePath);
    }

    private void Update()
    {
        if(gameManager.state != IGameStates.Paused)
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
                /*File.WriteAllText(_saveFilePath, debug);*/
            }
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

        debug += "Shoot\n";
        debug += worldInteractions.potencialEnemy.transform.parent.name + "\n";
        debug += worldInteractions.potencialEnemy.name + "\n";
        debug += worldInteractions.potencialEnemy.tag + "\n";

        if (worldInteractions.potencialEnemy)
        {
            if (worldInteractions.potencialEnemy.GetComponent<TravelerAI>())
            {
                Destroy(worldInteractions.potencialEnemy);
            }
            else if (worldInteractions.potencialEnemy.transform.parent.GetComponent<TravelerAI>())
            {
                Destroy(worldInteractions.potencialEnemy.transform.parent.gameObject);
            }
            else if (worldInteractions.potencialEnemy.CompareTag("Thief"))
            {
                Instantiate(ticketPrefab, worldInteractions.potencialEnemy.transform.position, ticketPrefab.transform.rotation);
                Destroy(worldInteractions.potencialEnemy);
            }
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