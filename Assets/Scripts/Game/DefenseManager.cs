using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DefenseManager : MonoBehaviour
{
    public static DefenseManager instance;
    private List<DefenderController> defenderList;
    private int fairZoneLayer;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        fairZoneLayer = LayerMask.NameToLayer("FairZone");

        GameObject[] defenders = GameObject.FindGameObjectsWithTag("Defender");
        defenderList = new List<DefenderController>();

        foreach (var defender in defenders)
        {
            DefenderController _defender = defender.GetComponent<DefenderController>();
            if (_defender != null) defenderList.Add(_defender);
        }
    }

    public void BatterHit(Rigidbody ballRigidbody)
    {
        StartCoroutine(PredictDrawTrajectory(ballRigidbody));
    }

    private IEnumerator PredictDrawTrajectory(Rigidbody ballRigidbody)
    {
        yield return new WaitForFixedUpdate();

        Vector3 startPoint = ballRigidbody.position;
        Vector3 initialVelocity = ballRigidbody.linearVelocity;

        Vector3[] trajectoryPoints = BallTrajectoryPrediction.GetTrajectoryPoints(startPoint, initialVelocity);

        if (trajectoryPoints.Length > 0)
        {
            Color linecolor = Color.yellow;
            for (int i = 0; i < trajectoryPoints.Length - 1; i++)
            {
                Debug.DrawLine(trajectoryPoints[i], trajectoryPoints[i + 1], Color.red, 30f);
            }
            Vector3 landingPoint = trajectoryPoints[trajectoryPoints.Length - 1];
            Debug.Log($"LandingPoint : {landingPoint}");

            Ray ray = new Ray(landingPoint + Vector3.up * 100f, Vector3.down);
            if (Physics.Raycast(ray, 200f, 1 << fairZoneLayer))
            {
                FindAndCommandClosestDefender(landingPoint);
            }
            
        }
    }

    private void FindAndCommandClosestDefender(Vector3 ballPosiontion)
    {
        DefenderController closestDefender = null;
        float minSqrDistance = float.MaxValue;
        if (defenderList.Count == 0) { Debug.Log($"No defender"); }
        
        foreach (DefenderController defender in defenderList)
        {
            Debug.Log(defender.name);
            float sqrDistance = (defender.transform.position - ballPosiontion).sqrMagnitude;
            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                closestDefender = defender;
            }
        }
        Debug.Log($"closetDefender : {closestDefender.name} Move To Ball Call {ballPosiontion}");
        closestDefender.MoveToBall(ballPosiontion);
    }
}