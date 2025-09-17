using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool hitBall = false;
    private bool nohit = false;
    private bool onhit = false;

    // ZoneLayers
    private int homeRunZoneLayer;
    private int fairZoneLayer;
    private int foulZoneLayer;
    private int strikeZoneLayer;
    private int ballZoneLayer;

    private void Awake()
    {
        homeRunZoneLayer = LayerMask.NameToLayer("HomeRunZone");
        fairZoneLayer = LayerMask.NameToLayer("FairZone");
        foulZoneLayer = LayerMask.NameToLayer("FoulZone");
        strikeZoneLayer = LayerMask.NameToLayer("StrikeZone");
        ballZoneLayer = LayerMask.NameToLayer("BallZone");
    }

    private void OnTriggerEnter(Collider other)
    {
        int otherLayer = other.gameObject.layer;

        if (hitBall)
        {
            if (onhit) return;
            ScoreManager.instance.DisplayMessage("ÃÆ½À´Ï´Ù !");
            if (otherLayer == homeRunZoneLayer)
            {
                ScoreManager.instance.ProcessHomeRun();
                onhit = true;
            }
            else if (otherLayer == fairZoneLayer)
            {
                ScoreManager.instance.ProcessHit();
                onhit = true;
            }
            else if (otherLayer == foulZoneLayer)
            {
                ScoreManager.instance.ProcessFoul();
                onhit = true;
            }
        }
        else
        {
            if (nohit) return;
            nohit = true;
            if (otherLayer == strikeZoneLayer)
            {
                gameObject.SetActive(false);
                ScoreManager.instance.PitchJudged(true);
            }
            else if (otherLayer == ballZoneLayer)
            {
                gameObject.SetActive(false);
                ScoreManager.instance.PitchJudged(false);
            }
        }
    }

    public void HitByBat()
    {
        hitBall = true;
    }

    public void ResetHitBall()
    {
        hitBall = false;
        nohit = false;
        onhit = false;
    }
}
