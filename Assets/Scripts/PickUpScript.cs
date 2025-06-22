using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
  public GameObject player;
  public GameObject van;
  public EnterVanTrigger enterVanTrigger;
  public Transform holdPos;
  //if you copy from below this point, you are legally required to like the video
  public float throwForce = 500f; //force at which the object is thrown at
  public float pickUpRange = 15f; //how far the player can pickup the object from
  private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
  private GameObject heldObj; //object which we pick up
  private Rigidbody heldObjRb; //rigidbody of object we pick up
  private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
  private readonly int LayerNumber; //layer index

  public bool IsHoldingObject => heldObj != null; //property to check if player is holding an object
  public Transform trunkDropPoint;


  bool IsOverTrunk()
  {
    RaycastHit hit;
    if (Physics.Raycast(heldObj.transform.position, Vector3.down, out hit, 2f))
    {
      return hit.collider.CompareTag("trunk");
    }
    return false;
  }

  //Reference to script which includes mouse movement of player (looking around)
  //we want to disable the player looking around when rotating the object
  //example below 
  //MouseLookScript mouseLookScript;
  void Start()
  {
    Debug.Log("PickUpScript started");
    // LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
    Debug.Log("LayerNumber set to: " + LayerNumber);

    //mouseLookScript = player.GetComponent<MouseLookScript>();
  }
  void Update()
  {
    Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * pickUpRange, Color.red);

    if (!enterVanTrigger.IsInVan && Input.GetKeyDown(KeyCode.E)) //change E to whichever key you want to press to pick up
    {
      Debug.Log("E key pressed");
      if (heldObj == null) //if currently not holding anything
      {
        Debug.Log("No object held, trying to pick up");

        //perform raycast to check if player is looking at object within pickuprange
        RaycastHit hit;

        bool isHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpRange);

        Debug.Log("Raycast hit: " + hit.transform.name + " with tag: " + hit.transform.tag);

        Debug.Log("Distance to object: " + Vector3.Distance(transform.position, hit.point));
        if (isHit)
        {
          Debug.Log("Raycast hit: " + hit.transform.name);
          //make sure pickup tag is attached
          if (hit.transform.gameObject.tag == "canPickUp")
          {
            Debug.Log("Object can be picked up");
            //pass in object hit into the PickUpObject function
            PickUpObject(hit.transform.gameObject);
          }
        }
      }
      else
      {
        if (canDrop == true)
        {
          StopClipping(); //prevents object from clipping through walls
          DropObject();
        }
      }
    }
    if (heldObj != null) //if player is holding object
    {
      MoveObject(); //keep object position at holdPos
      RotateObject();
      if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
      {
        StopClipping();
        ThrowObject();
      }

    }
  }
  void PickUpObject(GameObject pickUpObj)
  {
    if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
    {
      heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
      heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
      heldObjRb.isKinematic = true;
      heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
                                                      // heldObj.layer = LayerNumber; //change the object layer to the holdLayer
                                                      //make sure object doesnt collide with player, it can cause weird bugs
      Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
      SetVanColliders(true);
    }
  }

  private void SetVanColliders(bool ignoreCollisions = true)
  {
    Collider[] vanColliders = van.GetComponentsInChildren<Collider>();
    foreach (Collider col in vanColliders)
    {
      Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), col, ignoreCollisions);
    }
  }

  void DropObject()
  {
    //re-enable collision with player
    Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

    Debug.Log("Dropping object: " + heldObj.name);
    bool isOverTrunk = IsOverTrunk();
    Debug.Log("IsOverTrunk: " + isOverTrunk);
    if (isOverTrunk)
    {
      // Keep the cube kinematic and stick it to the trunk

      // SetVanColliders(false);
      heldObjRb.isKinematic = true;
      heldObjRb.useGravity = false;

      heldObj.transform.SetParent(van.transform);
      Debug.Log("Parent after drop: " + heldObj.transform.parent.name);
      heldObj.transform.position = trunkDropPoint.position + new Vector3(0, 0.1f, 0); // Slightly raised
      heldObj.transform.rotation = trunkDropPoint.rotation;

    }
    else
    {
      //heldObj.layer = 0; //object assigned back to default layer
      heldObjRb.isKinematic = false;
      heldObj.transform.parent = null; //unparent object
    }

    heldObj = null; //undefine game object
    heldObjRb = null;
  }
  void MoveObject()
  {
    //keep object position the same as the holdPosition position
    heldObj.transform.position = holdPos.transform.position;
  }
  void RotateObject()
  {
    if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
    {
      canDrop = false; //make sure throwing can't occur during rotating

      //disable player being able to look around
      //mouseLookScript.verticalSensitivity = 0f;
      //mouseLookScript.lateralSensitivity = 0f;

      float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
      float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
      //rotate the object depending on mouse X-Y Axis
      heldObj.transform.Rotate(Vector3.down, XaxisRotation);
      heldObj.transform.Rotate(Vector3.right, YaxisRotation);
    }
    else
    {
      //re-enable player being able to look around
      //mouseLookScript.verticalSensitivity = originalvalue;
      //mouseLookScript.lateralSensitivity = originalvalue;
      canDrop = true;
    }
  }
  void ThrowObject()
  {
    //same as drop function, but add force to object before undefining it
    Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);

    SetVanColliders(false);
    //heldObj.layer = 0;
    heldObjRb.isKinematic = false;
    heldObj.transform.parent = null;
    heldObjRb.AddForce(transform.forward * throwForce);
    Debug.Log("Thrown object tag: " + heldObj.tag);

    heldObj = null;
    heldObjRb = null;
  }
  void StopClipping() //function only called when dropping/throwing
  {
    var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
                                                                                      //have to use RaycastAll as object blocks raycast in center screen
                                                                                      //RaycastAll returns array of all colliders hit within the cliprange
    RaycastHit[] hits;
    hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
    //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
    if (hits.Length > 1)
    {
      //change object position to camera position 
      heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
                                                                                    //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
    }
  }
}