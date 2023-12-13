using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleInput : MonoBehaviour
{
    public float speed = 4;
    public float jumpHeight = 3;
    public float rotateSpeed = 60;
    private Transform gunPivot;
    private SpriteRenderer gunRenderer, bunnyRenderer;
    private Player player;
    private Rigidbody2D rb;
    private Animator animator;

    private void Start()
    {
        gunPivot = transform.GetChild(0);
        gunRenderer = gunPivot.GetChild(0).GetComponent<SpriteRenderer>();
        bunnyRenderer = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        Move();
        //Jump();
        RotateGun();
        Shot();
        ThrowBomb();
    }

    private void ThrowBomb()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            player.ThrowBomb();
        }
    }

    private void Shot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            player.ShotKeyToggle();
        }
    }
    private void RotateGun()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.z = 0f;
        target -= transform.position;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        if(Mathf.Abs(angle) > 90)
        {
            gunRenderer.flipY = true;
        }
        else
        {
            gunRenderer.flipY = false;
        }
        gunPivot.right = target;
        //gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal") * speed;
        float inputY = Input.GetAxis("Vertical") * speed;

        if(inputX == 0 && inputY == 0)
        {
            animator.SetBool("isWalking", false);
            return;
        }

        if(inputX < 0)
        {
            bunnyRenderer.flipX = true;
        }
        else if(inputX > 0)
        {
            bunnyRenderer.flipX = false;
        }

        animator.SetBool("isWalking", true);
        rb.velocity = new Vector2(inputX, inputY);

        //Vector3 dir = new Vector3(0, 0, 0);
        //if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        //{
        //    dir.y += 1;
        //}
        //if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        //{
        //    dir.y -= 1;
        //}
        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        //{
        //    dir.x -= 1;
        //}
        //if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        //{
        //    dir.x += 1;
        //}
        //dir.Normalize();
        //transform.position += dir * speed * Time.deltaTime;
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position += new Vector3(0, jumpHeight, 0);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            transform.position -= new Vector3(0, jumpHeight, 0);
        }
    }
}
