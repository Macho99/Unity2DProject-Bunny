using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Carrot : MonoBehaviour
{
    private int healAmount = 1;
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
            Destroy(gameObject);
        }
        else if(collision.TryGetComponent<Monster>(out _))
        {
            Destroy(gameObject);
        }
    }
}