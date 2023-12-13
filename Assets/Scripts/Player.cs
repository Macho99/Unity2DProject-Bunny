using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHp;
    public int curHp;
    private GameManager gameManager;
    public GameObject bulletPrefab;
    public GameObject bullets;
    private Transform gunTrans;
    private Transform gunFirePoint;
    public float shotPeriod;

    public GameObject bombPrefab;

    private bool shotKey;

    private void Start()
    {
        curHp = maxHp;
        shotKey = false;
        gunTrans = transform.GetChild(0).GetChild(0);
        gunFirePoint = gunTrans.GetChild(0);
        StartCoroutine(ShotCoroutine());
        gameManager = GameManager.Instance;
    }
    public void Shot()
    {
        GameObject bullet = Instantiate(bulletPrefab, bullets.transform);
        bullet.transform.position = gunFirePoint.position;
        bullet.GetComponent<Bullet>().SetDirection((gunTrans.position - transform.position).normalized);
        bullet.transform.rotation = gunTrans.rotation;
    }

    public void ThrowBomb()
    {
        Bomb bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity).GetComponent<Bomb>();
        Vector2 targetPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        bomb.Throw(targetPos);
    }

    public void ShotKeyToggle()
    {
        shotKey = !shotKey;
    }

    IEnumerator ShotCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => shotKey);
            Shot();
            yield return new WaitForSeconds(shotPeriod);
        }
    }
    private void TakeDamage()
    {
        curHp--;
        GameManager.Instance.PlayerHpChanged();
        if(curHp <= 0)
        {
            curHp = 0;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent<Monster>(out Monster monster))
        {
            TakeDamage();
        }
    }
}