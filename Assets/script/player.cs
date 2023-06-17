using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class player : MonoBehaviour
{
    public float traptime;


    public int bulletNum;
    public GameObject[] heart;
    public GameObject Gun, bat;

    CharacterController characterController;
    public float speed = 2.0f;
    Animator animator;
    bool isWalk = false;
    public bool isAttackCheck;
    public int hp = 3;
    bool isStop = false;
    bool isOnTrap = false; 

    [SerializeField] private float rotCamXAxisSpeed = 5f; // 카메라 x축 회전속도
    [SerializeField] private float rotCamYAxisSpeed = 3f; // 카메라 y축 회전속도

    private float limitMinX = -80; // 카메라 x축 회전 범위 (최소)
    private float limitMaxX = 50; // 카메라 x축 회전 범위 (최대)

    private float eulerAngleX; // 마우스 좌 / 우 이동으로 카메라 y축 회전
    private float eulerAngleY; // 마우스 위 / 아래 이동으로 카메라 x축 회전

    public GameObject PrefabBullet;
    public Transform BulletPoint;

    public float BulletDelay = 1f;
    public float BulletTime = 0;
    bool isBullet= false;

    public TextMeshProUGUI bulletUI;

    public void CalculateRotation(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
        transform.rotation = Quaternion.Euler(0, eulerAngleY, 0);
        transform.GetChild(0).transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    // 카메라 x축 회전의 경우 회전 범위를 설정
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }

        if (angle > 360)
        {
            angle -= 360;
        }

        return Mathf.Clamp(angle, min, max);
    }


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        bulletUI.text = "Bullet X " + bulletNum;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp != 0)
        {
           Walk();
           Attack();
           GunAttack();
           float mouseX = Input.GetAxis("Mouse X");
           float mouseY = Input.GetAxis("Mouse Y");
           CalculateRotation(mouseX, mouseY);
            // Camera.main.transform.position = this.transform.position + new Vector3(0, 1, -0.2f);
        }
 

    }


    public void Walk()
    {
        isWalk = false;
   
        if (Input.GetKey(KeyCode.W))
        {
            characterController.Move(this.transform.forward * Time.deltaTime * speed);
            isWalk = true;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            characterController.Move(-this.transform.forward * Time.deltaTime * speed);
            isWalk = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            characterController.Move(-this.transform.right * Time.deltaTime * speed);
            isWalk = true;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            characterController.Move(this.transform.right * Time.deltaTime * speed);
            isWalk = true;
        }

        animator.SetBool("walk", isWalk);


    }
    public void Attack()
    {  
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            bat.SetActive(true);
            Gun.SetActive(false);
            animator.SetTrigger("attack");
            isAttackCheck = true;
            Invoke("StopAttackCheck",0.5f);
        }
    }

    public void GunAttack()
    {
        if(isBullet)
        {
            BulletTime += Time.deltaTime;
            if(BulletTime >= BulletDelay)
            {
                isBullet = false;
                BulletTime = 0;
            }
        }
        if (!isBullet)
        {
            if (bulletNum > 0)
            {
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    bat.SetActive(false);
                    Gun.SetActive(true);
                    isBullet = true;
                    animator.SetTrigger("isAttack");
                    Invoke("SpawnBullet", 0.2f);
                    bulletNum--;
                    bulletUI.text = "Bullet X " + bulletNum;
                }
            }
        }




    }
    void SpawnBullet()
    {
        Instantiate(PrefabBullet, BulletPoint.position, this.transform.rotation);
    }
    void StopAttackCheck()
    {
        isAttackCheck = false;
    }
    void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero);

        float rayLength;

        if(plane.Raycast(ray, out rayLength))
        {
            Vector3 mousePoint = ray.GetPoint(rayLength);
            this.transform.LookAt(new Vector3(mousePoint.x,this.transform.position.y,mousePoint.z));

        }
    }

    public void sethp(int damage) 
    {

        if (!isStop)
        {
            hp -= damage;

           


            if (hp <= 0)
            {
                GameManager.Instance.GameOverUI.SetActive(true);
                hp = 0;
                Debug.Log("gameover");
                animator.SetTrigger("death");
                isStop = true;
                heart[hp].SetActive(false);
                Invoke("Timestop", 2f);
            }
            
             heart[hp].SetActive(false);

        }
    
    
    }
    public void Timestop()
    {
        Time.timeScale = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Heal"))
        {
            if (hp < 3)
            {
                hp++;
                heart[hp-1].SetActive(true);
                Destroy(other.gameObject);
            }
        }
        if (other.CompareTag("Bullet"))
        {
           
            bulletNum++;
            bulletUI.text = "Bullet X " + bulletNum;
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Trap"))
        {
            sethp(1);
        }
        if (other.CompareTag("traptrigger"))
        {
            Debug.Log("dd");
            if (other.GetComponent<Trap>().commDelayReal == true)
            {
                
                StartCoroutine(TrapDelay(other.gameObject));
            }
            else
            other.transform.GetChild(0).GetComponent<Animator>().SetTrigger("open");
        }


    }

    IEnumerator TrapDelay(GameObject other)
    {
        yield return new WaitForSeconds(0.3f);
        other.transform.GetChild(0).GetComponent<Animator>().SetTrigger("open");
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("traptrigger"))
        {
            other.transform.GetChild(0).GetComponent<Animator>().SetTrigger("close");
        }
    }
}
