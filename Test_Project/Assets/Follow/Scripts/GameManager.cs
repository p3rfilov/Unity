using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform ball;
    public Transform minion;
    public Transform platform;
    public int minionCount;

    // Start is called before the first frame update
    void Start()
    {
        // spawn minions
        //for (int i = 0; i < minionCount; i++)
        //{
        //    Transform t = Instantiate(minion);
        //    t.localPosition = Random.
        //}
        // spawn ball
        GetRandomGroundPosition();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // move minion towards the area behind the ball
        // when a minion is in position, start pushing the ball towards the hole
        // respawn minion/ball when it falls through
    }

    Vector3 GetRandomGroundPosition()
    {
        Transform[] children = platform.GetComponentsInChildren<Transform>();
        var groundSlabs = new List<Transform>();
        foreach (var item in children)
        {
            if (item.tag == "Ground")
                groundSlabs.Add(item);
        }

        Transform slab = groundSlabs[Random.Range(0, groundSlabs.Count)];
        Vector3 size = slab.GetComponent<Collider>().bounds.size;
        Debug.Log(slab);
        Debug.Log(size);
        return size;
    }
}
