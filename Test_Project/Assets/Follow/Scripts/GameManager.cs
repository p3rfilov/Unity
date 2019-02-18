using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform ball;
    public Transform minion;
    public Transform platform;
    public Transform finish;
    public int minionCount;
    public float spawnHeight;
    public float force;
    public float moveFrequency;
    public float pushDistance;
    public float finishAvoidDistance;

    private Transform[] allMinions;
    private Transform ballInstance;

    // Start is called before the first frame update
    void Start()
    {
        allMinions = new Transform[minionCount];
        // spawn minions
        for (int i = 0; i < minionCount; i++)
        {
            Transform t = Instantiate(minion);
            t.position = GetRandomGroundPosition();
            // store minions
            allMinions[i] = t;
        }
        // spawn ball
        ballInstance = Instantiate(ball);
        ballInstance.position = GetRandomGroundPosition();

        // move minion towards the area behind the ball
        foreach (var item in allMinions)
        {
            StartCoroutine(MoveMinion(item));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // respawn minion/ball when it falls through

    }

    IEnumerator MoveMinion(Transform m)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, moveFrequency));
            Vector3 behind = Vector3.Normalize(ballInstance.position - finish.position) * pushDistance;
            Vector3 direction;
            if (Vector3.Distance(m.position, ballInstance.position + behind) > pushDistance)
            {
                if (Vector3.Distance(m.position, finish.position) < finishAvoidDistance)
                {
                    // avoid the hole
                    direction = Quaternion.AngleAxis(90, Vector3.up) * Vector3.Normalize(m.position - finish.position);
                    m.GetComponent<Rigidbody>().AddForce(direction * force * Time.deltaTime, ForceMode.Impulse);
                    yield return new WaitForSeconds(Random.Range(0f, moveFrequency));
                }
                if (Vector3.Distance(m.position, ballInstance.position) < pushDistance)
                    // if we are close to the ball but not behind it, move 90 degrees to the side
                    direction = Quaternion.AngleAxis(90, Vector3.up) * Vector3.Normalize(ballInstance.position - m.position);
                else
                    // when a minion is in position, start pushing the ball towards the hole
                    direction = Vector3.Normalize((ballInstance.position + behind) - m.position);
                m.GetComponent<Rigidbody>().AddForce(direction * force * Time.deltaTime, ForceMode.Impulse);
            }
        }
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
        Vector3 minPos = slab.position - (size / 2);
        Vector3 maxPos = slab.position + (size / 2);
        Vector3 position = 
            new Vector3(Random.Range(minPos.x, maxPos.x), spawnHeight, Random.Range(minPos.z, maxPos.z));
        return position;
    }
}
