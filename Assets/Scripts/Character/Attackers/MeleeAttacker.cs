using UnityEngine;

public class MeleeAttacker : Attacker
{

    [SerializeField]AttackType attackType;
    public float damage;

    [Header("Splash")]
    public float splashRange;
    public float splashDamage;
    public LayerMask characterMask;

    [Header("Zombie Mode")]
    /// <summary>
    /// Anyone this guy kills becomes one of his (or hers (or its) ) allies
    /// </summary>
    /// 
    public bool zombieMode = false;
    public GameObject zombieModePrefab;

    public override void Attack(Character attacker, Character target)
    {
        if (target == null) return;

        target.TakeDamage(damage, attackType);

        if (splashRange > 0 && splashDamage > 0)
        {
            foreach (Collider col in Physics.OverlapSphere(transform.position, splashRange, characterMask))
            {
                col.GetComponent<Character>().TakeDamage(splashDamage, AttackType.Splash);
            }
        }

        if (zombieMode && target.health <= 0)
        {
            GameObject clone = Instantiate(zombieModePrefab, target.Heart.position, target.transform.rotation);
            WaveLauncher.instance.amount++;

            Character c = clone.GetComponent<Character>();

            if(c.GetType() == typeof(Troop))
            {
                Troop troop = (Troop)c;
                Troop ourTroop = GetComponent<Troop>();

                troop.spawner = ourTroop.spawner;
                troop.targetMode = ourTroop.targetMode;
                troop.enemyMask = ourTroop.enemyMask;

                ourTroop.spawner.troops.Add(troop);
            }
        }
    }

    void OnValidate()
    {
        switch (attackType)
        {
            case AttackType.Arrow:
            case AttackType.Bullet:
            case AttackType.Explosion:
                attackType = AttackType.NoWeapon;
                break;
        }
    }
}
