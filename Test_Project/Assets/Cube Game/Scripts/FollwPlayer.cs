using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FollwPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Update()
    {
        transform.position = player.position + offset;

        if (Input.GetKey(KeyCode.Escape)) // shouldn't be here. Restarts the game
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
