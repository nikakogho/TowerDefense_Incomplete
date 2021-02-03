using UnityEngine;

public class Turret : Tower
{
    public GameObject toShoot;
    public Transform firePoint;
    public Enemy target;

    public Transform rotationPivot;

    Transform TurnAxis { get { return rotationPivot ?? transform; } }

    public float rotationSpeed = 120;
    
    void Awake()
    {
        InvokeRepeating("TargetUpdate", 0, 0.1f);
    }

    void TargetUpdate()
    {
        target = UpdateTarget(transform);
    }

    protected override void PrepareToAttack()
    {
        TargetUpdate();

        if (target == null) return;

        base.PrepareToAttack();
    }

    protected override void Attack()
    {
        Bullet bullet = Instantiate(toShoot, firePoint.position, rotationPivot.rotation).GetComponent<Bullet>();

        bullet.target = target.Heart;
        bullet.characterMask = enemyMask;
    }
    
    void FixedUpdate()
    {
        if(target != null)
        {
            Vector3 dir = target.transform.position - TurnAxis.position;
            dir.y = 0;

            TurnAxis.rotation = Quaternion.Slerp(TurnAxis.rotation, Quaternion.LookRotation(dir), rotationSpeed * Time.fixedDeltaTime);
        }
    }
}