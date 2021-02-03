using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(Attacker))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CapsuleCollider))]
public abstract class Character : MonoBehaviour {
    public float health, attackRate, attackRange, moveSpeed;
    public AttackTypeArmor armor;
    protected float countdown = 0;
    protected bool dead = false;
    NavMeshAgent agent;

    protected bool IsMoving { get { return agent.velocity.sqrMagnitude > 0.2f; } }
    [HideInInspector] public float startHealth;


    public LayerMask AllieMask
    {
        get
        {
            return gameObject.layer;
        }
        set
        {
            gameObject.layer = value;
        }
    }

    public abstract LayerMask EnemyMask { get; set; }

    public GameObject deathEffect;
    protected Animator anim;
    protected Attacker attacker;

    List<Slower> slowers = new List<Slower>();

    public Transform heart;

    public Transform Heart { get { return heart ?? transform; } }

    public void AddSlower(Slower slower)
    {
        slowers.Add(slower);

        float slowestValue = 1;

        foreach (Slower s in slowers)
        {
            if (s.value < slowestValue) slowestValue = s.value;
        }

        agent.speed = moveSpeed * slowestValue;
    }

    public void RemoveSlower(Slower slower)
    {
        slowers.Remove(slower);

        float slowestValue = 1;

        foreach(Slower s in slowers)
        {
            if (s.value < slowestValue) slowestValue = s.value;
        }

        agent.speed = moveSpeed * slowestValue;
    }

    void Awake()
    {
        anim = GetComponent<Animator>();
        attacker = GetComponent<Attacker>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;
        agent.angularSpeed = 180;

        startHealth = health;
    }

    public void TakeDamage(float damage, AttackType attackType)
    {
        if (dead) return;

        health -= damage * (1f - armor[attackType]);

        if (health <= 0) Die();
    }

    public virtual void Die()
    {
        dead = true;

        if (deathEffect != null) Destroy(Instantiate(deathEffect, transform.position, transform.rotation), 5);

        Destroy(gameObject);
    }

    protected void MoveAgent(Vector3 point)
    {
        if (!agent.isOnNavMesh) return;

        agent.SetDestination(point);

        agent.velocity = agent.desiredVelocity;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    [System.Serializable]
    public struct AttackTypeArmor
    {
        [Range(0, 1)]
        public float noWeapon, splash, wood, stone, metal, arrow, bullet, fire, explosion, laser, magic;

        public float this[AttackType type]
        {
            get
            {
                switch (type)
                {
                    case AttackType.NoWeapon: return noWeapon;
                    case AttackType.Splash: return splash;
                    case AttackType.Wood: return wood;
                    case AttackType.Stone: return stone;
                    case AttackType.Metal: return metal;
                    case AttackType.Arrow: return arrow;
                    case AttackType.Bullet: return bullet;
                    case AttackType.Fire: return fire;
                    case AttackType.Explosion: return explosion;
                    case AttackType.Laser: return laser;
                    case AttackType.Magic: return magic;
                    default: throw new System.ArgumentException("There Is No Such AttackType!");
                }
            }
        }
    }
}

public enum AttackType { NoWeapon, Splash, Wood, Stone, Metal, Arrow, Bullet, Fire, Explosion, Laser, Magic }
