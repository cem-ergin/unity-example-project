using UnityEngine;

public class VanController : MonoBehaviour
{
  public float speed = 10f;
  public float turnSpeed = 50f;

  private bool isDriving = false;

  public void SetDriving(bool driving)
  {
    isDriving = driving;
  }

  void Update()
  {
    if (!isDriving) return;

    float move = Input.GetAxis("Vertical") * speed * Time.deltaTime;
    float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

    transform.Translate(Vector3.forward * move);
    transform.Rotate(Vector3.up * turn);
  }
}