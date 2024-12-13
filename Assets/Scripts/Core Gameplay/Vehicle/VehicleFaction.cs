using System;
using UnityEngine;
using static GameEnum;

public class VehicleFaction : MonoBehaviour
{
    [SerializeField] private CharacterMaterialPropertyBlock characterMaterialPropertyBlock;

    [SerializeField] private GameFaction _faction;

    public GameFaction Faction
    {
        get => _faction;
    }

    public static event Action<GameFaction> vehicleFactionSetEvent;

    private void Start()
    {
        SetFaction(_faction);
    }

    public void SetRandomFaction()
    {
        SetFaction((GameFaction)UnityEngine.Random.Range(0, 4));
    }

    private void SetFaction(GameFaction faction)
    {
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

        vehicleFactionSetEvent?.Invoke(faction);
    }
}
