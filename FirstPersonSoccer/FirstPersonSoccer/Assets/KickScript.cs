using UnityEngine;
using System.Collections;

public class KickScript : MonoBehaviour
{

    public Camera cam;

    public float bounceFactor = 0.9f; // Determines how the ball will be bouncing after landing. The value is [0..1]
    public float forceFactor = 10f;
    public float tMax = 5f; // Pressing time upper limit

    private float kickStart; // Keeps time, when you press button
    private float kickForce; // Keeps time interval between button press and release 
    private Vector3 prevVelocity; // Keeps rigidbody velocity, calculated in FixedUpdate()

    void Start()
    {
        Debug.Log("START!");
        //cam = Camera.main;
        //cam = GameObject.Find("Main Camera").GetComponent();
        if (cam == null)
        {
            Debug.Log("cam == null!");
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            kickStart = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(Input.mousePosition);
            if (cam == null)
            {
                Debug.Log("cam == null!");
                return; 
            }
            Debug.Log(cam.GetType().ToString());
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "Ball") // Rename ball object to "Ball" in Inspector, or change name here
                    kickForce = Time.time - kickStart;
            }
        }
    }

    void FixedUpdate()
    {

        if (kickForce != 0)
        {
            float angle = Random.Range(0, 20) * Mathf.Deg2Rad;
            GetComponent<Rigidbody>().AddForce(new Vector3(0.0f,
                                           forceFactor * Mathf.Clamp(kickForce, 0.0f, tMax) * Mathf.Sin(angle),
                                           forceFactor * Mathf.Clamp(kickForce, 0.0f, tMax) * Mathf.Cos(angle)),
                               ForceMode.VelocityChange);
            kickForce = 0;
        }
        prevVelocity = GetComponent<Rigidbody>().velocity;// rigidbody.velocity;

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ground") // Do not forget assign tag to the field
        {
            GetComponent<Rigidbody>().velocity = new Vector3(prevVelocity.x,
                                             -prevVelocity.y * Mathf.Clamp01(bounceFactor),
                                             prevVelocity.z);
        }
    }

}