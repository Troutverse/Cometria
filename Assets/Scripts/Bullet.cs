using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
    public float BulletSpeed = 10.0f;
    public float BulletLifeTime = 5.0f;
    public GameObject Effect_Prefab;

    private BulletPool Pool;
    private Coroutine Life_coroutine;

    public void SetPool(BulletPool pool)
    {
        this.Pool = pool;
    }

    private void OnEnable()
    {
        Life_coroutine = StartCoroutine(BulletReturn());
    }
    private void OnDisable()
    {
        if (Life_coroutine != null)
        {
            StopCoroutine(Life_coroutine);
        }
    }
    private void Update()
    {
        transform.position += transform.forward * BulletSpeed * Time.deltaTime;
    }
    IEnumerator BulletReturn()
    {
        yield return new WaitForSeconds(BulletLifeTime);
        ReturnPool();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.EnemyTakeDamage(20f);
            }
        }

        if (Effect_Prefab != null)
        {
            GameObject effect = Instantiate(Effect_Prefab, transform.position, Quaternion.identity);
            Destroy(effect, 1.0f);
        }
        ReturnPool();
    }
    void ReturnPool() => Pool.Return(gameObject);
}
