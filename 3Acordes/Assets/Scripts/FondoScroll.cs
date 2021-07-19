using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FondoScroll : MonoBehaviour
{
    public float Speed = -0.5f * 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + new Vector3(Speed * Time.deltaTime, 0, 0);
    }
}
