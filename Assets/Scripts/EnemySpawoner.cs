using System.Collections;
using UnityEngine;

public class EnemySpawoner : MonoBehaviour
{
    public float Interval = 5.0f;
    public Transform EnemySpawner;
    public GameObject Enemy_Prefab;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    IEnumerator Spawn()
    {
        while (true)
        {
            Instantiate(Enemy_Prefab, EnemySpawner.position, Quaternion.identity);
            yield return new WaitForSeconds(Interval);
        }
    }
}
