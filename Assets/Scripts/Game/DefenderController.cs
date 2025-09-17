using UnityEngine;
using UnityEngine.AI;

public enum DefenderRole
{
    FRIST_BASEMAN,
    SECONDE_BASEMAN,
    THIRD_BASEMAN,
    SHORTSTOP,
    OUTFIEDER,
}

public class DefenderController : MonoBehaviour
{
    private NavMeshAgent defenderAgent;
    private Animator defenderAnimator;
    private int defenderRun;
    private Vector3 defenderOriginalPosition;
    private Quaternion defenderOriginalQuaternion;

    public DefenderRole defenderRole;
    public Transform defenderBase;

    private void Awake()
    {
        defenderAgent = GetComponent<NavMeshAgent>();
        defenderAnimator = GetComponent<Animator>();
        defenderRun = Animator.StringToHash("Run");
        defenderOriginalPosition = transform.position;
        defenderOriginalQuaternion = transform.rotation;
    }

    private void Update()
    {
        float currentSpeed = defenderAgent.velocity.magnitude;
        defenderAnimator.SetFloat(defenderRun, currentSpeed);
    }

    public void MoveToBall(Vector3 targetPosition)
    {
        if (defenderAgent.isActiveAndEnabled)
        {
            defenderAgent.SetDestination(targetPosition);
        }
    }

    public void ReturnToOriginalPosition()
    {
        if (defenderAgent.isActiveAndEnabled)
        {
            defenderAgent.SetDestination(defenderOriginalPosition);
        }
    }
}
