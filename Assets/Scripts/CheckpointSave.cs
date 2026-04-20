using UnityEngine;

public static class CheckpointSave
{
    public static bool hasCheckpoint = false;
    public static string sceneName = "";
    public static string checkpointID = "";
    public static Vector3 respawnPosition = Vector3.zero;

    public static void SaveCheckpoint(string newSceneName, string newCheckpointID, Vector3 newRespawnPosition)
    {
        hasCheckpoint = true;
        sceneName = newSceneName;
        checkpointID = newCheckpointID;
        respawnPosition = newRespawnPosition;
    }

    public static bool HasCheckpointForScene(string currentSceneName)
    {
        return hasCheckpoint && sceneName == currentSceneName;
    }

    public static void ClearCheckpoint()
    {
        hasCheckpoint = false;
        sceneName = "";
        checkpointID = "";
        respawnPosition = Vector3.zero;
    }
}