using UnityEngine;

public class GameEnum : MonoBehaviour
{
    public enum ScreenRoute {
        Waiting,
        Lobby,
        LobbyRoom
    }

    public enum GameFaction {
        Red,
        Blue,
        Green,
        Purple,
        Orange
    }

    public enum CharacterAnimationState {
        Idle = 0,
        Walking = 1
    }
}
