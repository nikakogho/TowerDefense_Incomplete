using UnityEngine;

[DisallowMultipleComponent()]
public abstract class Attacker : MonoBehaviour
{
    public abstract void Attack(Character attacker, Character target);
}
