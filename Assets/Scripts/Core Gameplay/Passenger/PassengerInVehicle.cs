using PrimeTween;
using UnityEngine;

public class PassengerInVehicle : MonoBehaviour
{
    [SerializeField] private CharacterMaterialPropertyBlock characterMaterialPropertyBlock;

    public void SetColor(Color color)
    {
        Tween.Custom(1.2f, 2.3f, duration: 0.3f, cycles: 2, cycleMode: CycleMode.Yoyo, onValueChange: newVal =>
        {
            characterMaterialPropertyBlock.SetColor(color * newVal);
        });
    }
}
