using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Belt : MonoBehaviour {

    public float speed;
    public bool reverse;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void OnCollisionStay(Collision collision) {
        if (!reverse)
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.right * speed;
        else
            collision.gameObject.GetComponent<Rigidbody>().velocity = Vector3.left * speed;
    }
}
