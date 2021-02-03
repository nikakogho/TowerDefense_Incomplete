using UnityEngine;

public class Troop : Character {
    Enemy target;
    public TroopSpawner spawner;

    public float seeRange;

    [HideInInspector]
    public Tower.TargetMode targetMode;
    [HideInInspector]
    public LayerMask enemyMask;

    Vector3 spawnPosition;

    public float goBackToSpawnPosAfter = 10;

    public override LayerMask EnemyMask
    {
        get
        {
            return enemyMask;
        }

        set
        {
            enemyMask = value;
        }
    }

    void Start()
    {
        InvokeRepeating("AssignTarget", 0, 1);

        spawnPosition = transform.position;
        noTargetCountdown = goBackToSpawnPosAfter;
    }

    float noTargetCountdown;

    void AssignTarget()
    {
        target = Tower.UpdateTargetForAnyone(transform, targetMode, seeRange, enemyMask);

        if(target != null)
        {
            noTargetCountdown = goBackToSpawnPosAfter;
            MoveAgent(target.transform.position);
        }
    }

    void FixedUpdate()
    {
        anim.SetBool("moving", IsMoving);
        if (countdown > 0) countdown -= Time.fixedDeltaTime;

        if (target == null)
        {
            if(Vector3.Distance(transform.position, spawnPosition) > 5)
            {
                noTargetCountdown -= Time.fixedDeltaTime;
                if (noTargetCountdown <= 0)
                {
                    MoveAgent(spawnPosition);
                    noTargetCountdown = goBackToSpawnPosAfter;
                }
            }

            return;
        }

        if (Vector3.Distance(transform.position, target.transform.position) <= attackRange)
        {
            transform.LookAt(target.transform);
            MoveAgent(transform.position);

            if (countdown <= 0)
            {
                Attack();
            }
        }
    }
    
    public override void Die()
    {
        if (dead) return;
        dead = true;

        if(spawner != null)
        spawner.troops.Remove(this);

        base.Die();
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        countdown = 1f / attackRate;

        attacker.Attack(this, target);
    }
}
