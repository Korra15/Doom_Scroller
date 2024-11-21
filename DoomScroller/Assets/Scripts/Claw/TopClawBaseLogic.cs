using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopClawBaseLogic : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject cam;
    public float y;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =new Vector3( transform.position.x, cam.transform.position.y + y, transform.position.z);
    }
}
