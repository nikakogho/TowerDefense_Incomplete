using UnityEngine;
using System.Collections;

public class SlowBullet : Bullet {
    [Range(0, 1)]
    public float slowDownFirstBy, slowDownOthersBy;

    public float effectLifetTime = 1;

    protected override void HitCharacter(Character c)
    {
        StartCoroutine(HaveEffectForTime(c, slowDownFirstBy));

        base.HitCharacter(c);
    }

    protected override void ExplodeAtCharacter(Character c)
    {
        StartCoroutine(HaveEffectForTime(c, slowDownOthersBy));

        base.ExplodeAtCharacter(c);
    }

    protected override float DieAfter
    {
        get
        {
            return effectLifetTime + 0.02f;
        }
    }

    IEnumerator HaveEffectForTime(Character c, float slowBy)
    {
        Slower slower = slowBy;

        c.AddSlower(slower);

        yield return new WaitForSeconds(effectLifetTime);

        if(c != null)
        c.RemoveSlower(slower);
    }
}

public class Slower
{
    public float value;

    public Slower(float value)
    {
        this.value = value;
    }

    public static implicit operator Slower(float value)
    {
        return new Slower(value);
    }
}
