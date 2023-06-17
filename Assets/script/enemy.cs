using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public GameObject BulletPrefab;
    public GameObject Attackpoint;
    bool hit;
    Renderer[] renderers;
    Color originColor;
    public GameObject target;
    NavMeshAgent agent;
    public bool isAttackCheck;
    public int hp = 2;
    bool isStop = false;
    bool isDie;

    [SerializeField] bool DebugMode = false;
    [Range(0f, 360f)] [SerializeField] float ViewAngle = 0f;
    [SerializeField] float ViewRadius = 1f;
    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;
    List<Collider> hitTargetList = new List<Collider>();

    public int hitrayplayer;
    Vector3 myPos;

    public Vector3 PosA, PosB;
    public enum XorZ
    {
        X,
        Z
    }
    public XorZ xorz;
    public bool xZ;
    public float speed=3f;
    public bool filp;
    private void OnDrawGizmos()
    {
        if (!DebugMode) return;
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, ViewRadius);
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    void Start()
    {
        target = GameObject.Find("Player");
        agent = this.GetComponent<NavMeshAgent>();
        agent.SetDestination(target.transform.position);
        agent.destination = target.transform.position;
        renderers = this.GetComponentsInChildren<Renderer>();
        originColor = renderers[0].material.color;
        agent.isStopped = true;
        xZ = false;
    }
    void hitplayer()
    {
        myPos = this.transform.position;
        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(this.transform.position, rightDir * ViewRadius, Color.blue);
        Debug.DrawRay(this.transform.position, leftDir * ViewRadius, Color.blue);
        Debug.DrawRay(this.transform.position, lookDir * ViewRadius, Color.cyan);
        hitTargetList.Clear();
        Collider[] Targets = Physics.OverlapSphere(myPos, ViewRadius, TargetMask);

        if (Targets.Length == 0) return;
        foreach (Collider EnemyColli in Targets)
        {
      
                Vector3 targetPos = EnemyColli.transform.position;
                Vector3 targetDir = (targetPos - myPos).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, ViewRadius, ObstacleMask))
                {
                   
                    hitrayplayer++;
                    hitTargetList.Add(EnemyColli);
                    if (DebugMode) Debug.DrawLine(myPos, targetPos, Color.red);
                }
            
        }

    }
    private void Update()
    {
      

        hitplayer();

        if (hitrayplayer == 0)
        {
            if (xZ)
            {
                switch (xorz)
                {
                    case XorZ.X:

                        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);

                        if (PosA.x > myPos.x && !filp)// 내위치 x좌표갚 8보다 작으면 플립
                        {
                            this.transform.rotation = Quaternion.Euler(0, 90, 0);
                            filp = true;
                        }
                        if (PosB.x < myPos.x)// 내위치 x좌표갚 26보다 크면 플립
                        {
                            this.transform.rotation = Quaternion.Euler(0, -90, 0);
                            
                        }




                        break;

                    case XorZ.Z:
                        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);

                        if (PosA.z > myPos.z && !filp)// 내위치 z좌표갚 8보다 작으면 플립
                        {
                            this.transform.rotation = Quaternion.Euler(0, 180, 0);
                            filp = true;
                        }
                        if (PosB.z < myPos.z)// 내위치 z좌표갚 26보다 크면 플립
                        {
                            this.transform.rotation = Quaternion.Euler(0, -180, 0);
                    
                        }

                        break;

                }
            }

        }
        if (!isDie)
        {
            if(!hit&& hitrayplayer==0)
            {
                agent.isStopped = true;
            }
            else
            {
                agent.isStopped = false;
                this.transform.LookAt(target.transform.position);
            }


            if (!agent.isStopped)
                {
                    if (Vector3.Distance(this.transform.position, target.transform.position) < agent.stoppingDistance + 0.1f)
                {
                
                    agent.isStopped = true;
                        StartCoroutine("attack");

                    }
                    else
                    {
                        agent.isStopped = false;
                        agent.destination = target.transform.position;
                    }
                }
               




                this.GetComponent<Animator>().SetBool("walk", !agent.isStopped);
            
        }
    }
    public void StopEnemy()
    {
        isDie = true;
    }
    IEnumerator attack()
    {

        yield return new WaitForSeconds(0.5f);
        this.GetComponent<Animator>().SetTrigger("attack");
        isAttackCheck = true;
        yield return new WaitForSeconds(0.5f);
        isAttackCheck = false;
        if (Vector3.Distance(this.transform.position, target.transform.position) < agent.stoppingDistance + 0.1f)
        {
            StartCoroutine("attack");
        }
        else
        {
            agent.isStopped = false;
        }

    }
    public void sethp(int damage)
    {

        if (!isStop)
        {
            hit = true;
            hp -= damage;
            if (hp <= 0)
            {
                hp = 0;
                Debug.Log("gameover");
                this.GetComponent<Animator>().SetTrigger("death");
                this.GetComponent<Animator>().SetTrigger("downdeath");
                StopEnemy();
                isStop = true;
                float randomDrop = Random.value;

                if(randomDrop < 0.2f)
                {
                    Instantiate(BulletPrefab, this.transform.position, this.transform.rotation);

                }



            }
           
                StartCoroutine("HitColor");
            


        }

    }

    IEnumerator HitColor()
    {
        foreach(Renderer render in renderers)
        {
            render.material.color = Color.red;

        }
        yield return new WaitForSeconds(0.5f);
        foreach (Renderer render in renderers)
        {
            render.material.color = originColor;

        }

    }

    public void AttackpointOn()
    {
        if(!Attackpoint.activeSelf)
        Attackpoint.SetActive(true);
        else if (Attackpoint.activeSelf)
        Attackpoint.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            sethp(1);
        }
        if (other.CompareTag("traptrigger"))
        {
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

}
