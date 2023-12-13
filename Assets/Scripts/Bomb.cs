using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float maxHeight;     //�������� �׸� �� �ö� �ִ� ����
    public float flyingDuration;//�߻��������� ��ǥ �������� �̵��ϴµ� �ɸ��� �ð�
    public int damage;
    private Vector3 startPos;
    private Vector3 targetPos;
    public Transform spriteTransform;
    public GameObject explosionObj;
    public GameObject colliderObj;

    private void Start()
    {
        colliderObj.GetComponent<BombCollider>().SetDamage(damage);
    }

    public void Throw(Vector3 targetPos)
    {

        startPos = transform.position;
        this.targetPos = targetPos;
        
        _ = StartCoroutine(FlyingCoroutine());
    }

    IEnumerator FlyingCoroutine()
    {
        float startTime = Time.time;
        float endTime = startTime + flyingDuration;

        while (Time.time <= endTime)
        {
            float currentTime = Time.time - startTime;
            float durationAmount = currentTime / flyingDuration;
            float currentHeight;

            Vector2 spriteVector = new Vector2();

            if(durationAmount <= 0.5)
            {
                currentHeight = Mathf.Sin(Mathf.Lerp(0f, 90f,durationAmount * 2) * Mathf.Deg2Rad) * maxHeight;
                //currentHeight = Mathf.Lerp(0, maxHeight, durationAmount * 2);
                spriteVector.y = currentHeight;
            }
            else
            {
                currentHeight = Mathf.Sin(Mathf.Lerp(90f, 180f, (durationAmount - 0.5f) * 2) * Mathf.Deg2Rad) * maxHeight;
                //currentHeight = Mathf.Lerp(maxHeight, 0, (durationAmount - 0.5f) * 2);
                spriteVector.y = currentHeight;
            }

            spriteTransform.localPosition = spriteVector;
            transform.position = Vector3.Lerp(startPos, targetPos, durationAmount);

            yield return null;
        }
        spriteTransform.gameObject.SetActive(false);
        explosionObj.SetActive(true);
        colliderObj.SetActive(true);
        Destroy(gameObject, 0.5f);
    }
}
