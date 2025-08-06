using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int Scores = 0;
    public TextMeshProUGUI ScoreText;

    void ScoreUpdate()
    {
        ScoreText.text = "Score : " + (++Scores).ToString();
    }

    public void GetScore()
    {
        ScoreUpdate();
    }
}
