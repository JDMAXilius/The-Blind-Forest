using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cam;
    [SerializeField] Transform location;
    [SerializeField] float lerpAmount;
    [SerializeField] GameObject bossHealth;
    bool lerp;
    bool reverse;
    float initDist;
    private void FixedUpdate()
    {
        if (lerp) {
            if (!reverse)
            {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, location.position, lerpAmount);
                if (Vector3.Distance(cam.transform.position, location.position) < .05f)
                {
                    lerp = false;
                    cam.transform.position = location.position;
                    reverse = true;
                }
            }
            else {
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, Gamemanager.Instance.player.transform.position, lerpAmount);
                if (Vector3.Distance(cam.transform.position, Gamemanager.Instance.player.transform.position) < initDist)
                {
                    lerp = false;
                    cam.Follow = Gamemanager.Instance.player.transform;
                    reverse = false;
                }
            }
        }

        if (!bossHealth.activeSelf && reverse) {
            lerp = true;
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            initDist = Vector3.Distance(Gamemanager.Instance.player.transform.position, cam.transform.position);       
            cam.Follow = null;
            lerp = true;
            bossHealth.SetActive(true);
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
