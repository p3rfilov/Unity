using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Game : MonoBehaviour
{
    public Transform prefab;
    public KeyCode createKey = KeyCode.C;
    public KeyCode newGameKey = KeyCode.N;
    public KeyCode saveKey = KeyCode.S;
    public KeyCode loadKey = KeyCode.L;

    List<Transform> objects;
    string savePath;

    private void Awake()
    {
        objects = new List<Transform>();
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
        print(savePath);
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
        else if (Input.GetKeyDown(saveKey))
        {
            Save();
        }
        else if (Input.GetKeyDown(loadKey))
        {
            Load();
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
        using (var writer = new BinaryWriter(File.Open(savePath, FileMode.Create))) 
        {
            var dataWriter = new GameDataWriter(writer);
            dataWriter.Write(objects.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                Transform t = objects[i];
                dataWriter.Write(t.localPosition);
            }
        }
    }

    void Load()
    {
        BeginNewGame();
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            var dataReader = new GameDataReader(reader);
            int count = dataReader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Vector3 p = dataReader.ReadVector3();
                Transform t = Instantiate(prefab);
                t.localPosition = p;
                objects.Add(t);
            }
        }
    }
}
