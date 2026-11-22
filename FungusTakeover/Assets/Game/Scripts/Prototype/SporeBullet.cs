using MoreMountains.Feedbacks;
using UnityEngine;

public class SporeBullet : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private GameObject spore;
    [HideInInspector] public bool _isFiring = false;
    private float speed;
    private float range;
    private Vector3 beginPosition;

    [SerializeField] private MMFeedbacks SporeFeedback;

    private void Awake()
    {
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
        //spore.SetActive(false);
        _isFiring = false;
    }

    public void OnStartFire(Transform startPos, float _speed, float _range)
    {
        this.transform.position = startPos.position;
        this.transform.rotation = startPos.rotation;
        beginPosition = startPos.position;
        //meshRenderer.enabled = true;
        if(spore != null)
        spore.SetActive(true);
        //SporeFeedback.PlayFeedbacks();
        speed = _speed;
        range = _range;
        _isFiring = true;
        //subtract a spore to not bug out the touch takeover mechanic
        EntityTracker.ChangeSporeCount(-1);
    }

    private void Update()
    {
        if (_isFiring) //move bullet forwards
        {
            this.transform.position += transform.forward * Time.deltaTime * speed;
            if (Vector3.Distance(beginPosition, this.transform.position) >= range)
            {
                //Travelled to the max reach without hitting anything
                meshRenderer.enabled = false;
                if (spore != null)
                    spore.SetActive(false);
                _isFiring = false;
                EntityTracker.ChangeSporeCount(+1); //in that case, give spore back
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterBase collidedEntity = other.GetComponent<CharacterBase>();
        if (collidedEntity != null)
        {
            if (collidedEntity._isCapturable)
            {
                collidedEntity.InitializeTakeover();
                meshRenderer.enabled = false;
                if (spore != null)
                    spore.SetActive(false);
                _isFiring = false;
            }
        }

        SporeCatcher sporeCatcher = other.GetComponent<SporeCatcher>();
        if (sporeCatcher != null)
        {
            sporeCatcher.ToggleState(true);
            meshRenderer.enabled = false;
            if (spore != null)
                spore.SetActive(false);
            _isFiring = false;
        }
    }
}
