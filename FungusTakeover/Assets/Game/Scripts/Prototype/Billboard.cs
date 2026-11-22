using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{ 
    [SerializeField] private Camera cameraToLookAt;

    [Header("Lock Rotation")]
    [SerializeField] private bool lockX;
    [SerializeField] private bool lockY;
    [SerializeField] private bool lockZ;

    private Vector3 originalRotation;

    public Vector3 fixedRotation; // Set your desired rotation value here

    private Quaternion _rotation;

    private void Awake()
    {
        originalRotation = transform.rotation.eulerAngles;
        cameraToLookAt = Camera.main;
    }

    private void Start()
    {
        // Modify the rotation in Euler space to lock certain dimensions.
        /*Vector3 rotation = transform.rotation.eulerAngles;
        if (lockX) { rotation.x = originalRotation.x; }
        if (lockY) { rotation.y = originalRotation.y; }
        if (lockZ) { rotation.z = originalRotation.z; }
        transform.rotation = Quaternion.Euler(rotation);*/

        _rotation = Quaternion.Euler(fixedRotation);
    }

    /*void LateUpdate()
    {
        transform.LookAt(cameraToLookAt.transform);
    }*/

    void LateUpdate()
    {
        transform.rotation = _rotation; // Example: Lock Y rotation                                                          //transform.rotation = Quaternion.identity; // Example: Reset to initial rotation
    }


}
