using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWorldInteractions : MonoBehaviour
{
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private float distanceToInteract = 5f;

    [SerializeField] private Weapon weapon;
    [SerializeField] private GameObject body;
    [SerializeField] private GameObject testDoor;
    [SerializeField] private ItemType itemType;
    [SerializeField] private AnxietyBar anxietyBar;

    private bool gotKeyCard = false;

    private GameObject highLightObject;
    private Outline outline;

    private LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Default");
    }

    void Update()
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
                    anxietyBar.interacted = true;
                    break;
                case ItemType.KeyCardMachine:
                    if (gotKeyCard)
                    {
                        StartCoroutine(OpenDoor());
                    }
                    break;
                case ItemType.KeyCard:
                    gotKeyCard = true;
                    Destroy(highLightObject.transform.parent.gameObject);
                    break;
            }
        }
    }

    private IEnumerator OpenDoor()
    {
        testDoor.transform.localEulerAngles = new Vector3(0, 60, 0);

        yield return new WaitForSeconds(2f);

        testDoor.transform.localEulerAngles = new Vector3(0, 0, 0);
    }
}
