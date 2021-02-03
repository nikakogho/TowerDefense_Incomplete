using UnityEngine;

public class MagicAttacker : Attacker {
    public MagicBlurpint[] spells;
    float total = 0;

    void Awake()
    {
        foreach(MagicBlurpint spell in spells)
        {
            total += spell.chance;
        }
    }

    public override void Attack(Character attacker, Character target)
    {
        float rand = Random.Range(0, total);
        float sum = 0;

        Magic chosenMagic = null;

        foreach(var spell in spells)
        {
            sum += spell.chance;

            if(rand <= sum)
            {
                chosenMagic = spell.magic;
                break;
            }
        }

        chosenMagic.DoSpell(attacker, target);
    }

    [System.Serializable]
    public class MagicBlurpint
    {
        public Magic magic;
        public float chance;
    }
}
