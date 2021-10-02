using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private string requiredInventoryItemString;
    [SerializeField] private Animator animator;
    bool paused;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        HUDScript.pauseGame += Pause;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //debug.log("I'm triggered by the Player");
            
            if (Gamemanager.Instance.playerScript.inventory.ContainsKey(requiredInventoryItemString))
            {
                //Gamemanager.Instance.playerScript.RemoveInventoryItem(requiredInventoryItemString);
                animator.SetTrigger("open");
            }
        }
    }

    public void Pause()
    {
        paused = !paused;
        if (paused)
        {
            animator.enabled = false;
        }
        else
        {
            animator.enabled = true;
        }
    }
    private void OnDestroy()
    {
        HUDScript.pauseGame -= Pause;
    }
}
