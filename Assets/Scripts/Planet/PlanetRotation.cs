using UnityEngine;

public class PlanetRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(5 * Time.deltaTime, 10 * Time.deltaTime, 0);
    }
}
