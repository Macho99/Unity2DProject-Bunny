using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    public ParticleSystem ps;
    private int healAmount = 1;
    private void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        ps.gameObject.SetActive(false);
    }
    public void SetHealAmount(int amount)
    {
        healAmount = amount;
    }
    public int GetHealAmount()
    {
        return healAmount;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Player>(out _))
        {
            ps.gameObject.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = false;
            Destroy(gameObject, 2f);
        }
        else if(collision.TryGetComponent<Monster>(out _))
        {
            Destroy(gameObject);
        }
    }
}