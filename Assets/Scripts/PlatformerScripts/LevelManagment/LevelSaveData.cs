using System;
using TMPro;

[Serializable]
public class LevelSaveData 
{
    public string levelName;
    public int levelIndex;
    public bool unlock = false;
    public float[] checkpointX;
    public float[] checkpointY;
    public float[] checkpointZ;
    public bool[] unlockCheckpoint;

}
