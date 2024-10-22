using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythicCharacter_Sc : MonoBehaviour
{
    //
    Rigidbody2D rb;
    CapsuleCollider2D col;

    //
    [SerializeField] float moveSpeed;
    [SerializeField] float damage;
    [SerializeField] bool teamLeft;
    private string targetTag = "";

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();

        if(teamLeft == true)
        {
            targetTag = "Team2";
        }
        else
        {
            targetTag = "Team1";
            moveSpeed = -1 * moveSpeed;

            Vector2 direction = transform.localScale;
            direction.x = -1 * direction.x;
            transform.localScale = direction;
        }
    }

    private void Update()
    {
        float speed = moveSpeed;
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == targetTag)
        {
            if (collision.gameObject.GetComponent<Swordsman_Sc>() != null)
            {
                collision.gameObject.GetComponent<Swordsman_Sc>().Damage(damage);
            }
            else if (collision.gameObject.GetComponent<ArcherSc>() != null)
            {
                collision.gameObject.GetComponent<ArcherSc>().Damage(damage);
            }
            else if (collision.gameObject.GetComponent<Cavalry_Sc>() != null)
            {
                collision.gameObject.GetComponent<Cavalry_Sc>().Damage(damage);
            }
        }
    }
}