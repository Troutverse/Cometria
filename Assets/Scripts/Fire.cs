using UnityEngine;

public class Fire : MonoBehaviour
{
    public BulletPool pool;
    public GameObject ShootEffect_Prefab;

    // 발사 지점
    public Transform pos;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject ShootEffect = Instantiate(ShootEffect_Prefab, transform.position, Quaternion.identity);
            Destroy(ShootEffect, 0.3f);

            var bullet = pool.GetBullet();
            bullet.transform.position = pos.position;
            bullet.transform.rotation = pos.rotation;
        }
    }
}
