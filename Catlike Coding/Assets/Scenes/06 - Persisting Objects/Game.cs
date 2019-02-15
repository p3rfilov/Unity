using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Game : MonoBehaviour
{
    public Transform prefab;
    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;

    List<Transform> objects;
    string savePath;

    private void Awake()
    {
        objects = new List<Transform>();
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }

    private void Update()
    {
        if (Input.GetKeyDown(createKey))
        {
            CreateObject();
        }
        else if (Input.GetKeyDown(newGameKey))
        {
            BeginNewGame();
        }
    }

    void CreateObject()
    {
        Transform t = Instantiate(prefab);
        t.localPosition = Random.insideUnitSphere * 5f;
        t.localRotation = Random.rotation;
        t.localScale = Vector3.one * Random.Range(0.1f, 1f);
        objects.Add(t);
    }

    void BeginNewGame()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i].gameObject);
        }
        objects.Clear();
    }

    void Save()
    {
        using 
        (
            var writer = new BinaryWriter(File.Open(savePath, FileMode.Create));
        ) {}
    }
}
