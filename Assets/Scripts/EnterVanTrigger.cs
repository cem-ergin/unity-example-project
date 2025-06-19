using UnityEngine;

public class EnterVanTrigger : MonoBehaviour
{
  public GameObject player;
  public GameObject van;
  public VanController vanController;

  public Camera playerCamera;
  public Camera vanCamera;

  private bool playerInRange = true;
  private bool isInVan = false;


  void Start()
  {
    // Start with player camera ON, van camera OFF
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
        // Exit van
        player.transform.position = van.transform.position + Vector3.up * 2f + Vector3.left * 2f;
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