using UnityEngine;
using UnityEngine.AI;

public abstract class Tower : MonoBehaviour
{
    public float attackRate;
    public float range;
    public LayerMask enemyMask;
    float countdown = 0;
    public TowerBlueprint blueprint;
    public int level = 1;

    public enum TargetMode { First, Last, Nearest, Farthest, Weakest, Strongest }
    public TargetMode targetMode = TargetMode.Nearest;

    protected abstract void Attack();
    protected virtual void PrepareToAttack()
    {
        countdown = 1f / attackRate;
        Attack();
    }

    public static Enemy UpdateTargetForAnyone(Transform transform, TargetMode targetMode, float range, LayerMask enemyMask)
    {
        Enemy target = null;

        float minDist = float.MaxValue;
        float maxDist = float.MinValue;

        Collider[] cols = Physics.OverlapSphere(transform.position, range, enemyMask);

        if (cols.Length > 0)
            foreach (Collider col in cols)
            {
                Enemy e = col.GetComponent<Enemy>();

                if (e == null)
                {
                    Debug.LogError("Enemy Not Attached to " + col.name);
                    continue;
                }

                float dist = Vector3.Distance(transform.position, e.transform.position);
                NavMeshAgent agent = e.GetComponent<NavMeshAgent>();

                if ((targetMode == TargetMode.First || targetMode == TargetMode.Last) && !agent.isOnNavMesh) continue;

                float pathLeft = agent.remainingDistance;

                switch (targetMode)
                {
                    case TargetMode.First:
                        if (target == null || pathLeft < minDist)
                        {
                            minDist = pathLeft;
                            target = e;
                        }

                        break;

                    case TargetMode.Last:
                        if (pathLeft > maxDist)
                        {
                            maxDist = pathLeft;
                            target = e;
                        }

                        break;

                    case TargetMode.Nearest:

                        if (dist < minDist)
                        {
                            minDist = dist;
                            target = e;
                        }

                        break;

                    case TargetMode.Farthest:

                        if (dist > maxDist)
                        {
                            maxDist = dist;
                            target = e;
                        }

                        break;

                    case TargetMode.Weakest:

                        if (target == null || target.health > e.health)
                        {
                            target = e;
                        }

                        break;

                    case TargetMode.Strongest:

                        if (target == null || target.health < e.health)
                        {
                            target = e;
                        }

                        break;
                }
            }

        return target;
    }

    protected Enemy UpdateTarget(Transform transform)
    {
        return UpdateTargetForAnyone(transform, targetMode, range, enemyMask);
    }

    void Update()
    {
        countdown -= Time.deltaTime;

        if(countdown <= 0)
        {
            PrepareToAttack();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public virtual void OnTargetModeChanged() { }
}