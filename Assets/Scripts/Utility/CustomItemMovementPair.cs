using System;

[Serializable]
public class CustomItemMovementPair 
{
    public float value;
    public Operations operations;
    public PlayerMovement playerMovement;

    public CustomItemMovementPair() { }

    public CustomItemMovementPair(float val, Operations op, PlayerMovement playerMov)
    {
        value = val;
        operations = op;
        playerMovement = playerMov;
    }

}
