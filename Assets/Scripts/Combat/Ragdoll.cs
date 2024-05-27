using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] CharacterController controller;

    Collider[] ragdollColliders;
    Rigidbody[] ragdollRigidbodies;
    
    void Start()
    {
        ragdollColliders = GetComponentsInChildren<Collider>(true);
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>(true);

        ToggleRagdoll(false);
    }

    public void ToggleRagdoll(bool isRagdoll)
    {
        foreach (Collider collider in ragdollColliders)
        {
            if (collider.gameObject.CompareTag("Ragdoll"))
            {
                collider.enabled = isRagdoll;
            }
        }

        foreach (Rigidbody rb in ragdollRigidbodies)
        {
            if (rb.gameObject.CompareTag("Ragdoll"))
            {
                rb.isKinematic = !isRagdoll;
                rb.useGravity = isRagdoll;
            }
        }

        controller.enabled = !isRagdoll;
        animator.enabled = !isRagdoll;
    }
}
