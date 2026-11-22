using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Collider))]
public class Gimmick : MonoBehaviour
{
    private Collider _collider;
    private float hpTimer;
    public bool speed;
    [SerializeField, ShowIf("speed"), AllowNesting] public float envSpeedMultiplier = 1;  
    [SerializeField] bool direction;
    [SerializeField, ShowIf("direction"), AllowNesting] float envForceValue;
    [SerializeField] bool hp;
    [SerializeField, ShowIf("hp"), AllowNesting] float envHPTimer;
    [SerializeField, ShowIf("hp"), AllowNesting] float envHPValue;

    private void Awake()
    {
        _collider = this.GetComponent<Collider>();
    }

    private void Update()
    {
        if (hp)
            hpTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterBase character = other.GetComponent<CharacterBase>();

        if (character == null) //not a character
            return;

        if (speed)
            character.ModifySpeed(envSpeedMultiplier);
        if (direction)
            character.ModifyDirection(this.transform.forward * envForceValue);
        if (hp && hpTimer <= 0)
        {
            hpTimer = envHPTimer; //start timer
            character.ModifyHp(envHPValue);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        CharacterBase character = other.GetComponent<CharacterBase>();

        if (character == null) //not a character
            return;

        if (hp && hpTimer <= 0)
        {
            hpTimer = envHPTimer; //start timer
            character.ModifyHp(envHPValue);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        CharacterBase character = other.GetComponent<CharacterBase>();

        if (character == null) //not a character
            return;

        if (speed)
            character.ModifySpeed(1); //reset
        if (direction)
            character.ModifyDirection(Vector3.zero); //reset
    }
}
