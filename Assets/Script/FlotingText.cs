using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlotingText : MonoBehaviour
{
    public float destroyTime = 3f;
    public Vector3 offSet = new Vector3(-1f,2.3f,0);
    public Vector3 randomPos = new Vector3(-1f, 0.5f, 0);

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, destroyTime);
        transform.localPosition += offSet;
        transform.localPosition += new Vector3(Random.Range(randomPos.x, randomPos.x),
            Random.Range(randomPos.y, randomPos.y), Random.Range(randomPos.z, randomPos.z));
    }
}


