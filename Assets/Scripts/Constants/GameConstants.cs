using UnityEngine;

public static class GameConstants
{
    public static string MENU_SCENE = "Menu";
    public static string GAMEPLAY_SCENE = "Gameplay";

    #region SCREEN ROUTE
    public static string LOBBY_DETAIL_ROUTE = "Lobby Detail";
    #endregion

    #region POOLING
    public static string BOOSTER = "Booster";
    public static string VEHICLE_ENGINE_SOUND = "Vehicle Engine Sound";
    public static string HIT_OBSTACLE_SOUND = "Hit Obstacle Sound";
    public static string GET_IN_VEHICLE_SOUND = "Get In Vehicle Sound";
    public static string VEHICLE_MOVE_OUT_SOUND = "Vehicle Move Out Sound";
    #endregion

    #region COMMON TEXT
    public static string START_GAME = "Start Game";
    public static string CONNECTED = "Connected!";
    public static string DISCONNECTED = "Disconnected!";
    #endregion

    #region COLOR
    public static Color PRIMARY_BACKGROUND = new Color(30f / 255, 60f / 255, 50f / 255, 1);
    public static Color PRIMARY_TEXT = new Color(130f / 255, 255f / 255, 130f / 255, 1);
    public static Color ERROR_BACKGROUND = new Color(90f / 255, 40f / 255, 40f / 255, 1);
    public static Color ERROR_TEXT = new Color(255f / 255, 140f / 255, 140f / 255, 1);


    public static Color SAFERIO_RED = new Color(255f / 255, 90f / 255, 90f / 255, 1);
    public static Color SAFERIO_GREEN = new Color(90f / 255, 255f / 255, 90f / 255, 1);
    public static Color SAFERIO_ORANGE = new Color(255f / 255, 120f / 255, 0f / 255, 1);
    public static Color SAFERIO_PURPLE = new Color(200f / 255, 0f / 255, 255f / 255, 1);
    public static Color SAFERIO_BLUE = new Color(90f / 255, 90f / 255, 255f / 255, 1);
    #endregion

    #region OBJECT POOLING
    public static string TAG_SOUND = "Tag Sound";
    #endregion

    #region ANIMATION
    public static string ANIMATION_STATE = "AnimationState";
    #endregion
}
