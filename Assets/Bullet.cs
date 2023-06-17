using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10;
    public float endTime = 2f;
    public float lifeTime = 0f;
    public int damage = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime += Time.deltaTime;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (lifeTime >= endTime)
        {
            lifeTime = 0;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {

            enemy Enemy = other.GetComponent<enemy>();
            Enemy.sethp(damage);
            Destroy(this.gameObject);
        }

    }
}
