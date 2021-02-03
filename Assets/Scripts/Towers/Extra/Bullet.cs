using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour {
    [HideInInspector]public Transform target;
    public float speed = 40;
    public bool follows = true;
    public float damage, explosionDamage;
    public float explosionRange = 0;
    public LayerMask characterMask;
    public GameObject effect;
    Rigidbody rb;
    bool dead = false;

    public float lifeTime = 10;
    
    void Awake()
    {
        transform.LookAt(target);

        if (follows) InvokeRepeating("Follow", 0.04f, 0.04f);

        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.isKinematic = false;
        rb.useGravity = false;

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    void Follow()
    {
        if (target == null) return;

        transform.LookAt(target);

        if(Vector3.Distance(target.position, transform.position) <= 0.3f)
        {
            OnTriggerEnter(target.GetComponentInParent<Collider>());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (dead) return;

        Character c = other.GetComponent<Character>();

        if (c == null) return;

        HitCharacter(c);

        if(explosionRange > 0)
        {
            foreach(Collider col in Physics.OverlapSphere(transform.position, explosionRange, characterMask))
            {
                ExplodeAtCharacter(col.GetComponent<Character>());
            }
        }

        if (effect != null) Destroy(Instantiate(effect, transform.position, transform.rotation), 5);

        Die();
    }

    protected virtual float DieAfter { get { return 0; } }

    void Die()
    {
        dead = true;
        GetComponent<Collider>().enabled = false;

        Destroy(gameObject, DieAfter);
    }

    protected virtual void HitCharacter(Character c)
    {
        c.TakeDamage(damage, AttackType.Bullet);
    }

    protected virtual void ExplodeAtCharacter(Character c)
    {
        c.TakeDamage(explosionDamage, AttackType.Explosion);
    }

    void OnDrawGizmosSelected()
    {
        if (explosionRange == 0) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
