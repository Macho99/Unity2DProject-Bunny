using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 100;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        StartCoroutine(TimeOutDestroy());
    }

    public void SetDirection(Vector3 dir)
    {
        rb.velocity = dir * speed;
    }

    IEnumerator TimeOutDestroy()
    {
        yield return new WaitForSeconds(10f);
        ObjPoolManager.Instance.ReturnBullet(this);
        yield return null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ObjPoolManager.Instance.ReturnBullet(this);
    }
}
