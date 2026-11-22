using UnityEngine;
using System.Linq;

public class SporeCatcher : MonoBehaviour
{
    private CapsuleCollider _collider;
    private bool isInfected;
    [SerializeField] Material materialOpen;
    [SerializeField] Material materialInfected;

    private Vector3 p1;
    private Vector3 p2;
    private float radius;

    readonly Collider[] hitCollider = new Collider[8]; //max of 8 colliders detectable per frame

    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();

        /* shit dont work
        //grabbed the code from here: https://dev.to/fujieda/unity-how-to-use-physics-overlapcapsule-5fng
        var direction = new Vector3 { [_collider.direction] = 1 };
        var offset = _collider.height / 2 - _collider.radius;
        var localPoint0 = _collider.center - direction * offset;
        var localPoint1 = _collider.center + direction * offset;
        p1 = transform.TransformPoint(localPoint0);
        p2 = transform.TransformPoint(localPoint1);
        */
        /* shit dont work rip
        var r = transform.TransformVector(_collider.radius, _collider.radius, _collider.radius);
        radius = Enumerable.Range(0, 3).Select(xyz => xyz == _collider.direction ? 0 : r[xyz])
            .Select(Mathf.Abs).Max();
        */
        //just assumes collider is along the y axis and scales it along the x axis
        radius = _collider.radius * transform.lossyScale.x;

        ToggleState(false);
    }

    private void Update()
    {
        if (!isInfected)
            return;

        int nb = Physics.OverlapSphereNonAlloc(this.transform.position, radius, hitCollider);
        for (int x = 0; x < nb; x++)
        {
            CharacterBase hitChar = hitCollider[x].GetComponent<CharacterBase>();
            if (hitChar == null) //not a char
                continue;
            if (!hitChar._isCapturable) //not ded
                continue;
            hitChar.InitializeTakeover();
        }
    }

    public void ToggleState(bool infectState)
    {
        isInfected = infectState;
        if (isInfected)
            this.GetComponentInChildren<MeshRenderer>().material = materialInfected;
        else
            this.GetComponentInChildren<MeshRenderer>().material = materialOpen;
    }
}
