using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public Animator spikeTrapAnim; //Animator for the SpikeTrap;
    private bool isPlayerOnTrap = false;
    private bool isFirst = true;
    public float commDelay = 1.0f, commDelayTime;
    public bool  commDelayReal;

    private void Update()
    {
        if (commDelayReal)
        {
            commDelayTime += Time.deltaTime;
            if (commDelayTime > commDelay)
            {
                commDelayReal = false;
            }
        }
    }
    void Awake()
    {

        spikeTrapAnim = transform.GetChild(0).GetChild(0).GetComponent<Animator>();

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")||other.CompareTag("Enemy"))
        {
            commDelayReal = true;

        }
    }

}