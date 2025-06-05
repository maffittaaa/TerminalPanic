using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public enum ItemType { None, Mirror, Weapon, Bullets, Door, KeyCardMachine, KeyCard, Ticket }

public class WorldInteractions : MonoBehaviour
{
    [Header("Interaction Items")]
    [SerializeField] private Weapon weapon;
    [field: SerializeField] public GameObject body { get; set; }
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject flashLightPhysic;
    [SerializeField] private GameObject[] doors;
    [SerializeField] private ItemType itemType;
    [SerializeField] private AnxietyBar anxietyBar;
    [SerializeField] private bool gotKeyCard = false;
    [SerializeField] public bool gotTicket = false;
    [field: SerializeField] public GameObject potencialEnemy { get; private set; }

    [Header("Interaction Settings")]
    [SerializeField] private float distanceToInteract = 5f;
    private GameObject highLightObject;
    private GameObject mirror;
    [SerializeField]  private float lookToMirrorSpeed;
    private InteractableObject interactable;
    [SerializeField] private GameManager gameManager;

    [Header("Door")]
    [SerializeField] private float doorAngleOpen;
    [SerializeField] private float doorAngleClose;
    [SerializeField] private float doorOpenTime;
    [SerializeField] private float doorOpenAddAngle;
    [SerializeField] private float doorOpenSpeed;
    [SerializeField] private float doorCloseSpeed;
    [SerializeField] private float doorCloseAddAngle;
    [SerializeField] private float time = 0f;
    public bool doorOpeningClosing = false;

    [Header("FlashLight")]
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private float flashLightTimeDelay = 0.1f;
    [SerializeField] private float FLmMaxFlickerTimeDist = 10f;
    [SerializeField] private float FLmMinFlickerTimeDist = 20f;
    private AudioManager audioManager;

    [Header("Die")]
    private bool dead = false;
    private PlayerFainting faint;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<AudioManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        faint = FindFirstObjectByType<PlayerFainting>();
        /*        weapon.debug += layerMask.value + " LayerMask\n";
                weapon.debug += layerMask.value.ToString() + " LayerMask\n";*/
    }

    private void Update()
    {
        Interact();
    }    
    
    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        RayCast();
    }

    public void KillPlayer()
    {
        if (!dead)
        {
            dead = true;
            StartCoroutine(faint.KillPlayerOpacity());
        }
    }

    private void RayCast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            interactable = hit.collider.gameObject.GetComponent<InteractableObject>();

            if (interactable != null && hit.distance < distanceToInteract)
            {
                itemType = interactable.GetItemType();

                if (highLightObject != hit.collider.gameObject)
                {
                    if (highLightObject == null)
                    {
                        highLightObject = hit.collider.gameObject;
                        highLightObject.GetComponent<InteractableObject>().ChangeOutlineState(true);
                    }
                    else if (highLightObject != null)
                    {
                        highLightObject.GetComponent<InteractableObject>().ChangeOutlineState(false);
                        highLightObject = hit.collider.gameObject;
                        highLightObject.GetComponent<InteractableObject>().ChangeOutlineState(true);
                    }
                }
            }
            else
            {
                itemType = ItemType.None;

                if (highLightObject != null)
                {
                    highLightObject.GetComponent<InteractableObject>().ChangeOutlineState(false);
                }
                highLightObject = null;
            }

            potencialEnemy = hit.collider.gameObject;
            shootingPoint.transform.position = hit.point;
        }
        else
        {
            itemType = ItemType.None;
            if (highLightObject != null)
            {
                highLightObject.GetComponent<InteractableObject>().ChangeOutlineState(false);
            }
            highLightObject = null;
        }
    }

    private void Interact()
    {
        if (gameManager.state != IGameStates.Paused)
        {
            if (anxietyBar.interacted)
            {
                ChangeCameraView();
                return;
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                switch (itemType)
                {
                    case ItemType.Door:
                        if (!doorOpeningClosing)
                        {
                            doorOpeningClosing = true;
                            StartCoroutine(OpenDoor(interactable.GetItemId()));
                        }
                        break;
                    case ItemType.Ticket:
                        gotTicket = true;
                        Destroy(highLightObject.transform.parent.gameObject);
                        break;
                    case ItemType.Weapon:
                        if (!body.activeSelf)
                        {
                            Destroy(highLightObject);
                            body.SetActive(true);
                            weapon.AddMagBullets(10);
                        }
                        break;
                    case ItemType.Bullets:
                        if(weapon.CurrentBulletsInPocket() < weapon.maxBulletsInPocket)
                        {
                            Destroy(highLightObject);
                            weapon.AddPocketBullets(5);
                        }
                        break;
                    case ItemType.Mirror:
                        if (anxietyBar.interacted == false && anxietyBar.currentAnxiety > 0f && anxietyBar.currentAnxiety < anxietyBar.maxAnxiety)
                        {
                            mirror = highLightObject;
                            anxietyBar.interacted = true;
                            anxietyBar.realityMode = true;
                            anxietyBar.StartCoroutine(anxietyBar.FreezeMovementWhileCalming());
                        }
                        break;
                    case ItemType.KeyCardMachine:
                        if (gotKeyCard && !doorOpeningClosing)
                        {
                            doorOpeningClosing = true;
                            StartCoroutine(OpenDoor(0));
                        }
                        break;
                    case ItemType.KeyCard:
                        gotKeyCard = true;
                        Destroy(highLightObject.transform.parent.gameObject);
                        break;
                }
            }

            if (Input.GetMouseButtonDown(1) && body.activeSelf == true)
            {
                StartCoroutine(FlashLight());
            }
        }        
    }

    private void ChangeCameraView()
    {
        Vector3 parentRotation = mirror.transform.rotation.eulerAngles;

        parentRotation.x = parentRotation.z = 0;

        transform.parent.rotation = Quaternion.Euler(parentRotation);
        transform.localRotation = Quaternion.identity;
    }

    private IEnumerator OpenDoor(int id)
    {
        time = 0;
        while (doors[id].transform.localEulerAngles.y < doorAngleOpen)
        {
            doors[id].transform.localEulerAngles = new Vector3(0, doors[id].transform.localEulerAngles.y + doorAngleOpen * Mathf.Sin(Mathf.Deg2Rad * (time * doorCloseAddAngle)), 0);
            yield return new WaitForSeconds(doorOpenSpeed);
        }
        doors[id].transform.localEulerAngles = new Vector3(0, doorAngleOpen, 0);

        yield return new WaitForSeconds(doorOpenTime);

        time = 0;
        while (doors[id].transform.localEulerAngles.y > doorAngleClose && doors[id].transform.localEulerAngles.y <= doorAngleOpen)
        {
            doors[id].transform.localEulerAngles = new Vector3(0, doors[id].transform.localEulerAngles.y - doorAngleOpen * Mathf.Sin(Mathf.Deg2Rad * (time * doorCloseAddAngle)), 0);
            yield return new WaitForSeconds(doorCloseSpeed);
        }
        doors[id].transform.localEulerAngles = new Vector3(0, doorAngleClose, 0);
        doorOpeningClosing = false;
    }

    private IEnumerator FlashLight()
    {
        if (flashLight.gameObject.activeSelf == true)
        {
            audioManager.flashLightOn.Stop();
            audioManager.flashLightOff.Play();

            flashLight.gameObject.SetActive(false);
            flashLightPhysic.gameObject.SetActive(false);

            yield return new WaitForSeconds(flashLightTimeDelay);

            flashLight.gameObject.SetActive(true);
            flashLightPhysic.gameObject.SetActive(true);

            yield return new WaitForSeconds(flashLightTimeDelay);

            flashLight.gameObject.SetActive(false);
            flashLightPhysic.gameObject.SetActive(false);

            StopAllCoroutines();
        }
        else
        {
            audioManager.flashLightOff.Stop();
            audioManager.flashLightOn.Play();

            flashLight.gameObject.SetActive(true);
            flashLightPhysic.gameObject.SetActive(true);

            yield return new WaitForSeconds(flashLightTimeDelay * 2);

            flashLight.gameObject.SetActive(false);
            flashLightPhysic.gameObject.SetActive(false);

            yield return new WaitForSeconds(flashLightTimeDelay);

            flashLight.gameObject.SetActive(true);
            flashLightPhysic.gameObject.SetActive(true);

            yield return new WaitForSeconds(flashLightTimeDelay);

            flashLight.gameObject.SetActive(false);
            flashLightPhysic.gameObject.SetActive(false);

            yield return new WaitForSeconds(flashLightTimeDelay * 3);

            float maxIntensity = flashLight.gameObject.GetComponent<Light>().intensity;

            flashLight.gameObject.GetComponent<Light>().intensity = 0;

            flashLight.gameObject.SetActive(true);
            flashLightPhysic.gameObject.SetActive(true);

            while (flashLight.gameObject.GetComponent<Light>().intensity < maxIntensity)
            {
                yield return new WaitForSeconds(0.01f);
                flashLight.gameObject.GetComponent<Light>().intensity += 0.3f;
            }
            
            flashLight.gameObject.GetComponent<Light>().intensity = maxIntensity;

            StartCoroutine(FlickerFlashLight());
        }
    }

    private IEnumerator FlickerFlashLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(FLmMinFlickerTimeDist, FLmMaxFlickerTimeDist));
            if(gameManager.state != IGameStates.Paused)
            {
                flashLight.gameObject.SetActive(false);
                flashLightPhysic.gameObject.SetActive(false);

                yield return new WaitForSeconds(flashLightTimeDelay * 2);

                flashLight.gameObject.SetActive(true);
                flashLightPhysic.gameObject.SetActive(true);

                yield return new WaitForSeconds(flashLightTimeDelay);

                flashLight.gameObject.SetActive(false);
                flashLightPhysic.gameObject.SetActive(false);

                yield return new WaitForSeconds(flashLightTimeDelay * 2);

                flashLight.gameObject.SetActive(true);
                flashLightPhysic.gameObject.SetActive(true);
            }
        }
    }
}
