using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    public int SelectCard(int deckLength)
    {
        return Random.Range(0, deckLength);
    }

    public int SelectCategory(int categoryCount = 4)
    {
        return Random.Range(0, categoryCount);
    }
}