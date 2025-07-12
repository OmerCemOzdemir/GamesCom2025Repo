using System;

[Serializable]
public class LevelSaveData 
{
    public string levelName;
    public int levelIndex;
    public bool unlock;
    public float[] checkpointX;
    public float[] checkpointY;
    public float[] checkpointZ;
    public bool[] unlockCheckpoint;

}
