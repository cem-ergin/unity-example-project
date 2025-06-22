using UnityEngine;

public class EnterVanTrigger : MonoBehaviour
{
  public GameObject player;
  public GameObject van;
  public VanController vanController;
  public PlayerMovement playerMovement;
  public PickUpScript pickUpScript;

  public Camera playerCamera;
  public Camera vanCamera;
  public GameObject interactionPrompt;

  private bool playerInRange = false;
  private bool isInVan = false;
  public float triggerDistance = 3f;

  public bool IsInVan => isInVan;


  void Start()
  {
    playerCamera.enabled = true;
    vanCamera.enabled = false;
    interactionPrompt.SetActive(false);
  }

  void Update()
  {
    if (pickUpScript.IsHoldingObject)
    {
      // If the player is holding an object, they cannot enter the van
      return;
    }

    float distance = Vector3.Distance(van.transform.position, player.transform.position);
    playerInRange = distance <= triggerDistance;

    bool lookingAtVan = true;
    // Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
    // if (Physics.Raycast(ray, out RaycastHit hit, triggerDistance))
    // {
    //   if (hit.collider.gameObject == van)
    //   {
    //     lookingAtVan = true;
    //   }
    // }


    if (playerInRange && lookingAtVan && !isInVan)
    {
      interactionPrompt.SetActive(true);
    }
    else
    {
      interactionPrompt.SetActive(false);
    }

    if (((playerInRange && lookingAtVan) || isInVan) && Input.GetKeyDown(KeyCode.E))
    {
      isInVan = !isInVan;

      if (isInVan)
      {
        // Enter van
        player.SetActive(false);
        vanController.SetDriving(true);

        playerCamera.enabled = false;
        vanCamera.enabled = true;
      }
      else
      {
        player.transform.position = van.transform.position + Vector3.up * 2f - van.transform.right * 2f;
        player.transform.rotation = van.transform.rotation;
        playerMovement.AlignWithRotation(van.transform.rotation);


        player.SetActive(true);
        vanController.SetDriving(false);

        playerCamera.enabled = true;
        vanCamera.enabled = false;
      }
    }
  }

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject == player)
      playerInRange = true;
  }

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject == player)
      playerInRange = false;
  }
}