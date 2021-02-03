using UnityEngine;

public class Enemy : Character {
    public int value = 10;
    float troopCountdown = 0;
    public int wallDamage = 1;
    public float stoppingDistance = 0.02f;
    Transform target;
    protected Troop troopTarget = null;
    Transform reachPlace;
    Vector3 direction;

    public float getTargetDelta = 0.5f;

    public LayerMask troopMask;

    public override LayerMask EnemyMask
    {
        get
        {
            return troopMask;
        }

        set
        {
            troopMask = value;
        }
    }

    public float troopIsTargetRange = 5;

    void Start()
    {
        target = reachPlace = WaveLauncher.instance.reachPlace;

        InvokeRepeating("GetTarget", getTargetDelta, getTargetDelta);
    }

    void GetTarget()
    {
        float minDist = float.MaxValue;

        foreach(Collider col in Physics.OverlapSphere(transform.position, troopIsTargetRange, troopMask))
        {
            float dist = Vector3.Distance(transform.position, col.transform.position);

            if(dist < minDist)
            {
                minDist = dist;
                target = col.transform;
            }
        }

        if (target == null) target = reachPlace;

        MoveAgent(target.position);
    }

    void FixedUpdate()
    {
        if (dead) return;

        if(target == null)
        {
            anim.SetTrigger("Move");
            GetTarget();
        }
        
        MoveAgent(target.position);

        if (troopCountdown > 0) troopCountdown -= Time.fixedDeltaTime;

        Attack();
    }

    void Attack()
    {
        float dist = Vector3.Distance(transform.position, target.position);

        if (target == reachPlace)
        {
            if (dist <= stoppingDistance)
            {
                PlayerStats.Health -= wallDamage;
                Die();
            }
        }
        else if(troopCountdown <= 0 && dist <= attackRange)
        {
            troopCountdown = 1f / attackRate;
            if (troopTarget == null) troopTarget = target.GetComponent<Troop>();
            anim.SetTrigger("Attack");

            attacker.Attack(this, troopTarget);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, troopIsTargetRange);
    }

    public override void Die()
    {
        if (dead) return;
        dead = true;
        WaveLauncher.instance.EnemyDied();
        PlayerStats.Money += value;

        base.Die();
    }
}
