using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCog : MonoBehaviour
{
    public float rotateSpeed; //set it in the  inspector
    public bool rotateClockwise = true;
 
    void Update () {
        rotate();
    }
 
 
    void rotate() {
        float direction = 1;
        if (!rotateClockwise)
		{
            direction = -1;
        }
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime * direction);
    }
}
