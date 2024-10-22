using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Swordsman_Sc : MonoBehaviour
{
    //
    Rigidbody2D rb;
    CapsuleCollider2D col;
    Animator animator;
    SpriteRenderer sp;

    //Move
    [SerializeField] float moveSpeed;
    private float gameSpeed;

    //Attack
    private float timer;
    [SerializeField] public float range;
    private string targetTag = "";
    [SerializeField] private float damage;

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
            direction.x = -1*direction.x;
            transform.localScale = direction;     
        }
    }

    private void Update()
    {
        Movement();
        Attack();

        if (health <= 0)
        {
            Dead();
        }

        if (health > 0)
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
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void Attack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float targetDistance = Mathf.Infinity;
        GameObject targetObj = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector2.Distance(gameObject.transform.position, enemy.transform.position);
            float distance_y = Mathf.Abs(gameObject.transform.position.y - enemy.transform.position.y);

            if (distance_y <= 2f && distance < targetDistance)
            {
                targetDistance = distance;
                targetObj = enemy;
            }
        }

        if (targetObj != null && health > 0)
        {
            if (targetDistance <= range)
            {
                gameSpeed = 0f;
                animator.SetBool("Attack", true);
                timer += Time.deltaTime;

                if (timer >= 1f)
                {
                    if(targetObj.GetComponent<Swordsman_Sc>() != null)
                    {
                        targetObj.GetComponent<Swordsman_Sc>().Damage(damage);
                    }
                    else if(targetObj.GetComponent<ArcherSc>() != null)
                    {
                        targetObj.GetComponent<ArcherSc>().Damage(damage);
                    }
                    else if (targetObj.GetComponent<Cavalry_Sc>() != null)
                    {
                        targetObj.GetComponent<Cavalry_Sc>().Damage(damage);
                    }
                    timer = 0f;
                }
            }
            else
            {
                gameSpeed = moveSpeed;
                animator.SetBool("Attack", false);
                timer = 0f;
            }
        }
        else
        {
            animator.SetBool("Attack", false);
            gameSpeed = moveSpeed;
        }
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

    private void Destroy()
    {
        Destroy(gameObject);
    }

    public void Damage(float damage)
    {
        health -= damage;
    }
}