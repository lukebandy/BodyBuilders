using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

    public GameObject claw;
    Rigidbody holding;
    int score;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // Claw position
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.x = Mathf.Clamp(target.x, -11, 11);
        target.y = Mathf.Clamp(target.y, -2.5f, 8.3f);
        target.z = transform.position.z;
        transform.position = target;

        // Grab body part
        Debug.DrawRay(claw.transform.position, Vector3.forward * 5.0f, Color.red);
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(claw.transform.position, Vector3.forward, out hit, 5.0f)) {
                if (hit.transform.CompareTag("BodyPart")) {
                    holding = hit.transform.GetComponent<Rigidbody>();
                    holding.GetComponent<Collider>().enabled = false;
                }
            }
        }
        // Drop body part
        if (Input.GetMouseButtonUp(0) && holding != null) {
            RaycastHit hit;
            // If body part is above a template
            if (Physics.Raycast(holding.transform.position, Vector3.forward, out hit, 10.0f, 1 << 9) && hit.transform.childCount == 0) {
                // Attach the part to the template
                holding.useGravity = false;
                holding.velocity = Vector3.zero;
                holding.transform.parent = hit.transform;
                holding.transform.localPosition = new Vector3(0, 0, 0);
                holding.transform.localRotation = Quaternion.Euler(Vector3.zero);
                // Increase the score if the part is correctly placed
                if (holding.name.Split('(')[0] == hit.transform.name) {
                    GameController.gameScore++;
                }
            }
            // Drop back onto the belt
            else {
                holding.GetComponent<Collider>().enabled = true;
                holding.velocity = Vector3.zero;
            }
            // In both cases, no longer holding this part
            holding = null;
        }

        // If holding a body part, move it with the claw
        if (holding != null) {
            holding.transform.position = new Vector3(claw.transform.position.x, claw.transform.position.y, holding.transform.position.z);
        }
    }
}