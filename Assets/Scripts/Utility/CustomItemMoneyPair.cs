using System;

[Serializable]
public class CustomItemMoneyPair
{
    public float value;
    public Operations operations;
    public MoneySpent moneySpent;

    public CustomItemMoneyPair() { }

    public CustomItemMoneyPair(float val, Operations op, MoneySpent ms)
    {
        value = val;    
        operations = op;
        moneySpent = ms;    
    }

}
