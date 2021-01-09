using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject[] adjacentPlatforms;
    public bool isPath = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        if (isPath)
        {
            Gizmos.color = Color.yellow;
            Vector3 gizmosPos = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            Gizmos.DrawSphere(gizmosPos, 1);
        }
    }
}
