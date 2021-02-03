using UnityEngine;
using UnityEngine.AI;

public class SorterOfCharacters : MonoBehaviour {
    public bool sort = false;

    public float minOffsetX;

    void OnValidate()
    {
        if (sort)
        {
            sort = false;

            Sort();
        }
    }
    
    class Pair
    {
        public Transform transform;
        public float height;

        public float offsetX;

        public Pair(Transform transform, float height)
        {
            this.transform = transform;
            this.height = height;
        }
    }

    void Sort()
    {
        int size = transform.childCount;

        Pair[] order = new Pair[size];

        for(int i = 0; i < size; i++)
        {
            Transform child = transform.GetChild(i);
            float scale = child.localScale.x;

            NavMeshAgent agent = child.GetComponent<NavMeshAgent>();
            float height = agent.height * scale;

            bool picked = false;

            Pair pair = new Pair(child, height);

            for(int j = 0; j < i; j++)
            {
                if(height < order[j].height)
                {
                    picked = true;

                    for(int k = i; k > j; k--)
                    {
                        order[k] = order[k - 1];
                    }

                    order[j] = pair;

                    break;
                }
            }

            if (!picked) order[i] = pair;

            float offsetX = Mathf.Max(minOffsetX, agent.radius * scale);

            pair.offsetX = offsetX;
        }

        float x = 0;

        foreach(Pair pair in order)
        {
            pair.transform.parent = null;
            pair.transform.parent = transform;
            pair.transform.position = transform.position + Vector3.right * x;

            x += pair.offsetX;
        }
    }
}
