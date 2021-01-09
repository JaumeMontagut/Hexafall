using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Move(Vector3 destination)
    {
        //Move the player to the new hexagon.

        bool ret = false;

        transform.position = new Vector3(destination.x, transform.position.y, destination.z);

        ret = true;


        return ret;
    }
}
