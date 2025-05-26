using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags : MonoBehaviour
{
    public enum TagType { Player, MainCamera, Respawn, Finish, EditorOnly, GameController, Music, Difficulty }

    public bool Player;
    public bool MainCamera;
    public bool Respawn;
    public bool Finish;
    public bool EditorOnly;
    public bool GameController;
    public bool Music;
    public bool Difficulty;

    public bool CheckTag(TagType type)
    {
        switch (type)
        {
            case TagType.Player:
                return Player;
            case TagType.MainCamera:
                return MainCamera;
            case TagType.Respawn:
                return Respawn;
            case TagType.Finish:
                return Finish;
            case TagType.EditorOnly:
                return EditorOnly;
            case TagType.GameController:
                return GameController;
            case TagType.Music:
                return Music;
            case TagType.Difficulty:
                return Difficulty;
            default:
                return false;
        }
    }
}
