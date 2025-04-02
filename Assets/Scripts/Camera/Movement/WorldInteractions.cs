using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;
public enum ItemType { None, Mirror, Weapon, Bullets, KeyCardMachine, KeyCard, Ticket }

public class WorldInteractions : MonoBehaviour
{
    [Header("Interaction Items")]
    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private GameObject flashLightPhysic;
    [SerializeField] private GameObject testDoor;
    [SerializeField] private ItemType itemType;
    [SerializeField] private AnxietyBar anxietyBar;
    private bool gotKeyCard = false;
    [field: SerializeField] public GameObject potencialEnemy { get; private set; }

    [Header("Interaction Settings")]
    [SerializeField] private float distanceToInteract = 5f;
    private GameObject highLightObject;
    private Outline outline;
    private LayerMask layerMask;

    [Header("Door")]
    [SerializeField] private float doorAngleOpen;
    [SerializeField] private float doorAngleClose;
    [SerializeField] private float doorOpenTime;
    [SerializeField] private float doorOpenAddAngle;
    [SerializeField] private float doorOpenSpeed;
    [SerializeField] private float doorCloseSpeed;
    [SerializeField] private float doorCloseAddAngle;
    [SerializeField] private float time = 0f;
    private bool doorOpeningClosing = false;
    [SerializeField] private bool flashLightOnOff = false;


    [Header("FlashLight")]
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private float flashLightTimeDelay = 0.1f;
    [SerializeField] private float FLmMaxFlickerTimeDist = 10f;
    [SerializeField] private float FLmMinFlickerTimeDist = 20f;


    private void Start()
    {
        layerMask = LayerMask.GetMask("Default");
    }

    private void FixedUpdate()
    {
        time += Time.fixedDeltaTime;
        RayCast();
    }

    private void RayCast()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

            outline = hit.collider.gameObject.GetComponent<Outline>();

            if (outline != null && hit.distance < distanceToInteract)
            {
                itemType = outline.GetItemType();

                if (highLightObject != hit.collider.gameObject)
                {
                    if (highLightObject == null)
                    {
                        highLightObject = hit.collider.gameObject;
                        highLightObject.GetComponent<Outline>().enabled = true;
                    }
                    else if (highLightObject != null)
                    {
                        highLightObject.GetComponent<Outline>().enabled = false;
                        highLightObject = hit.collider.gameObject;
                        highLightObject.GetComponent<Outline>().enabled = true;
                    }
                }
            }
            else
            {
                itemType = ItemType.None;

                if (highLightObject != null)
                {
                    highLightObject.GetComponent<Outline>().enabled = false;
                }
                highLightObject = null;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            hit.point = transform.TransformDirection(Vector3.forward) * 1000;
        }

        if(hit.collider.gameObject != null)
            potencialEnemy = hit.collider.gameObject;

        shootingPoint.transform.position = hit.point;

        Interact();
    }

    private void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                    Destroy(highLightObject);
                    body.SetActive(true);
                    weapon.AddBullets(10);
                    break;
                case ItemType.Bullets:
                    Destroy(highLightObject.transform.parent.gameObject);
                    weapon.AddBullets(5);
                    break;
                case ItemType.Mirror:
                    if (anxietyBar.interacted == false)
                    {
                        anxietyBar.interacted = true;
                        anxietyBar.StartCoroutine(anxietyBar.FreezeMovementWhileCalming());
                    }
                    break;
                case ItemType.KeyCardMachine:
                    if (gotKeyCard && !doorOpeningClosing)
                    {
                        doorOpeningClosing = true;
                        StartCoroutine(OpenDoor());
                    }
                    break;
                case ItemType.KeyCard:
                    gotKeyCard = true;
                    Destroy(highLightObject.transform.parent.gameObject);
                    break;
            }
        }

        if (Input.GetMouseButtonDown(1) && !flashLightOnOff)
        {
            flashLightOnOff = true;
            StartCoroutine(FlashLight());
        }
    }

    private IEnumerator OpenDoor()
    {
        time = 0;
        while (testDoor.transform.localEulerAngles.y < doorAngleOpen)
        {
            testDoor.transform.localEulerAngles = new Vector3(0, testDoor.transform.localEulerAngles.y + doorAngleOpen * Mathf.Sin(Mathf.Deg2Rad * (time * doorCloseAddAngle)), 0);
            yield return new WaitForSeconds(doorOpenSpeed);
        }
        testDoor.transform.localEulerAngles = new Vector3(0, doorAngleOpen, 0);

        yield return new WaitForSeconds(doorOpenTime);

        time = 0;
        while (testDoor.transform.localEulerAngles.y > doorAngleClose && testDoor.transform.localEulerAngles.y <= doorAngleOpen)
        {
            testDoor.transform.localEulerAngles = new Vector3(0, testDoor.transform.localEulerAngles.y - doorAngleOpen * Mathf.Sin(Mathf.Deg2Rad * (time * doorCloseAddAngle)), 0);
            yield return new WaitForSeconds(doorCloseSpeed);
        }
        testDoor.transform.localEulerAngles = new Vector3(0, doorAngleClose, 0);
        doorOpeningClosing = false;
    }

    private IEnumerator FlashLight()
    {
        if (flashLight.gameObject.activeSelf == true)
        {
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
        flashLightOnOff = false;
    }

    private IEnumerator FlickerFlashLight()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(FLmMinFlickerTimeDist, FLmMaxFlickerTimeDist));

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
