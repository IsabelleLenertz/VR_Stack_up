using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxStacking : MonoBehaviour {

    private SteamVR_TrackedObject trackedObj;   // to get a handle of the controller
    private GameObject collidingObject;         // store a reference to the colliding object
    private GameObject objectInHand;            // store a reference to the object hold with the controller
    public GameObject boxPrefab;                // reference to the box prefab to create
    public GameObject camera;                // reference to the player's position
    private const float speed = 0.05f;

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
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touchpad = Controller.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0);
            if(touchpad.y > 0.6f)
            {
                // Move up
                Vector3 position = camera.transform.position;
                position.y += speed;
                camera.transform.position = position;
            } else if(touchpad.y < -0.6f)
            {
                // Move Down
                Vector3 position = camera.transform.position;
                if (position.y > speed)
                    position.y -= speed;
                else
                    position.y = 0;
                camera.transform.position = position;

            }
        }

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
                collidingObject = box;
                GrabObject();
            }
        }
        else if (Controller.GetHairTriggerUp())
        {
            ReleaseObject();
        }
    }

    // Setting the colliding object if appropriate
    private void SetCollidingObject(Collider col)
    {
        // if there is no colliding object yet stored and the argument has a rigidbody (ie, can be interacted with)
        if (!collidingObject && col.GetComponent<Rigidbody>())
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
        objectInHand.gameObject.tag = "CarriedBox";

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
            //this.GetComponent<FixedJoint>().connectedBody = null;   // not sure this line is needed
            Destroy(this.GetComponent<FixedJoint>());

            // Set appropriate velocity
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
            objectInHand.gameObject.tag = "Box";

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
