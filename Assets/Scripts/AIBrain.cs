using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBrain : MonoBehaviour
{
    public string BannedCategory;

    public void SetBannedCategory(string categoryName)
    {
        if (BannedCategory == "")
        {
            BannedCategory = categoryName;
        }
    }
    public int SelectCard(int deckLength)
    {
        return Random.Range(0, deckLength);
    }

    public string SelectCategory(List<string> categories)
    {
        string randomCategory = categories[Random.Range(0, categories.Count)];
        if (BannedCategory == "" || BannedCategory != randomCategory)
        {
            return randomCategory;
        }

        return SelectCategory(categories);
    }
}