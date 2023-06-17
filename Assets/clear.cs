using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clear : MonoBehaviour
{
    public GameObject clearUI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            //other.gameObject.transform.position = new Vector3(100,100,100);
            clearUI.SetActive(true);

        }

    }
}
