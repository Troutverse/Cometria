using UnityEngine;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float HP = 50.0f;
    public TextMeshProUGUI HPText;


    void Start()
    {
        EnemyHPUpdate();       
    }
    void EnemyHPUpdate()
    {
        HPText.text = HP.ToString();
    }
    public void EnemyTakeDamage(float damage)
    {
        HP -= damage;
        EnemyHPUpdate();
        if (HP <= 0)
        {
            Score scoreManager = FindFirstObjectByType<Score>().GetComponent<Score>();
            Destroy(gameObject);
            scoreManager.GetScore();
        }
    }
}
