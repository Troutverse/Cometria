using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
    // Singleton
    public static ScoreManager instance;

    public BSO_Display bso_Display;
    public TextMeshProUGUI ballStateText;

    // Game Count
    private int strikeCount = 0;
    private int ballCount = 0;
    private int outCount = 0;

    private int currentRound = 1;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        if (bso_Display != null) bso_Display.UpdateDisplay(strikeCount, ballCount, outCount);
        if (ballStateText != null) ballStateText.text = "";
    }

    public void DisplayMessage(string message)
    {
        if (ballStateText == null) return;
        ballStateText.text = message;
    }

    public void ProcessHit()
    {
        DisplayMessage("안타 입니다 !");
        ResetBatterCount();
    }

    public void ProcessFoul()
    {
        DisplayMessage("파울 입니다 !");
        if (strikeCount < 2)
        {
            strikeCount++;
        }
        if (bso_Display != null) bso_Display.UpdateDisplay(strikeCount, ballCount, outCount);
    }

    public void ProcessHomeRun()
    {
        DisplayMessage("홈런 입니다 !");
        ResetBatterCount();
    }

    public void PitchJudged(bool isStrike)
    {
        if (isStrike)
        {
            strikeCount++;
        }
        else
        {
            ballCount++;
        }
        string message = isStrike ? "스트라이크 !" : "볼 !";
        DisplayMessage(message);
        CheckCount();
        if (bso_Display != null) bso_Display.UpdateDisplay(strikeCount, ballCount, outCount);
    }

    private void CheckCount()
    {
        if (strikeCount >= 3)
        {
            outCount++;
            ResetBatterCount(); 
        }
        else if (ballCount >= 4)
        {
            ResetBatterCount(); 
        }
        else if (outCount > 2)
        {
            ResetBatterCount();
        }
    }

    private void ResetBatterCount()
    {
        strikeCount = 0;
        ballCount = 0;
    }
}
