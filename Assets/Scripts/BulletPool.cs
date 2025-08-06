using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public GameObject Bullet_Prefab;
    public int BulletBox = 30;

    private List<GameObject> Pool;
    void Start()
    {
        Pool = new List<GameObject>();

        for (int i = 0; i < BulletBox; i++)
        {
            var Bullet = Instantiate(Bullet_Prefab);
            Bullet.transform.parent = transform;
            Bullet.SetActive(false);
            Bullet.GetComponent<Bullet>().SetPool(this);
            Pool.Add(Bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (var Bullet in Pool)
        {
            if (!Bullet.activeInHierarchy)
            {
                Bullet.SetActive(true);
                return Bullet;
            }
        }
        var new_bullet = Instantiate(Bullet_Prefab);
        new_bullet.transform.parent = transform;
        new_bullet.GetComponent<Bullet>().SetPool(this);
        Pool.Add(new_bullet);
        return new_bullet;
    }

    public void Return(GameObject Bullet)
    {
        Bullet.SetActive(false);
    }
}
