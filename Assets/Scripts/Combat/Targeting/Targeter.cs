using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Targeter : MonoBehaviour
{
    [SerializeField] CinemachineTargetGroup cinemachineTargetGroup;

    List<Target> targets = new List<Target>();
    Camera mainCamera;
    
    public Target CurrentTarget { get; private set; }

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target otherTarget))
        {
            targets.Add(otherTarget);
            otherTarget.DestroyedEvent += RemoveTarget;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Target>(out Target otherTarget) && targets.Contains(otherTarget))
        {
            RemoveTarget(otherTarget);
        }
    }

    public bool SelectTarget()
    {
        if (targets.Count == 0) { return false; }

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach(Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            if (!target.GetComponentInChildren<Renderer>().isVisible)
            {
                continue;
            }

            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < Mathf.Infinity)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null) { return false; }

        CurrentTarget = closestTarget;
        cinemachineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

        return true;
    }

    public void Cancel()
    {
        if (CurrentTarget == null) { return; }

        cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
        CurrentTarget = null;
    }

    void RemoveTarget(Target target)
    {
        if (CurrentTarget == target)
        {
            cinemachineTargetGroup.RemoveMember(CurrentTarget.transform);
            CurrentTarget = null;
        }

        target.DestroyedEvent -= RemoveTarget;
        targets.Remove(target);
    }
}
