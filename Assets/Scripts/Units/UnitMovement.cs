using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class UnitMovement : NetworkBehaviour
{

    [SerializeField] private NavMeshAgent agent = null;
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private float chaseDistance = 10f;

    #region Server

    [ServerCallback]
    private void Update()
    {
        Targetable target = targeter.GetTarget();
        if (target != null)
        {
            if ((target.transform.position - transform.position).sqrMagnitude > chaseDistance * chaseDistance)
            {
                agent.SetDestination(target.transform.position);
            }
            else if (agent.hasPath)
            {
                agent.ResetPath();
            }
            return;
        }

        if (!agent.hasPath) return;

        if (agent.remainingDistance > agent.stoppingDistance) return;

        agent.ResetPath();
    }

    [Command]
    public void CmdMove(Vector3 position)
    {
        targeter.ClearTarget();

        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;

        agent.SetDestination(position);
    }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}