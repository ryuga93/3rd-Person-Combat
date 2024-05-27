using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] float smoothTime = 0.3f;

    Vector3 impact;
    Vector3 dampingVelocity;
    float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity;

    // Update is called once per frame
    void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }

        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, smoothTime);

        if (impact.sqrMagnitude < 0.2f * 0.2f && agent != null)
        {
            agent.enabled = true;
        }
    }

    public void AddForce(Vector3 force)
    {
        impact += force;

        if (agent != null)
        {
            agent.enabled = false;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    public void Reset()
    {
        impact = Vector3.zero;
        verticalVelocity = 0;
    }
}
