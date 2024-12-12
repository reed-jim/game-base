using UnityEngine;
using static GameEnum;

public class PassengerFaction : MonoBehaviour
{
    [SerializeField] private CharacterMaterialPropertyBlock characterMaterialPropertyBlock;

    [SerializeField] private GameFaction _faction;

    public GameFaction Faction
    {
        get => _faction;
    }

    private void Awake()
    {
        PassengerQueue.setPassengerFactionEvent += SetFaction;
    }

    private void OnDestroy()
    {
        PassengerQueue.setPassengerFactionEvent -= SetFaction;
    }

    private void SetFaction(int instanceId, GameFaction faction)
    {
        if (instanceId != gameObject.GetInstanceID())
        {
            return;
        }

        if (faction == GameFaction.Red)
        {
            characterMaterialPropertyBlock.SetColor(Color.red);
        }
        else if (faction == GameFaction.Green)
        {
            characterMaterialPropertyBlock.SetColor(Color.green);
        }
        else if (faction == GameFaction.Orange)
        {
            characterMaterialPropertyBlock.SetColor(new Color(255f / 255, 120f / 255, 0f / 255, 1));
        }
        else if (faction == GameFaction.Purple)
        {
            characterMaterialPropertyBlock.SetColor(new Color(200f / 255, 0f / 255, 255f / 255, 1));
        }
        else if (faction == GameFaction.Blue)
        {
            characterMaterialPropertyBlock.SetColor(Color.blue);
        }

        _faction = faction;
    }
}
