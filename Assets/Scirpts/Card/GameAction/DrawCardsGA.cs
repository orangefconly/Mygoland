using UnityEngine;

public class DrawCardsGA : GameAction
{
    public int Amout {  get; private set; }

    public DrawCardsGA(int amout)
    {
        Amout = amout;
    }
}
