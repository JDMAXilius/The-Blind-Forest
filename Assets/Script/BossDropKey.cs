using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDropKey : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= 1.25f)
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = new Vector3(transform.localPosition.x, 1.25f, 0);
            Destroy(this);
        }
    }
}
