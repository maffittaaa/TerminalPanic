using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private ParticleSystem particleSystems;
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject shootingLight;
    [SerializeField] private GameObject ticketPrefab;
    [SerializeField] private float lightLifeSec;
    [SerializeField] private float shootingDelay;
    private float currentShootingDelay = 0f;

    [field: Header("Magazine")]
    [field: SerializeField] public int maxBulletsInPocket { get; private set; }
    [SerializeField] private int maxBulletsInMag;
    [SerializeField] private int currentBulletsInPocket;
    [SerializeField] private int currentBulletsInMag;
    [SerializeField] private int bulletsShoot;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool reloading;
    [SerializeField] private GameObject magModel;
    
    [Header("Managers and Scripts")]
    [SerializeField] private Animator anim;
    [SerializeField] private WorldInteractions worldInteractions;
    [SerializeField] private BulletCountUI bulletCountUI;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameManager gameManager;

    [Header("Build Debuging")]
    private string _saveFilePath;
    public string debug = "";

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _saveFilePath = Application.persistentDataPath + "/debug.txt";

        bulletCountUI.SetBulletCountText(currentBulletsInMag, maxBulletsInMag);
        bulletCountUI.SetPocketCountText(currentBulletsInPocket, maxBulletsInPocket);
        //print(_saveFilePath);
    }

    private void Update()
    {
        if(gameManager.state != IGameStates.Paused)
        {
            if (Input.GetMouseButtonDown(0) && CanShoot())
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R) && CanReload())
            {
                StartCoroutine(ReloadGun());
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                magModel.SetActive(true);
                anim.SetTrigger("inspectingT");
                anim.SetBool("inspecting", true);
            }
            if (Input.GetKeyUp(KeyCode.G))
            {
                magModel.SetActive(false);
                anim.SetBool("inspecting", false);
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                /*File.WriteAllText(_saveFilePath, debug);*/
            }
        }
    }

    private IEnumerator ReloadGun()
    {
        anim.SetTrigger("reload");
        reloading = true;

        float reloadingBullet = reloadTime / 10;

        bulletsShoot = 0;

        while (reloading) 
        {
            yield return new WaitForSeconds(reloadingBullet);

            if (currentBulletsInMag < maxBulletsInMag && 0 < currentBulletsInPocket)
            {
                currentBulletsInMag++;
                currentBulletsInPocket--;
            }
            else
            {
                reloading = false;
            }
        }

        bulletCountUI.SetBulletCountText(currentBulletsInMag, maxBulletsInMag);
        bulletCountUI.SetPocketCountText(currentBulletsInPocket, maxBulletsInPocket);
    }

    void FixedUpdate()
    {
        currentShootingDelay += Time.fixedDeltaTime;
    }

    private bool CanShoot()
    {
        if(currentBulletsInMag <= 0 || currentShootingDelay < shootingDelay || bulletsShoot >= maxBulletsInMag || reloading)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CanReload()
    {
        if (currentBulletsInPocket <= 0 || maxBulletsInMag <= currentBulletsInMag || reloading)
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
        anim.SetTrigger("shoot");
        StartCoroutine(ShootingLight());
        bulletsShoot++;
        particleSystems.Play();
        audioManager.shoot.Play();
        currentShootingDelay = 0;
        currentBulletsInMag--;
        bulletCountUI.SetBulletCountText(CurrentBulletsInMag(), maxBulletsInMag);
        bulletCountUI.SetPocketCountText(currentBulletsInPocket, maxBulletsInPocket);

        /*        debug += "Shoot\n";
                debug += worldInteractions.potencialEnemy.transform.parent.name + "\n";
                debug += worldInteractions.potencialEnemy.name + "\n";
                debug += worldInteractions.potencialEnemy.tag + "\n";*/


        if (worldInteractions.potencialEnemy)
        {
            TravelerAI enemy = worldInteractions.potencialEnemy.GetComponent<TravelerAI>();
    
            if (enemy)
            {
                StartCoroutine(enemy.Die());
            }
            else if (worldInteractions.potencialEnemy.CompareTag("Thief"))
            {
                ThiefBehavior thief = worldInteractions.potencialEnemy.GetComponent<ThiefBehavior>();

                Instantiate(ticketPrefab, worldInteractions.potencialEnemy.transform.position, ticketPrefab.transform.rotation);

                StartCoroutine(thief.Die());
            }
        }
    }

    private IEnumerator ShootingLight()
    {
        shootingLight.SetActive(true);
        yield return new WaitForSeconds(lightLifeSec);
        shootingLight.SetActive(false);
    }

    public void AddMagBullets(int amount)
    {
        currentBulletsInMag = Mathf.Clamp(currentBulletsInMag + amount, 0, maxBulletsInMag);
        bulletCountUI.SetBulletCountText(CurrentBulletsInMag(), maxBulletsInMag);
        bulletCountUI.SetPocketCountText(currentBulletsInPocket, maxBulletsInPocket);
    }

    public void AddPocketBullets(int amount)
    {
        currentBulletsInPocket = Mathf.Clamp(currentBulletsInPocket + amount, 0, maxBulletsInPocket);
        bulletCountUI.SetBulletCountText(CurrentBulletsInMag(), maxBulletsInMag);
        bulletCountUI.SetPocketCountText(currentBulletsInPocket, maxBulletsInPocket);
    }

    public int CurrentBulletsInMag()
    {
        return currentBulletsInMag;
    }

    public int CurrentBulletsInPocket()
    {
        return currentBulletsInPocket;
    }

    public int TotalStoredBullets()
    {
        return currentBulletsInMag + currentBulletsInPocket;
    }
}