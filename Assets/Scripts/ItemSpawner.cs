using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
  public GameObject itemPrefab;
  public Transform backAnchor;

  public void AddItemToBack()
  {
    Debug.Log("Adding item to back");
    GameObject newItem = Instantiate(itemPrefab, backAnchor.position, backAnchor.rotation);
    newItem.transform.SetParent(backAnchor);
  }
}