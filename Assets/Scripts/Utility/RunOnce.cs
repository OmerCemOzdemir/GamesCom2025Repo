

using System.Collections.Generic;

public class RunOnce
{
    private static RunOnce instance;

    public static RunOnce Instance
    {
        get
        {
            if (instance == null)
            {
                //_instance = FindFirstObjectByType<GameManager>();
                RunOnce runOnce = new RunOnce();
            }
            return instance;
        }
        set
        {
            instance = value;
        }

    }

    public List<bool> runs = new List<bool>();



}
