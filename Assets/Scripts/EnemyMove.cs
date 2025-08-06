using System.Collections;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float EnemyMoveSpeed = 1.0f;
    public float Detection = 10000.0f;

    private Transform PlayerPosition;

    private void Start()
    {
        PlayerPosition = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (PlayerPosition != null)
        {
            StartCoroutine(EnemyMoves());
        }
    }

    IEnumerator EnemyMoves()
    {
        while (PlayerPosition != null)
        {
            float Distance = Vector3.Distance(transform.position, PlayerPosition.position);
            
            if (Distance < Detection)
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerPosition.position, EnemyMoveSpeed * Time.deltaTime);
            }
            yield return null;
        }
    }
}
