using UnityEngine;

public class ShooterAttacker : Attacker {
    [SerializeField] AttackType attackType;
    public GameObject bulletPrefab;
    public Transform firePoint;

    public override void Attack(Character attacker, Character target)
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();

        bullet.target = target.Heart;
        bullet.characterMask = attacker.GetType() == typeof(Troop) ? (attacker as Troop).enemyMask : (attacker as Enemy).troopMask;
    }
    
    void OnValidate()
    {
        switch (attackType)
        {
            case AttackType.Arrow:
            case AttackType.Bullet:
            case AttackType.Laser:
            case AttackType.Magic:
                break;
            default:
                attackType = AttackType.Bullet;
                break;
        }
    }
}
