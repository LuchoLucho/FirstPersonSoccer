using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool duckActionActive = false;
    private float totalTime = 1.0f * 0.7f / 2;
    private float t = 0.0f;
    private float vy = 2.5f;

    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f*2;

        float y = 0.0f;

        if (Input.GetKeyDown("space"))
        {
            if (!duckActionActive)
            {
                duckActionActive = true;
                t = 0.0f;
            }
            //y += Time.deltaTime * 150.0f;
        }

        if (Input.GetKeyUp("space"))
        {
            if (!duckActionActive)
            {
                duckActionActive = true;
                t = 0.0f;
            }
            //y += Time.deltaTime * 150.0f;
        }

        if (duckActionActive)
        {
            if (t > totalTime)
            {
                t = 0;
                duckActionActive = false;
            }
            else
            {
                /*if (t < totalTime / 2)
                {
                    y -= vy * Time.deltaTime;                    
                }
                else
                {
                    y += vy * Time.deltaTime;
                }*/
                y -= vy * Time.deltaTime;
                t += Time.deltaTime;
            }
        }

        transform.Rotate(0, x, 0);
        transform.Translate(0, y, z);        
    }
}
