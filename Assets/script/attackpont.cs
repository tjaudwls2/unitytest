using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackpont : MonoBehaviour
{

    public player Player;
    public int damage;

      public enum Char
      {
       Player,Enemy

      }

    public Char character;
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        switch (character)
        {

            case Char.Player:
               
                if (other.CompareTag("Enemy"))
                {
                    if (this.transform.root.GetComponent<player>().isAttackCheck)
                    {
                        // Debug.Log(other.tag);
                        this.transform.root.GetComponent<player>().isAttackCheck = false;
                        other.GetComponent<enemy>().sethp(damage);
                    }
                }
                break;

            case Char.Enemy:
                if (other.CompareTag("Player"))
                {
                    if (this.transform.root.GetComponent<enemy>().isAttackCheck)
                    {
                        //Debug.Log(other.tag);
                        this.transform.root.GetComponent<enemy>().isAttackCheck = false;
                        other.GetComponent<player>().sethp(damage);
                    }


                }





                break;

        }
    }
}
