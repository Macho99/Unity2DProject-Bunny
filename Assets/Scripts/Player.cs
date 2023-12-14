using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHp;
    public int curHp;
    private Transform gunTrans;
    private Transform gunFirePoint;
    public float shotPeriod;
    public GameObject healParticlePrefab;

    public GameObject bombPrefab;

    private bool shotKey;

    private void Start()
    {
        curHp = maxHp;
        shotKey = false;
        gunTrans = transform.GetChild(0).GetChild(0);
        gunFirePoint = gunTrans.GetChild(0);
        StartCoroutine(ShotCoroutine());
    }
    public void Shot()
    {
        GameObject bullet = ObjPoolManager.Instance.GetBullet().gameObject;
        bullet.transform.position = gunFirePoint.position;
        bullet.GetComponent<Bullet>().SetDirection((gunTrans.position - transform.position).normalized);
        bullet.transform.rotation = gunTrans.rotation;
    }

    public void ThrowBomb()
    {
        Bomb bomb = ObjPoolManager.Instance.GetBomb();
        Vector2 targetPos = Camera.main.ScreenToWorldPoint((Vector2)Input.mousePosition);
        bomb.Throw(transform.position ,targetPos);
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
    private void TakeDamage(int amount)
    {
        curHp--;
        if(curHp <= 0)
        {
            curHp = 0;
        }
        GameManager.Instance.PlayerHpChanged();
    }
    private void Heal(int amount)
    {
        curHp += amount;
        if(curHp > maxHp)
        {
            curHp = maxHp;
        }
        GameManager.Instance.PlayerHpChanged();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.TryGetComponent<Monster>(out Monster monster))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Carrot>(out Carrot carrot))
        {
            Heal(carrot.GetHealAmount());
            GameObject particleObj = Instantiate(healParticlePrefab, transform);
            Destroy(particleObj, particleObj.GetComponent<ParticleSystem>().main.duration);
        }
    }
}