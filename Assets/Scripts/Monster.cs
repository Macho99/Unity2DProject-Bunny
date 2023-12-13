using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Monster : MonoBehaviour
{
    GameManager gameManager;
    Player player;
    Rigidbody2D rb;
    private Vector3 dir;
    private Animator animator;
    public int maxHp;
    public int curHp;
    public int speed;
    public float chaseDist;
    public Image hpImage;
    private float hpImageWidth;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        curHp = maxHp;
        hpImageWidth = hpImage.rectTransform.sizeDelta.x;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        animator = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        player = gameManager.GetPlayer();
        StartCoroutine(RandomMove());
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private bool CanChase()
    {
        if ((player.transform.position - transform.position).sqrMagnitude < chaseDist * chaseDist)
        {
            return true;
        }
        return false;
    }
    private void FixedUpdate()
    {
        if(rb.velocity.x < 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }
    IEnumerator RandomMove()
    {
        animator.SetBool("isChasing", false);
        while (true)
        {
            dir = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            dir.Normalize();
            rb.velocity = dir * speed;

            if (CanChase())
            {
                StartCoroutine(Chase()); // 추격 코루틴 시작
                yield break; // 현재 코루틴 종료
            }

            yield return new WaitForSeconds(Random.Range(0f, 2.0f));
        }
    }

    IEnumerator Chase()
    {
        animator.SetBool("isChasing", true);
        while (CanChase())
        {
            Vector3 dir = player.transform.position - transform.position;
            dir.Normalize();
            rb.velocity = dir * speed;

            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(RandomMove()); // 추격 종료 후 다시 무작위 움직임 시작
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Bullet>(out Bullet bullet))
        {
            TakeDamage();
        }
        else if (collision.collider.TryGetComponent<Player>(out _))
        {
            Destroy(gameObject);
        }
        else if (collision.collider.tag.Equals("HorizonWall"))
        {
            rb.AddForce(new Vector2(0, -transform.position.y), ForceMode2D.Impulse);
        }
        else if (collision.collider.tag.Equals("VerticalWall"))
        {

            rb.AddForce(new Vector2(-transform.position.x / 2, 0), ForceMode2D.Impulse);
        }
    }

    private void TakeDamage()
    {
        animator.SetTrigger("Hit");
        curHp--;
        if(curHp <= 0)
        {
            curHp = 0;
            gameManager.ScoreAdd();
            Destroy(gameObject);
        }
        SetHpBar();
    }
    private void SetHpBar()
    {
        float ratio = (float)curHp / (float)maxHp;
        hpImage.rectTransform.sizeDelta = new Vector2(ratio * hpImageWidth, hpImage.rectTransform.sizeDelta.y);
    }
}
