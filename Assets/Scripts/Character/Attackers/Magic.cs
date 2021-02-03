using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Threading;

[System.Serializable]
public class Magic
{
    public string spellName;
    public string[] args;

    [Header("Only For Raising The Dead")]
    public ChanceBlueprint[] deadToRaise;

    public void DoSpell(Character attacker, Character target)
    {
        object[] allArguments;

        if(deadToRaise.Length > 0)
        {
            List<object> list = args.ToList<object>();
            list.Add(deadToRaise);

            allArguments = list.ToArray();
        }
        else
        {
            allArguments = args;
        }

        Magick.spellBook[spellName].spell.Invoke(attacker, target, allArguments);
    }
}

public class Magick
{
    public delegate void Spell(Character attacker, Character target, object[] args);
    public class SpellData
    {
        public struct Ingredient
        {
            public string name;
            public System.Type type;

            public Ingredient(string name, System.Type type)
            {
                this.name = name;
                this.type = type;
            }
        }

        public Ingredient[] ingredients;
        public Spell spell;

        public SpellData(Spell spell, params Ingredient[] ingredients)
        {
            this.spell = spell;
            this.ingredients = ingredients;
        }
    }

    public static Dictionary<string, SpellData> spellBook = new Dictionary<string, SpellData>()
    {
        {"Heal", new SpellData((attacker, target, args) =>
        {
            float healthRaise = args[0].ToFloat();
            float healRange = args[1].ToFloat();
            float healOthersBy = args[2].ToFloat();

            attacker.health += healthRaise;

            if(healRange > 0)
            {
                foreach(Collider col in Physics.OverlapSphere(attacker.transform.position, healRange, attacker.AllieMask))
                {
                   if(col == null)continue;

                   Character c = col.GetComponent<Character>();

                    if(c == null)continue;

                   c.health += healOthersBy;
                }
            }

        },

            new SpellData.Ingredient("Health Raise", typeof(float)),
            new SpellData.Ingredient("Heal Range", typeof(float)),
            new SpellData.Ingredient("Heal Others By", typeof(float))
        ) },

        {"Health Fill", new SpellData((attacker, target, args) =>
        {
            float spellWorkRange = args[0].ToFloat();

             attacker.health = attacker.startHealth;

           if(spellWorkRange > 0)
            {
            foreach(Collider col in Physics.OverlapSphere(attacker.transform.position, spellWorkRange, attacker.AllieMask))
                {
                    if(col == null)continue;
                Character c = col.GetComponent<Character>();
                    
                    if(c == null)continue;
                c.health = c.startHealth;
                }
            }
        },

            new SpellData.Ingredient("Spell Work Range", typeof(float))
            )},

        {"Teleport", new SpellData((attacker, target, args) =>
        {
            float maxDistance = args[0].ToFloat();
            float spellWorkRange = args[1].ToFloat();

            Teleport(attacker, maxDistance);

            foreach(Collider col in Physics.OverlapSphere(attacker.transform.position, spellWorkRange, attacker.AllieMask))
            {
                Teleport(col.GetComponent<Character>(), maxDistance);
            }
        },

            new SpellData.Ingredient("Max Distance", typeof(float)),
            new SpellData.Ingredient("Spell Work Range", typeof(float))
            )},

        {"Raise The Dead", new SpellData((attacker, target, args) =>
        {
            int amount = (int)args[0].ToFloat();
            float radius = args[1].ToFloat();
            ChanceBlueprint[] possibles = (ChanceBlueprint[])args[2];
            LayerMask pathMask = BuildManager.instance.pathMask;

            float? total = null;

            Character[] spawned = new Character[amount];
            Collider[] pathCols = Physics.OverlapSphere(attacker.transform.position, radius, pathMask);

            if(total == null)
            {
                foreach (var possible in possibles) total += possible.chance;
            }

            for(int i = 0; i < amount; i++)
            {
                Transform spawnPoint = pathCols[Random.Range(0, pathCols.Length)].transform;

                GameObject toSpawn = null;

                float rand = Random.Range(0, total.Value);
                float sum = 0;

                foreach(var possible in possibles)
                {
                    sum += possible.chance;

                    if(rand <= sum)
                    {
                        toSpawn = possible.prefab;
                    }
                }

                spawned[i] = Object.Instantiate(toSpawn, spawnPoint.position, Quaternion.identity).GetComponent<Character>();
            }

            foreach(Character c in spawned)
            {
                if(c.GetType() == typeof(Troop))
                {
                    Troop troop = c as Troop;
                    Troop oldTroop = (attacker.GetType() == typeof(Troop) ? attacker : target) as Troop;
                    
                    troop.targetMode = oldTroop.targetMode;
                    troop.enemyMask = oldTroop.enemyMask;

                    troop.spawner = oldTroop.spawner;

                    if (troop.spawner != null) troop.spawner.troops.Add(troop);
                }
            }
        },

            new SpellData.Ingredient("Amount", typeof(int)),
            new SpellData.Ingredient("Radius", typeof(float)),
            new SpellData.Ingredient("Possibles", typeof(ChanceBlueprint[]))
            )},

        {"Disintegrate", new SpellData((attacker, target, args) =>
          {
              target.Die();

              float range = args[0].ToFloat();

              if(range > 0)
              {
                  foreach(Collider col in Physics.OverlapSphere(attacker.transform.position, range, attacker.EnemyMask))
                  {
                      col.GetComponent<Character>().Die();
                  }
              }
          },

            new SpellData.Ingredient("Range", typeof(float)))
        },

        {"Hypnotize", new SpellData((attacker, target, args) =>
          {
              float range = args[0].ToFloat();
              float duration = args[1].ToFloat();

              if(range > 0)
              {
                  foreach(Collider col in Physics.OverlapSphere(attacker.transform.position, range, attacker.EnemyMask))
                  {
                      Hypnotize(col.GetComponent<Character>(), duration);
                  }
              }

              Hypnotize(attacker, duration);
          },

            new SpellData.Ingredient("Range", typeof(float)),
            new SpellData.Ingredient("Duration", typeof(float))
        )}
    };

    static void Teleport(Character character, float maxDistance)
    {
        var agent = character.GetComponent<NavMeshAgent>();

        if (!agent.hasPath) return;

        float remainingDistance = agent.remainingDistance;
        float distance = Random.Range(0, maxDistance);

        if (distance >= remainingDistance)
        {
            agent.transform.position = agent.destination;
        }

        var path = agent.path;

        int index = path.corners.Length - 1;

        Vector3 oldPos = agent.transform.position;

        for (int i = 0; i < path.corners.Length; i++)
        {
            agent.transform.position = path.corners[i];
            float remain = agent.remainingDistance;
            agent.transform.position = oldPos;

            if (remain < remainingDistance)
            {
                index = i - 1;
            }
        }

        if (index == path.corners.Length - 1) return;

        for (; ; index++)
        {
            Vector3 aim = path.corners[index + 1];
            float dist = Vector3.Distance(agent.transform.position, aim);

            if (dist < distance)
            {
                distance -= dist;

                agent.transform.position = aim;
            }
            else
            {
                Vector3 dir = (aim - agent.transform.position).normalized;

                agent.transform.position += dir * distance;

                break;
            }
        }
    }

    static void Hypnotize(Character target, float duration)
    {
        new Thread(() =>
        {
            LayerMask enemyMask = target.EnemyMask;

            target.EnemyMask = target.AllieMask;
            target.AllieMask = target.EnemyMask;

            Thread.Sleep((int)(duration * 1000));

            if (target == null) return;

            target.AllieMask = target.EnemyMask;
            target.EnemyMask = enemyMask;
        }
        );
    }
}

[System.Serializable]
public class ChanceBlueprint
{
    public GameObject prefab;
    public float chance;
}
