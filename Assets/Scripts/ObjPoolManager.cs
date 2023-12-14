using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolManager : MonoBehaviour
{
    private static ObjPoolManager _instance;
    public static ObjPoolManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("ObjectPoolmanager").AddComponent<ObjPoolManager>();
            }
            return _instance;
        }
    }

    public GameObject runtimeObjFolder;
    public Bullet bulletPrefab;
    private Queue<Bullet> bullets;
    public Bomb bombPrefab;
    private Queue<Bomb> bombs;

    private void Awake()
    {
        if (_instance != null)
        {
            DestroyImmediate(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        bullets = new Queue<Bullet>(20);
        bombs = new Queue<Bomb>(10);

        for(int i=0;i<20; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullets.Enqueue(bullet);
            bullet.gameObject.SetActive(false);
            bullet.transform.parent = transform;
        }

        for (int i = 0; i < 10; i++)
        {
            Bomb bomb = Instantiate(bombPrefab);
            bombs.Enqueue(bomb);
            bomb.gameObject.SetActive(false);
            bomb.transform.parent = transform;
        }
    }

    public Bullet GetBullet()
    {
        if(bullets.Count == 0)
        {
            return Instantiate(bulletPrefab, runtimeObjFolder.transform);
        }
        else
        {
            Bullet bullet = bullets.Dequeue();
            bullet.gameObject.SetActive(true);
            bullet.transform.parent = runtimeObjFolder.transform;
            return bullet;
        }
    }
    
    public Bomb GetBomb()
    {
        if(bombs.Count == 0)
        {
            return Instantiate(bombPrefab, runtimeObjFolder.transform);
        }
        else
        {
            Bomb bomb = bombs.Dequeue();
            bomb.gameObject.SetActive(true);
            bomb.transform.parent = runtimeObjFolder.transform;
            return bomb;
        }
    }

    public void ReturnBomb(Bomb bomb)
    {
        bombs.Enqueue(bomb);
        bomb.gameObject.SetActive(false);
        bomb.transform.parent = transform;
    }

    public void ReturnBullet(Bullet bullet)
    {
        bullets.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.parent = transform;
    }
}