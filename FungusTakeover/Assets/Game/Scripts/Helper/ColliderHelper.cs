using Helper;
using System.Collections.Generic;
using Game.Entity;
using UnityEngine;
using Game.Interface;
using System;

public static class ColliderHelper //Damn this class is a mess... making methods for one specific case...
{
    /// <summary>
    /// Returns the damageable entity type
    /// </summary>
    public static bool DamageAbleType(EntityContextType ownType, EntityContextType otherType)
    {
        switch (ownType)
        {
            case EntityContextType.Player:
                return otherType.HasFlag(EntityContextType.Enemy);
            case EntityContextType.Ally:
                return otherType.HasFlag(EntityContextType.Enemy);
            case EntityContextType.Enemy:
                return !otherType.HasFlag(EntityContextType.Enemy);
        }

        return false;
    }

    public static bool TypeToGet(EntityContextType desiredType, EntityContextType toCheck)
    {
        return toCheck.HasFlag(desiredType);
    }

    /// <summary>
    /// Returns all hit colliders that have an EntityBase somewhere in their parent object
    /// and that match the ContextType type
    /// </summary>
    public static Collider[] Get3DColliderArray(Vector3 position, float range, EntityContextType type)
    {
        List<Collider> result = new List<Collider>();
        Collider[] hitColliders = Physics.OverlapSphere(position, range);

        foreach (var hitCollider in hitColliders)
        {
            EntityBase temp = hitCollider.GetComponentInParent<EntityBase>();

            if (temp == null)
                continue;

            if (!DamageAbleType(type,temp.ContextType))
                continue;

            result.Add(hitCollider);
        }

        return result.ToArray();
    }
    public static Collider[] Get3DColliderArray(Collider collider, EntityContextType type)
    {
        List<Collider> result = new List<Collider>();
        Collider[] hitColliders = CastOverlap(collider);

        foreach (var hitCollider in hitColliders)
        {
            EntityBase tempBase = GetComponent<EntityBase>(hitCollider);

            if (tempBase == null)
                continue;

            if (!DamageAbleType(type, tempBase.ContextType))
                continue;

            result.Add(hitCollider);
        }

        return result.ToArray();
    }
    public static Collider[] Get3DColliderArray(Collider collider, Vector3 center, float range, EntityContextType type)
    {
        List<Collider> result = new List<Collider>();
        Collider[] hitColliders;
        if (collider == null)
            hitColliders = Physics.OverlapSphere(center, range);
        else
            hitColliders = CastOverlap(collider, center);

        foreach (var hitCollider in hitColliders)
        {
            EntityBase tempBase = GetComponent<EntityBase>(hitCollider);

            if (tempBase == null)
                continue;

            if (!DamageAbleType(type, tempBase.ContextType))
                continue;

            result.Add(hitCollider);
        }

        return result.ToArray();
    }

    public static Collider[] Get3DColliderArrayForSpecificType(Vector3 position, float range, EntityContextType type)
    {
        List<Collider> result = new List<Collider>();
        Collider[] hitColliders = Physics.OverlapSphere(position, range);

        foreach (var hitCollider in hitColliders)
        {
            EntityBase temp = hitCollider.GetComponentInParent<EntityBase>();

            if (temp == null)
                continue;

            if (!TypeToGet(type, temp.ContextType))
                continue;

            result.Add(hitCollider);
        }

        return result.ToArray();
    }


    /// <summary>
    /// Returns all hit colliders that have an EntityBase somewhere in their parent object
    /// and that match the ContextType type
    /// 
    /// //Why make it one specific case, make it generic
    /// </summary>
    public static Collider[] Get3DColliderArraySporeable(Vector3 position, float range, EntityContextType type)
    {
        List<Collider> result = new List<Collider>();
        Collider[] hitColliders = Physics.OverlapSphere(position, range);

        foreach (var hitCollider in hitColliders)
        {
            ISporeable temp = GetComponent<ISporeable>(hitCollider);
            EntityBase tempBase = GetComponent<EntityBase>(hitCollider);

            if (temp == null)
                continue;

            if (tempBase != null) //only check for if you can hit it, if it's an entity
            {
                if (!DamageAbleType(type,tempBase.ContextType))
                    continue;
            }
            
            result.Add(hitCollider);
        }

        return result.ToArray();
    }
    public static Collider[] Get3DColliderArray(Collider collider)
    {
        List<Collider> result = new List<Collider>();
        Collider[] hitColliders = CastOverlap(collider);

        foreach (var hitCollider in hitColliders)
        {
            EntityBase tempBase = GetComponent<EntityBase>(hitCollider);

            if (tempBase == null)
                continue;
            
            result.Add(hitCollider);
        }

        return result.ToArray();
    }

    public static Collider[] CastOverlap(Collider collider)
    {
        switch (collider)
        {
            case CapsuleCollider:
                return null;
            case BoxCollider box:
                Vector3 worldCenter = box.transform.TransformPoint(box.center);
                Vector3 worldHalfExtents = Vector3.Scale(box.size, box.transform.lossyScale) * 0.5f;
                return Physics.OverlapBox(worldCenter, worldHalfExtents, box.transform.rotation);
            case SphereCollider sphere:
                return Physics.OverlapSphere(sphere.center, sphere.radius);
            default:
                return null;
        }
    }
    public static Collider[] CastOverlap(Collider collider, Vector3 center)
    {
        switch (collider)
        {
            case CapsuleCollider:
                return null;
            case BoxCollider box:
                Vector3 worldHalfExtents = Vector3.Scale(box.size, box.transform.lossyScale) * 0.5f;
                return Physics.OverlapBox(center, worldHalfExtents, box.transform.rotation);
            case SphereCollider sphere:
                return Physics.OverlapSphere(center, sphere.radius);
            default:
                return null;
        }
    }
    public static void DrawGizmos(Collider collider)
    {
        switch (collider)
        {
            case CapsuleCollider:
                break;
            case BoxCollider box:
                Gizmos.DrawCube(box.center,box.size);
                break;
            case SphereCollider sphere:
                Gizmos.DrawSphere(sphere.center,sphere.radius);
                break;
            default:
                break;
        }
    }
    public static void DrawGizmos(Collider collider, Vector3 center)
    {
        switch (collider)
        {
            case CapsuleCollider:
                break;
            case BoxCollider box:
                Gizmos.DrawCube(center, Vector3.Scale(box.size, box.transform.lossyScale));
                break;
            case SphereCollider sphere:
                Gizmos.DrawSphere(center,sphere.radius);
                break;
            default:
                break;
        }
    }
    public static void DrawGizmos(Vector3 center, float radius)
    {
        Gizmos.DrawSphere(center, radius);
    }
    public static void DrawGizmos(Vector3 center, Vector3 size)
    {
        Gizmos.DrawCube(center, size);
    }



    public static Collider2D[] GetColliderArray(Transform hitbox, float angle, float range, EntityContextType type)
    {
        List<Collider2D> result = new List<Collider2D>();
        Collider2D[] hitColliders;
        hitColliders = Physics2D.OverlapCircleAll(hitbox.position, angle);

        //Probably Problematic Obacht!
        for (int i = 0; i < hitColliders.Length; i++)
        {
            EntityBase temp = hitColliders[i].GetComponentInParent<EntityBase>();
            if (temp == null)
                continue;

            if (!temp.ContextType.HasFlag(type))
                continue;
            
            result.Add(hitColliders[i]);
        }

        return result.ToArray();
    }
    
    public static Collider2D[] GetColliderArray(Vector3 position, Vector3 range, EntityContextType type = default)
    {
        List<Collider2D> result = new List<Collider2D>();
        Collider2D[] hitColliders;
        hitColliders = Physics2D.OverlapBoxAll(position, range, 0);

        //Probably Problematic Obacht!
        for (int i = 0; i < hitColliders.Length; i++)
        {
            EntityBase temp = hitColliders[i].GetComponentInParent<EntityBase>();
            if (temp == null)
                continue;

            if (type != default && !temp.ContextType.HasFlag(type))
                continue;
            
            result.Add(hitColliders[i]);
        }

        return result.ToArray();
    }

    public static T GetComponent<T>(Collider2D hitCollider)
    {
        T result;
        if (hitCollider == null)
            return default(T);

        T searchedComponent = hitCollider.GetComponentInChildren<T>();

        if (searchedComponent == null)
        {
            searchedComponent = hitCollider.GetComponentInParent<T>();
        }

        if (searchedComponent == null)
            return default(T);

        result = searchedComponent;
        return result;
    }
    public static T GetComponent<T>(Collider hitCollider)
    {
        T result;
        if (EqualityComparer<Collider>.Default.Equals(hitCollider, null))
            return default(T);


        T searchedComponent = hitCollider.GetComponentInChildren<T>();

        if (searchedComponent == null)
        {
            searchedComponent = hitCollider.GetComponentInParent<T>();
        }
        
        if (searchedComponent == null)
        {
            searchedComponent = hitCollider.GetComponent<T>();
        }

        if (searchedComponent == null)
            return default(T);

        result = searchedComponent;
        return result;
    }
    
    /*
    public static T[] GetComponentArray<T>(Collider2D[] hitColliders)
    {
        List<T> result = new List<T>();
        
        if (hitColliders.Length <= 0)
        {
            return result.ToArray();
        }
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == null)
                continue;
            
            //To-Do: Angle Calculation
            
            //if (not in the angle, then you just continue and live happily ever after)


            T searchedComponent = hitColliders[i].GetComponentInChildren<T>();

            if (searchedComponent == null)
            {
                searchedComponent = hitColliders[i].GetComponentInParent<T>();
            }


            if (searchedComponent == null)
                continue;

            result.Add(searchedComponent);
        }

        return result.ToArray();
    }
    */
    
    /*
    public static T[] GetHitArray<T>(Transform hitbox, float angle, float range)
    {
        List<T> result = new List<T>();

        Collider2D[] hitColliders;
        hitColliders = Physics2D.OverlapCircleAll(hitbox.position, angle);
        
        if (hitColliders.Length <= 0)
        {
            return result.ToArray();
        }
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == null)
                continue;
            
            //To-Do: Angle Calculation
            
            //if (not in the angle, then you just continue and live happily ever after)
            

            T searchedComponent = hitColliders[i].GetComponentInChildren<T>();

            if (searchedComponent == null)
            {
                searchedComponent = hitColliders[i].GetComponentInParent<T>();
            }


            if (searchedComponent == null)
                continue;

            result.Add(searchedComponent);
        }

        return result.ToArray();
    }
    */

    /*
    public static T[] GetHitArray<T>(Vector3 position, Vector3 scale, int layerNumber)
    {
        List<T> result = new List<T>();

        Collider2D[] hitColliders;
        hitColliders = Physics2D.OverlapBoxAll(position, scale, 0, LayerAndMaskHelper.ConvertNumberIntoLayer(layerNumber));

        if (hitColliders.Length <= 0)
        {
            return result.ToArray();
        }
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == null)
                continue;

            T searchedComponent = hitColliders[i].GetComponentInChildren<T>();

            if (searchedComponent == null)
            {
                searchedComponent = hitColliders[i].GetComponentInParent<T>();
            }


            if (searchedComponent == null)
                continue;

            result.Add(searchedComponent);
        }

        return result.ToArray();
    }

    public static T[] GetHitArray<T>(Transform hitbox, LayerMask layerMask, bool thorough = false)
    {
        List<T> result = new List<T>();

        Collider2D[] hitColliders;
        hitColliders = Physics2D.OverlapBoxAll(hitbox.position, hitbox.localScale / 2, 0, layerMask.value);

        if (hitColliders.Length <= 0)
        {
            return result.ToArray();
        }
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] == null)
                continue;

            T searchedComponent = hitColliders[i].GetComponentInChildren<T>();

            if (searchedComponent == null)
            {
                if (thorough)
                {
                    Transform temp = hitColliders[i].transform;
                    while (searchedComponent == null && temp.parent != null)
                    {
                        searchedComponent = temp.GetComponent<T>();
                        temp = temp.parent;
                    }
                }
                else
                {
                    searchedComponent = hitColliders[i].GetComponentInParent<T>();
                }
            }

            if (searchedComponent == null)
                continue;

            result.Add(searchedComponent);
        }

        return result.ToArray();
    }
*/
}
