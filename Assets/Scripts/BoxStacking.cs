using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStacking : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;   // to get a handle of the controller
    private GameObject collidingObject;         // store a reference to the colliding object
    private GameObject objectInHand;            // store a reference to the object hold with the controller
    public GameObject boxPrefab;                // reference to the box prefab to create

    private SteamVR_Controller.Device Controller        // Reference to the controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        // Get a handle of the controller
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                GrabObject();
            }
            else
            {
                // Create a new box
                GameObject box = Instantiate<GameObject>(boxPrefab);
                box.transform.position = this.transform.position;
                GrabObject();
            }
        }
        else if (Controller.GetHairTriggerUp() && objectInHand)
        {
            ReleaseObject();
        }
    }

    // Setting the colliding object if appropriate
    private void SetCollidingObject(Collider col)
    {
        // if there is no colliding object yet stored and the argument has a rigidbody (ie, can be interacted with)
        if (!collidingObject || col.GetComponent<Rigidbody>())
        {
            // set the argument as colliding object
            collidingObject = col.gameObject;
        }
    }

    // Create a link between the object coliding and the controller
    // precondition: collidingObject be != null
    // postcondition: objectInHand set, collidingObject = null;
    private void GrabObject()
    {
        // Consider the colliding object as hold
        objectInHand = collidingObject;
        collidingObject = null;

        // Create a strong link between the object and the controller
        FixedJoint fx = this.gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        fx.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    // Release the link between the controller and the object in hand
    private void ReleaseObject()
    {
        // if there is a link between the object in hand and the controller
        if (this.GetComponent<FixedJoint>())
        {
            // Remove the link
            this.GetComponent<FixedJoint>().connectedBody = null;   // not sure this line is needed
            Destroy(this.GetComponent<FixedJoint>());

            // Set appropriate velocity
            objectInHand.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = this.GetComponent<Rigidbody>().angularVelocity;

            // remove the reference
            objectInHand = null;
        }

    }

    // When another collising box enters the controller's colliding box
    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }
    // When another collising box stays in the controller's colliding box
    public void OnTriggerStay(Collider other)
    {
        SetCollidingObject(other);
    }
    // When another collising box exits the controller's colliding box
    public void OnTriggerExit(Collider other)
    {
        // Delete reference to it
        collidingObject = null;
    }

}
