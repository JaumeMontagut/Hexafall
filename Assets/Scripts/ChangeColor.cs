using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    Color myColor;
    Material []materials;
   float intensity = 50.0f;
    // Start is called before the first frame update

    void Start()
    {
        myColor = GetComponent<Image>().color;
       
        Renderer renderer = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Renderer>();
        materials = renderer.materials;
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangePlayerColor()
    {
        materials[0].SetColor("_EmissionColor",myColor * intensity);
        materials[1].SetColor("_EmissionColor",myColor * intensity);
    }
}
