using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public List<GameObject> monsterPrefabList;
    private static GameManager _instance;
    private Player player;
    public TextMeshProUGUI scoreText;
    public Image hpImage;
    private float hpImageWidth;
    private int score = 0;
    public int goal = 5;
    public float spawnPeriod;
    public float spawnPeriodOffset;
    public GameObject monsterFolder;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject singleton = new GameObject("GameManager");
                    _instance = singleton.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }
    public Player GetPlayer()
    {
        return player;
    }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    void Start()
    {
        hpImageWidth = hpImage.transform.parent.GetComponent<Image>().rectTransform.sizeDelta.x;
        player = FindObjectOfType<Player>();
        scoreText.text = $"0 / {goal.ToString()}";
        StartCoroutine(SpawnCoroutine());
    }

    void Update()
    {

    }
    public void PlayerHpChanged()
    {
        RectTransform rectTransform = hpImage.GetComponent<RectTransform>();
        rectTransform.anchorMax = new Vector2(player.curHp / (float)player.maxHp, 1);
        if(player.curHp <= 0)
        {
            SceneManager.LoadScene("LoseScene");
        }
    }
    public void ScoreAdd()
    {
        score++;
        scoreText.text = $"{score.ToString()} / {goal.ToString()}";
        if(score >= goal)
        {
            SceneManager.LoadScene("EndScene");
        }
        return;
    }

    void SpawnMonster()
    {
        GameObject monsterPrefab = monsterPrefabList[Random.Range(0, monsterPrefabList.Count)];
        GameObject monster = Instantiate(monsterPrefab, monsterFolder.transform);
        monster.transform.position = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(-3.0f, 3.0f));
    }

    IEnumerator SpawnCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(spawnPeriod - spawnPeriodOffset, spawnPeriod + spawnPeriodOffset));
            SpawnMonster();
        }
    }
}
