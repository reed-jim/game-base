using Unity.Services.Core;
using UnityEngine;

public class GameLeaderboardManager : MonoBehaviour
{
    private async void InitUnityService()
    {
        await UnityServices.InitializeAsync();
    }
}
