using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooks : MonoBehaviour {

    public GameObject template;
    public Transform bodypartFolder;
    public Transform outroFolder;
    public float spawnWait;
    public float speed = 1.0f;

    float spawnWaited = 0.0f;

    // Start is called before the first frame update
    void Start() {
        
    }

    private void OnEnable() {
        Instantiate(template, new Vector3(13, 5, -3.75f), Quaternion.Euler(Vector3.zero), transform);
        spawnWaited = 0;
    }

    // Update is called once per frame
    void Update() {
        spawnWaited += Time.deltaTime;

        if (spawnWaited >= spawnWait) {
            Instantiate(template, new Vector3(13, 5, -3.75f), Quaternion.Euler(Vector3.zero), transform);
            spawnWaited = 0;
        }

        foreach (Transform child in transform) {
            child.position += Vector3.left * Time.deltaTime * speed;
            // When the template is at the end of the rail
            if (child.position.x < -9.5f) {
                // Count how many body parts have been added to the template
                int count = 0;
                foreach(Transform bodypart in child) {
                    count += bodypart.childCount;
                }
                // If the body hasn't been completed, drop all the parts
                if (count < 6) {
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
                // If the body has been completed
                else {
                    // Calculate score 
                    int correct = 0;
                    foreach (Transform bodypart in child) {
                        if (bodypart.childCount > 0) { 
                            if (bodypart.name == bodypart.GetChild(0).name.Split('(')[0])
                                correct++;
                            // Change name so that this loop doesn't happen again
                            bodypart.GetChild(0).name = "Counted"; 
                        }
                    }
                    if (correct == 6)
                        correct += 4;
                    GameController.gameScore += correct;
                    // If it's now moved off screen
                    if (child.position.x < -14.0f) {
                        foreach (Transform spot in outroFolder) {
                            if (spot.childCount == 0) {
                                child.parent = spot;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
