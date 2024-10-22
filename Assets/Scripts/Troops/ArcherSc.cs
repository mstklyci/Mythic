using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSc : MonoBehaviour
{
    //
    Rigidbody2D rb;
    CapsuleCollider2D col;
    Animator animator;
    SpriteRenderer sp;

    //Move
    [SerializeField] float moveSpeed;
    private float gameSpeed;

    //Shot
    [SerializeField] private float range;
    private float enemyDistance;
    [SerializeField] private string targetTag = "";

    //Arrow
    [SerializeField] private Transform bow;
    private GameObject clonArrow;
    [SerializeField] private GameObject arrow;
    private float timer;
    private float randomShot;
    private bool shot, firstShot;

    //Health
    [SerializeField] private float health;

    //Team
    [SerializeField] private bool teamLeft;
    public Score score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();
        timer = 0f;
        gameSpeed = moveSpeed;

        score = GameObject.FindWithTag("Score").GetComponent<Score>();

        if (teamLeft == true)
        {
            gameObject.tag = "Team1";
            targetTag = "Team2";
        }
        else
        {
            gameObject.tag = "Team2";
            targetTag = "Team1";
            moveSpeed = -1 * moveSpeed;

            Vector2 direction = transform.localScale;
            direction.x = -1 * direction.x;
            transform.localScale = direction;
        }
    }

    private void Update()
    {
        Movement();
        Shot();
        
        if(health <= 0)
        {
            Dead();
        }

        if(health > 0)
        {
            if (score.team1Win == true && teamLeft != true)
            {
                gameObject.tag = "Untagged";
                targetTag = "Untagged";
                gameSpeed = -1.2f * moveSpeed;

                Vector2 direction = transform.localScale;
                direction.x = 1;
                transform.localScale = direction;
            }
            else if (score.team2Win == true && teamLeft == true)
            {
                gameObject.tag = "Untagged";
                targetTag = "Untagged";
                gameSpeed = -1.2f * moveSpeed;

                Vector2 direction = transform.localScale;
                direction.x = -1;
                transform.localScale = direction;
            }
        } 
    }

    private void Movement()
    {
        float speed = gameSpeed;
        rb.velocity = new Vector2 (speed, rb.velocity.y);
    }

    private void Shot()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float targetDistance = Mathf.Infinity;
        GameObject targetObj = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(gameObject.transform.position, enemy.transform.position);
            float distance_y = Mathf.Abs(gameObject.transform.position.y - enemy.transform.position.y);

            if(distance_y <= 2f && distance < targetDistance)
            {
                targetDistance = distance;
                targetObj = enemy;
            }            
        }

        if(targetObj != null && health > 0)
        {
            if(targetDistance <= range)
            {
                if(firstShot != true)
                {
                    StartCoroutine(Walk_shot());
                    firstShot = true;
                }

                if (shot != true && targetDistance > 5f)
                {
                    gameSpeed = moveSpeed;
                    animator.SetBool("Shoting", false);
                }
                else
                {
                    gameSpeed = 0f;
                    animator.SetBool("Shoting", true);
                    timer += Time.deltaTime;

                    if (timer >= 1f)
                    {
                        randomShot = Random.Range(0, 3);
                        timer = 0f;

                        if (targetDistance > 20 && targetDistance <= 30)
                        {
                            switch (randomShot)
                            {
                                case 0: RandomShot(Random.Range(150, 250), Random.Range(20, 25)); break;
                                case 1: RandomShot(Random.Range(200, 250), Random.Range(28, 33)); break;
                                case 2: RandomShot(Random.Range(125, 150), Random.Range(10, 17)); break;
                            }
                        }
                        else if (targetDistance > 10 && targetDistance <= 20)
                        {
                            switch (randomShot)
                            {
                                case 0: RandomShot(Random.Range(150, 250), Random.Range(27, 33)); break;
                                case 1: RandomShot(Random.Range(150, 250), Random.Range(10, 25)); break;
                                case 2: RandomShot(Random.Range(150, 250), Random.Range(10, 25)); break;
                            }
                        }
                        else
                        {
                            switch (randomShot)
                            {
                                case 0: RandomShot(Random.Range(125, 250), Random.Range(5, 25)); break;
                                case 1: RandomShot(Random.Range(125, 250), Random.Range(5, 25)); break;
                                case 2: RandomShot(Random.Range(125, 250), Random.Range(5, 25)); break;
                            }
                        }
                    }
                }               
            }
            else
            {
                gameSpeed = moveSpeed;
                animator.SetBool("Shoting", false);
                timer = 0f;
            }
        }
        else
        {           
            gameSpeed = moveSpeed;
            animator.SetBool("Shoting", false);
        }
    }

    IEnumerator Walk_shot()
    {
        while(true)
        {
            shot = true;
            yield return new WaitForSeconds(1f);
            shot = false;
            yield return new WaitForSeconds(2f);
        }  
    }

    private void RandomShot(float arrowForce, float arrowUpForce)
    {
        clonArrow = Instantiate(arrow, bow.position, gameObject.transform.rotation);
        clonArrow.GetComponent<Arrow_Sc>().arrow_targetTag = targetTag;

        if (teamLeft != true)
        {
            arrowForce = -1 * arrowForce;
        }

        Vector2 force = new Vector2(arrowForce, arrowUpForce);
        clonArrow.GetComponent<Rigidbody2D>().AddForce(force);
    }

    private void Dead()
    {
        gameSpeed = 0f;
        animator.SetBool("Death", true);
        gameObject.tag = "Untagged";
        rb.isKinematic = true;
        col.isTrigger = true;
        sp.sortingOrder = 1;
        Invoke("Destroy", 15f);
    }

    public void Damage(float damage)
    {
        health -= damage;
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}