using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class Claw : MonoBehaviour {

    Rigidbody holding;
    public GameObject closed;
    public GameObject open;

    // Start is called before the first frame update
    void Start() {
        
    }

    public void Move() {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target.x = Mathf.Clamp(target.x, -12, 12);
        target.y = Mathf.Clamp(target.y, -2.5f, 8.3f);
        target.z = transform.position.z;
        transform.position = target;
    }

    // Update is called once per frame
    void Update() {
        // Claw position
        Move();

        // Animate
        closed.SetActive(Input.GetMouseButton(0));
        open.SetActive(!Input.GetMouseButton(0));

        // Grab body part
        if (Input.GetMouseButtonDown(0)) {
            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, new Vector3(1, 1, 5)/2, Quaternion.identity, 1 << 8);
            if (hitColliders.Length > 0) {
                holding = hitColliders[0].transform.GetComponent<Rigidbody>();
                holding.GetComponent<Collider>().enabled = false;
                holding.angularVelocity = Vector3.zero;
            }
        }
        // Drop body part
        if (holding != null)
            Debug.DrawRay(holding.transform.position, Vector3.forward * 5.0f, Color.blue);
        if (Input.GetMouseButtonUp(0) && holding != null) {
            RaycastHit hit;
            // If body part is above a template
            if (Physics.Raycast(holding.transform.position, Vector3.forward, out hit, 10.0f, 1 << 9) && hit.transform.childCount == 0) {
                // Attach the part to the template
                holding.useGravity = false;
                holding.velocity = Vector3.zero;
                holding.transform.parent = hit.transform;
                holding.transform.localPosition = new Vector3(0, 0, 0);
                holding.transform.localRotation = Quaternion.Euler(0, 180, 0);
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
            holding.transform.position = new Vector3(transform.position.x, transform.position.y, holding.transform.position.z);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 5));
    }
}