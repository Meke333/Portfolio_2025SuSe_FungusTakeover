using System.Threading.Tasks;
using UnityEngine;
using EditorAttributes;
using Zenject;
using UnityEngine.AI;

public class TriggerboxBehaviour : MonoBehaviour 
{
    [Inject] private SceneEvents SceneEvents;

    public bool isOpen;

    public Transform _teleportPoint;


    [FoldoutGroup("Don't Touch",
        nameof(openMaterial),
        nameof(closeMaterial),
        nameof(meshRenderer),
        nameof(alreadyUsed))]
    public EditorAttributes.Void dontTouchGroup;

    [HideProperty] public Material openMaterial;
    [HideProperty] public Material closeMaterial;
    [HideProperty] public MeshRenderer meshRenderer;
    [HideProperty] public bool alreadyUsed = false;

    private void OnEnable()
    {
        EntityTracker.OnAllEnemiesDeadRoom += TurnOnOpen;
        SceneEvents.OnPlayerTeleportedToNewRoom += CloseRoom;
    }

    private void OnDisable()
    {
        EntityTracker.OnAllEnemiesDeadRoom -= TurnOnOpen;
        SceneEvents.OnPlayerTeleportedToNewRoom -= CloseRoom;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen)
        {
            return;
        }

        if (!other.GetComponentInChildren<CharacterBase>()._isPlayer)
            return;
        
        TeleportToPlace();
    }

    public void TurnOnOpen()
    {
        isOpen = true;
        meshRenderer.material = openMaterial;
    }

    private async void TeleportToPlace()
    {
        alreadyUsed = true;

        for (int i = 0; i < EntityTracker.entities_allies.Count; i++)
        {
            
            
            NavMeshAgent agent = EntityTracker.entities_allies[i].GetComponentInChildren<NavMeshAgent>();

            if (agent == null)
                continue;

            agent.Warp(_teleportPoint.position);
            //EntityTracker.entities_allies[i].transform.position = _teleportPoint.position;

            await Task.Yield();
        }

        SceneEvents.OnPlayerTeleportedToNewRoom?.Invoke();
        EntityTracker.IncreaseRoomCount();
    }
    private void CloseRoom()
    {
        meshRenderer.material = closeMaterial;
        isOpen = false;
    }
}

