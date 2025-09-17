using UnityEngine;

public static class BallTrajectoryPrediction
{
    public static Vector3 PredictLandingPoint(Vector3 startPoint, Vector3 initialVelocity, float groundHeight = 0f)
    {
        float gravity = Physics.gravity.y;

        float a = 0.5f * gravity;
        float b = initialVelocity.y;
        float c = startPoint.y - groundHeight;

        float discriminant = (b * b) - (4 * a * c);
        if (discriminant < 0) return startPoint;

        float flightTime = Mathf.Max(
            (-b + Mathf.Sqrt(discriminant)) / (2 * a),
            (-b - Mathf.Sqrt(discriminant)) / (2 * a)
        );

        if (flightTime <= 0) return startPoint;

        return new Vector3(
            startPoint.x + initialVelocity.x * flightTime,
            groundHeight,
            startPoint.z + initialVelocity.z * flightTime
        );
    }

    public static Vector3[] GetTrajectoryPoints(Vector3 startPoint, Vector3 initialVelocity, int steps = 100, float groundHeight = 0f)
    {
        float gravity = Physics.gravity.y;

        float a = 0.5f * gravity;
        float b = initialVelocity.y;
        float c = startPoint.y - groundHeight;

        float discriminant = (b * b) - (4 * a * c);
        if (discriminant < 0) return new Vector3[0];

        float flightTime = Mathf.Max(
            (-b + Mathf.Sqrt(discriminant)) / (2 * a),
            (-b - Mathf.Sqrt(discriminant)) / (2 * a)
        );

        if (flightTime <= 0) return new Vector3[0];

        Vector3[] points = new Vector3[steps];
        for (int i = 0; i < steps; i++)
        {
            float t = (float)i / (steps - 1) * flightTime;
            float x = startPoint.x + initialVelocity.x * t;
            float y = startPoint.y + initialVelocity.y * t + 0.5f * gravity * t * t;
            float z = startPoint.z + initialVelocity.z * t;
            points[i] = new Vector3(x, y, z);
        }

        return points;
    }
}