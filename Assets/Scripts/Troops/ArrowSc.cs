using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Sc : MonoBehaviour
{
    //
    Rigidbody2D rb;
    private bool hit;
    [SerializeField] private float damage;
    public string arrow_targetTag = "";

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(hit != true)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.tag == arrow_targetTag)
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
            hit = true;
            rb.velocity = Vector2.zero;
            arrow_targetTag = "Untagged";
            Invoke("Destroy", 15f);
        }
        if (collision.gameObject.tag == "Ground")
        {
            hit = true;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            arrow_targetTag = "Untagged";
            Invoke("Destroy", 15f);
        }
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}