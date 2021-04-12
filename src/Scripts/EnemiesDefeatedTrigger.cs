using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesDefeatedTrigger : MonoBehaviour
{
    public GameObject[] enemies;
    public List<GameObject> enemiesList;
    public int enemiesLeft;
    void Start()
    {
        enemiesLeft = enemies.Length;
        foreach (GameObject enemy in enemies)
        {
            enemiesList.Add(enemy);
        }
    }
    void Update()
    {
        for (int i = 0; i < enemiesList.Count; i++)
        {
            if (enemiesList[i] == null)
            {
                enemiesList.RemoveAt(i);
                enemiesLeft--;
            }
        }
        if (enemiesList.Count == 0)
        {
            FindObjectOfType<AudioManager>().Play("RoomClear");
            Destroy(gameObject);
        }
    }
}