using UnityEngine;
using NaughtyAttributes;

//it's basically a gimmick..... just copy pasted so the entity can find it's ref + pick targets based on settings NOTE: notify mike of this, also 
//make sure that in the final these don't overwrite each other randomly when placing a mycel on top of an already existing gimmick
public class GimmickMycel : MonoBehaviour
{
    private Collider _collider;
    private MeshRenderer _meshRenderer;
    private float hpTimer;
    public bool speed;
    [SerializeField, ShowIf("speed"), AllowNesting] public float envSpeedMultiplierENEMY = 1;
    [SerializeField, ShowIf("speed"), AllowNesting] public float envSpeedMultiplierALLY = 1;
    [SerializeField, ShowIf("speed"), AllowNesting] public float envSpeedMultiplierPLAYER = 1;
    [SerializeField] bool direction;
    [SerializeField, ShowIf("direction"), AllowNesting] float envForceValueENEMY;
    [SerializeField, ShowIf("direction"), AllowNesting] float envForceValueALLY;
    [SerializeField, ShowIf("direction"), AllowNesting] float envForceValuePLAYER;
    [SerializeField] bool hp;
    [SerializeField, ShowIf("hp"), AllowNesting] float envHPTimer;
    [SerializeField, ShowIf("hp"), AllowNesting] float envHPValueENEMY;
    [SerializeField, ShowIf("hp"), AllowNesting] float envHPValueALLY;
    [SerializeField, ShowIf("hp"), AllowNesting] float envHPValuePLAYER;

    private void Awake()
    {
        _collider = this.GetComponent<Collider>();
        _meshRenderer = this.GetComponentInChildren<MeshRenderer>();
        DisableMycel();
    }

    public void DisableMycel()
    {
        _collider.enabled = false;
        _meshRenderer.enabled = false;
    }
    public void EnableMycel()
    {
        _collider.enabled = true;
        _meshRenderer.enabled = true;
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
        {
            if (!character._isCaptured) //Enemy
                character.ModifySpeed(envSpeedMultiplierENEMY);
            else if (!character._isPlayer) //Ally
                character.ModifySpeed(envSpeedMultiplierALLY);
            else //Player or AllyPlayer
                character.ModifySpeed(envSpeedMultiplierPLAYER);
        }
            
        if (direction)
        {
            if (!character._isCaptured)
                character.ModifyDirection(this.transform.forward * envForceValueENEMY);
            else if (!character._isPlayer)
                character.ModifyDirection(this.transform.forward * envForceValueALLY);
            else
                character.ModifyDirection(this.transform.forward * envForceValuePLAYER);
        }
        if (hp && hpTimer <= 0)
        {
            hpTimer = envHPTimer; //start timer
            if (!character._isCaptured)
                character.ModifyHp(envHPValueENEMY);
            else if (!character._isPlayer)
                character.ModifyHp(envHPValueALLY);
            else
                character.ModifyHp(envHPValuePLAYER);
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
            if (!character._isCaptured)
                character.ModifyHp(envHPValueENEMY);
            else if (!character._isPlayer)
                character.ModifyHp(envHPValueALLY);
            else
                character.ModifyHp(envHPValuePLAYER);
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
