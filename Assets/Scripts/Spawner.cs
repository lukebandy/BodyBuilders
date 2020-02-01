using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public List<GameObject> bodyParts;
    public Transform bodyPartsFolder;

    public float spawnWait;
    private float spawnWaited;

    // Start is called before the first frame update
    void Start() {
        
    }

    private void Awake() {
        Instantiate(bodyParts[Random.Range(0, bodyParts.Count)], transform.position, transform.rotation, bodyPartsFolder);
    }

    // Update is called once per frame
    void Update() {
        spawnWaited += Time.deltaTime;
        if (spawnWaited >= spawnWait) {
            Instantiate(bodyParts[Random.Range(0, bodyParts.Count)], transform.position, transform.rotation, bodyPartsFolder);
            spawnWaited = Random.Range(-1.5f, 0f);
        }
    }
}
