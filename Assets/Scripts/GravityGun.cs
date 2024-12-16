using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] float maxGravityDist = 10f;
    [SerializeField] float throwForce = 20f;
    [SerializeField] float lerpSpeed = 10f;
    [SerializeField] Transform objectHolder;

    Rigidbody grabbedRB;


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (grabbedRB)
        {
            grabbedRB.MovePosition(Vector3.Lerp(grabbedRB.position, objectHolder.transform.position, Time.deltaTime * lerpSpeed));
        }
        if (Input.GetMouseButtonDown(1))
        {
            grabbedRB.isKinematic = false;
            grabbedRB.AddForce(camera.transform.forward * throwForce, ForceMode.VelocityChange);
            grabbedRB = null;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (grabbedRB)
            {
                grabbedRB.isKinematic = false;
                grabbedRB = null;   
            }
            else
            {
                RaycastHit hit;
                Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
                if (Physics.Raycast(ray, out hit, maxGravityDist))
                {
                    grabbedRB = hit.collider.gameObject.GetComponent<Rigidbody>();
                    if (grabbedRB)
                    {
                        grabbedRB.isKinematic = true;
                    }



                }

            }
        }
    }
}
    

