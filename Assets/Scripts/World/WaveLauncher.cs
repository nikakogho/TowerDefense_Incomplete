using UnityEngine;
using System.Collections;

public class WaveLauncher : MonoBehaviour {
    public static WaveLauncher instance;
    public Wave[] waves;
    int index = -1;
    Wave wave;
    int spawning = 0;
    public int amount = 0;

    public Transform spawnPoint, reachPlace;

    Transform waveParent;

    public void EnemyDied()
    {
        amount--;

        CheckForEnd();
    }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        NextWave();

        InvokeRepeating("CheckForEnd", 10, 4);
    }

    void NextWave()
    {
        index++;

        if (index == waves.Length)
        {
            //you win

            Destroy(this);
            return;
        }

        wave = waves[index];

        SpawnWave();
    }

    void SpawnWave()
    {
        waveParent = new GameObject("Wave " + (index + 1)).transform;

        foreach(EnemyGroup group in wave.groups)
        {
            StartCoroutine(SpawnGroup(group, waveParent));
        }
    }

    IEnumerator SpawnGroup(EnemyGroup group, Transform waveParent)
    {
        spawning++;

        yield return new WaitForSeconds(group.startSpawningAfter);

        Transform parent = new GameObject(group.prefab.name + "s").transform;
        parent.parent = waveParent;

        amount += group.amount;

        for (int i = 0; i < group.amount; i++)
        {
            Instantiate(group.prefab, spawnPoint.position, spawnPoint.rotation, parent);

            yield return new WaitForSeconds(group.spawnDelta);
        }

        spawning--;

        CheckForEnd();
    }

    void CheckForEnd()
    {
        if (spawning == 0 && amount == 0)
        {
            if (waveParent != null)
            {
                bool stillHasChild = false;

                for(int i = 0; i < waveParent.childCount; i++)
                {
                    if(waveParent.GetChild(i).childCount > 0)
                    {
                        stillHasChild = true;
                        break;
                    }
                }

                if (stillHasChild) return;

                Destroy(waveParent.gameObject);
            }

            NextWave();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public EnemyGroup[] groups;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public GameObject prefab;
        public int amount;
        public float startSpawningAfter;
        public float spawnDelta;
    }
}
