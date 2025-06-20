using UnityEngine;

public class EnterVanTrigger : MonoBehaviour
{
  public GameObject player;
  public GameObject van;
  public VanController vanController;
  public PlayerMovement playerMovement;

  public Camera playerCamera;
  public Camera vanCamera;

  private bool playerInRange = true;
  private bool isInVan = false;


  void Start()
  {
    playerCamera.enabled = true;
    vanCamera.enabled = false;
  }

  void Update()
  {
    if (playerInRange && Input.GetKeyDown(KeyCode.E))
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