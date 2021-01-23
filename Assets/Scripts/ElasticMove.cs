using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElasticMove : MonoBehaviour
{
    bool doAnimation = false;
    bool moveDown = true;
    Vector3 startPosition;
    Vector3 maxDownPos;
    float berpValue = 0;
    float maxDownValue =  0.05f;
    public float verticalSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        maxDownPos = new Vector3(startPosition.x, startPosition.y - maxDownValue, startPosition.z);
    }

    public void StartMoving()
    {
        startPosition = transform.position;
        maxDownPos = new Vector3(startPosition.x, startPosition.y - maxDownValue, startPosition.z);
        doAnimation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(doAnimation)
        {
            if (moveDown)
            {
                transform.position -= Vector3.up * verticalSpeed * Time.deltaTime;
                if (transform.position.y <= maxDownPos.y)
                    moveDown = false;
            }
            else
            {
                berpValue += 1 * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, Berp(maxDownPos.y, startPosition.y, berpValue), transform.position.z);
               if(berpValue >= 1)
                {
                    berpValue = 0;
                    moveDown = true;
                    doAnimation = false;
                }
            }
        }
        
    }
    public static float Berp(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.5f + 3.0f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }
}
