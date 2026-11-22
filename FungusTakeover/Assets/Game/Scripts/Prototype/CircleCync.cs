using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircleCync : MonoBehaviour
{
    [SerializeField] private Material WallMaterial;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;
    public static int PosID = Shader.PropertyToID("_position");
    public static int SizeID = Shader.PropertyToID("_size");

    // Update is called once per frame
    void Update()
    {

        if (_camera != null)
        {
            var direction = _camera.transform.position - transform.position;
            var ray = new Ray(transform.position, direction.normalized);

            if (Physics.Raycast(ray, 3000, _layerMask))
                WallMaterial.SetFloat(SizeID, 1);
            else
                WallMaterial.SetFloat(SizeID, 0);

            var view = _camera.WorldToViewportPoint(transform.position);
            WallMaterial.SetVector(PosID, view);
        }
    }
}
