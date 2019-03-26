using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody;
    private Vector3 targetLookAtPoint;
    private float currentMoveSpeed;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.LookAt(targetLookAtPoint);
        playerRigidbody.MovePosition(playerRigidbody.position + transform.forward * currentMoveSpeed * Time.deltaTime);
    }

    public void MovePlayer(Vector3 lookAtPoint, float newMoveSpeed)
    {
        if (lookAtPoint == Vector3.zero)
        {
            lookAtPoint = transform.position + transform.forward;
        }

        lookAtPoint.y = transform.position.y;
        targetLookAtPoint = lookAtPoint;
        currentMoveSpeed = newMoveSpeed;
    }
}
