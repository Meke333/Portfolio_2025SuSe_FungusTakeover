using Helper;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Entity;

/// <summary> 
/// Adds helpful functions for navmesh navigation/movement
/// </summary>
public static class NavMeshAgentHelper
{
    public static void FaceTarget(NavMeshAgent agent, Vector3 targetPosition)
    {
        //exit early if no navmesh agent found?
        if (agent == null)
            return;
        //exit early if no target position was requested
        if (targetPosition == null)
            return;
        Vector3 lookPos = targetPosition - agent.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, rotation, agent.angularSpeed);
    }
    /// <summary> Set target position of an agent </summary>
    public static void NavMeshAgentWalk(NavMeshAgent agent, Vector3 targetPosition)
    {
        agent.angularSpeed = 999;

        agent.SetDestination(targetPosition);
    }
    /// <summary> Set target position of an agent with stun flags/stun </summary>
    public static void NavMeshAgentWalk(NavMeshAgent agent, Vector3 targetPosition, bool isStunned, CharActions stunFlags)
    {
        if (isStunned && !stunFlags.HasFlag(CharActions.Rotate)) //cant rotate & stunned
            agent.angularSpeed = 0;
        else
            agent.angularSpeed = 999;
        
        targetPosition.y = 0f;

        agent.SetDestination(targetPosition);

        if (isStunned)
            agent.velocity = Vector3.zero;
    }
    /// <summary> Set target position of an agent with stun flags/stun and knockback/environment influence </summary>
    public static void NavMeshAgentWalk(NavMeshAgent agent, Vector3 targetPosition, bool isStunned, CharActions stunFlags, 
        Vector3 knockbackVector, AnimationCurve curve, float curvePercent)
    {
        if (isStunned && !stunFlags.HasFlag(CharActions.Rotate)) //cant rotate & stunned
            agent.angularSpeed = 0;
        else
            agent.angularSpeed = 999;

        agent.SetDestination(targetPosition);

        if (isStunned)
            agent.velocity = Vector3.zero;

        Knockback(agent.gameObject.transform, knockbackVector, curve, curvePercent);
    }
    
    /// <summary> Modify the position of a transform based on an inputVector like knockback or environment factors </summary>
    public static void Knockback(Transform transform, Vector3 knockbackVector)
    {
        if (knockbackVector == Vector3.zero)
            return;
        transform.position += knockbackVector * MyTime.deltaTime;
    }
    /// <summary> Modify the position of a transform based on an inputVector like knockback or environment factors AND make it smoothTM based on a curve </summary>
    public static Vector3 Knockback(Transform transform, Vector3 knockbackVector, AnimationCurve curve, float curvePercent)
    {
        knockbackVector.y = 0;
        
        if (knockbackVector == Vector3.zero)
            return Vector3.zero;
        //clamp curve percent from 0 to 1
        curvePercent = Mathf.Clamp01(curvePercent);
        float curveValue = curve.Evaluate(curvePercent);
        
        if (curveValue == float.NaN)
            return Vector3.zero;
        Vector3 incrementalKnockback = knockbackVector * curveValue;
        transform.position += incrementalKnockback * MyTime.deltaTime;

        return knockbackVector - incrementalKnockback;
    }

    /// <summary> Move on the navmesh based on an input vector </summary>
    public static void ControllerMovement(Transform transform, Vector2 movementInput, float rotationSpeed, float speed, 
        bool isStunned, CharActions stunFlags)
    {
        if (movementInput == Vector2.zero)
            return;
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 newPos = currentPos + movementInput * MyTime.deltaTime * speed;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(new Vector3(newPos.x, transform.position.y, newPos.y), out hit, 3f, NavMesh.AllAreas))
        {
            //return early if not on navmesh (not sure this works tbh)
            if ((transform.position - hit.position).magnitude < .02f)
                return;

            //movement
            if (!isStunned)
            {
                transform.position = new Vector3(hit.position.x, transform.position.y, hit.position.z);
            }
            //rotation
            if ((isStunned && !stunFlags.HasFlag(CharActions.Rotate)) || isStunned)
                return;

            Quaternion movementRotation = transform.rotation;
            
            if (movementInput.magnitude > 0.1)
            {
                movementRotation = Quaternion.LookRotation(new Vector3(movementInput.x, 0, movementInput.y));
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, movementRotation, rotationSpeed * MyTime.deltaTime);

        }
    }
}
public static class MyTime
{
    public static float maxDT = 1f / 60f;
    public static float TargetFrameRate
    {
        get { return 1f / maxDT; }
        set { maxDT = 1f / value; }
    }
    public static float deltaTime
    {
        get
        {
            if (Time.deltaTime > maxDT)
                return maxDT;
            return Time.deltaTime;
        }
    }
}
