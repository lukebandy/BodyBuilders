﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooks : MonoBehaviour {

    public GameObject template;
    public Transform bodypartFolder;
    public float spawnWait;
    public float speed = 1.0f;

    float spawnWaited = 0.0f;

    // Start is called before the first frame update
    void Start() {
        
    }

    private void Awake() {
        Instantiate(template, new Vector3(10, 7, 3), Quaternion.Euler(Vector3.zero), transform);
        spawnWaited = 0;
    }

    // Update is called once per frame
    void Update() {
        spawnWaited += Time.deltaTime;

        if (spawnWaited >= spawnWait) {
            Instantiate(template, new Vector3(10, 7, 3), Quaternion.Euler(Vector3.zero), transform);
            spawnWaited = 0;
        }

        foreach (Transform child in transform) {
            child.position += Vector3.left * Time.deltaTime * speed;
            if (child.position.x < -8) {
                foreach (Transform bodypart in child) {
                    if (bodypart.name != "Body" && bodypart.childCount == 1) {
                        bodypart.GetChild(0).GetComponent<Collider>().enabled = true;
                        bodypart.GetChild(0).GetComponent<Rigidbody>().useGravity = true;
                        bodypart.GetChild(0).GetComponent<Rigidbody>().velocity = Vector3.zero;
                        bodypart.GetChild(0).transform.position = new Vector3(bodypart.GetChild(0).transform.position.x,
                            bodypart.GetChild(0).transform.position.y, -2.5f);
                        bodypart.GetChild(0).parent = bodypartFolder;
                    }
                }
            }
        }
    }
}
