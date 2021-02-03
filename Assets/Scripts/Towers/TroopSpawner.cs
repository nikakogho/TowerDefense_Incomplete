using UnityEngine;
using System.Collections.Generic;

public class TroopSpawner : Tower {
    public int maxAmount = 3;
    public GameObject prefab;
    public LayerMask pathMask;
    public List<Troop> troops = new List<Troop>();

    protected override void PrepareToAttack()
    {
        for (int i = troops.Count - 1; i > -1; i--)
        {
            if (troops[i] == null) troops.RemoveAt(i);
        }

        if(troops.Count < maxAmount)
        base.PrepareToAttack();
    }

    protected override void Attack()
    {
        Vector3 pathPoint;

        Collider[] possibles = Physics.OverlapSphere(transform.position, range, pathMask);

        if (possibles.Length > 0)
        {
            pathPoint = possibles[Random.Range(0, possibles.Length)].transform.position;
        }
        else return;

        Troop troop = Instantiate(prefab, pathPoint, Quaternion.identity).GetComponent<Troop>();
        troops.Add(troop);
        
        troop.targetMode = targetMode;
        troop.enemyMask = enemyMask;
        troop.spawner = this;
    }

    public override void OnTargetModeChanged()
    {
        foreach (Troop troop in troops) troop.targetMode = targetMode;
    }
}
