using UnityEngine;

public class VanFollowCamera : MonoBehaviour
{
  public Transform target;       // The van
  public Vector3 offset = new Vector3(0, 3, -6);
  public float smoothSpeed = 5f;

  void LateUpdate()
  {
    if (target == null) return;

    // Offset relative to the van's local space (behind it)
    Vector3 desiredPosition = target.position + target.TransformDirection(offset) + Vector3.left * 5f;
    transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

    // Look at the van
    transform.LookAt(target);
  }
}