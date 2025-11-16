using UnityEngine;

public static class DifficultySetting
{
    private static int difficulty;
    public static void EasySetting()   { difficulty = 1; }
    public static void MediumSetting() { difficulty = 2; }
    public static void HardSetting()   { difficulty = 3; }

    private static GameObject player;

    public static void SetTheDifficulty()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        switch (difficulty){
            case 3:
                if (player == null)
                    return;
                player.GetComponent<PlayerHealth>().DifficultySet(3);
                Debug.Log("Difficulty Hard Set");
                //hard Mode
                break;

            case 2:
                if (player == null)
                    return;
                player.GetComponent<PlayerHealth>().DifficultySet(12);
                Debug.Log("Difficulty Normal Set");
                //Medium Mode
                break;

            case 1:
                if (player == null)
                    return;
                player.GetComponent<PlayerHealth>().DifficultySet(24);
                Debug.Log("Difficulty Easy Set");
                // Easy Mode
                break;

            default:
                Debug.Log("Invalid selection");

                break;
        }
    }
}
