using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleport : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //other.gameObject.transform.position = new Vector3(100,100,100);
            other.GetComponent<CharacterController>().Move(new Vector3(256.8753f, 5.28f, 8.19f)-other.transform.position);
            Debug.Log("dd");
        }

    }
}
