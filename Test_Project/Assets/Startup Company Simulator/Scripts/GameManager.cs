using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Transform ball;
    public Transform minion;
    public Transform platform;
    public Transform finish;
    public int minionCount = 40;
    public float spawnHeight = 3f;
    public float force = 150f;
    public float moveFrequency = 1f;
    public float pushDistance = 1f;
    public float avoidDistance = 5f;
    public int jumpAngle = 15;

    private Vector3 ballPos;
    private Vector3 ballVelocity;
    private Vector3 behindPos;
    private Vector3 velocityThreshold = new Vector3(0.25f, 0.0f, 0.25f);
    private float inherentBallVelocity = 0.5f;

    private Transform[] allMinions;
    private Transform ballInstance;
    private float respawnAlt = -500f;
    private KeyCode restartKey = KeyCode.R;

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

    void FixedUpdate()
    {
        // respawn ball when it falls through
        if (ballInstance.position.y < respawnAlt)
        {
            ballInstance.position = GetRandomGroundPosition();
        }
        if (Input.GetKeyDown(restartKey))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        ballPos = ballInstance.position;
        ballVelocity = ballInstance.GetComponent<Rigidbody>().velocity;
        behindPos = Vector3.Normalize(ballPos - finish.position) * pushDistance;
    }

    IEnumerator MoveMinion(Transform m)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, moveFrequency));
            yield return new WaitForFixedUpdate();

            float distanceToBall = Vector3.Distance(m.position, ballPos + behindPos);
            float thresholdDist = Vector3.Distance(Vector3.zero, velocityThreshold);
            float velocityDist = Vector3.Distance(ballPos, ballPos + ballVelocity);
            float ballToFinishDist = Vector3.Distance(ballPos, finish.position);
            float minionToFinishDist = Vector3.Distance(m.position, finish.position);
            Quaternion minionJumpQuat = Quaternion.AngleAxis(jumpAngle, Vector3.Cross(finish.position - m.position, Vector3.up));
            Rigidbody body = m.GetComponent<Rigidbody>();
            Vector3 direction;

            // respawn minion when it falls through
            if (m.position.y < respawnAlt)
            {
                m.position = GetRandomGroundPosition();
            }
            else if (distanceToBall > pushDistance)
            {
                // avoid the hole
                if (Vector3.Distance(m.position, finish.position) < avoidDistance)
                {
                    direction = Quaternion.AngleAxis(90, Vector3.up) * Vector3.Normalize(m.position - finish.position);
                    body.AddForce(minionJumpQuat * direction * force * Time.deltaTime, ForceMode.Impulse);
                    yield return new WaitForSeconds(Random.Range(0f, moveFrequency));
                }

                // if the ball is fast-moving, try to intercept it
                if (thresholdDist < velocityDist && ballToFinishDist > minionToFinishDist)
                {
                    direction = Vector3.Normalize(ballVelocity) * inherentBallVelocity;
                    body.AddForce(minionJumpQuat * direction * force * Time.deltaTime, ForceMode.Impulse);
                    yield return new WaitForSeconds(Random.Range(0f, moveFrequency));
                }

                if (ballToFinishDist > minionToFinishDist && Vector3.Distance(m.position, ballPos) < avoidDistance)
                    // if we are close to the ball but not behind it, move 90 degrees to the side
                    direction = Quaternion.AngleAxis(90, Vector3.up) * Vector3.Normalize(ballPos - m.position);
                else
                    // when a minion is in position, start pushing the ball towards the hole
                    direction = Vector3.Normalize((ballPos + behindPos) - m.position);
                body.AddForce(minionJumpQuat * direction * force * Time.deltaTime, ForceMode.Impulse);
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
