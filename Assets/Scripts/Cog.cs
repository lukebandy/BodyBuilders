using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cog : MonoBehaviour {
    public bool reverse;
    public float speed;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (!reverse)
            transform.Rotate(Vector3.forward, Time.deltaTime * speed);
        else
            transform.Rotate(Vector3.forward, Time.deltaTime * -speed);
    }
}
