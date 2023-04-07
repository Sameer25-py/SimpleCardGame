using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardContainer", menuName = "CardContainer", order = 0)]
public class CardContainer : ScriptableObject
{
    public Sprite                CardSprite;
    public List<CategoryDetails> Categories;

    public List<string> GetCategoryNames()
    {
        List<string> categoryNames = new();
        foreach (CategoryDetails categoryDetails in Categories)
        {
            categoryNames.Add(categoryDetails.Name);
        }

        return categoryNames;
    }
}

[Serializable]
public class CategoryDetails
{
    public string Name;
    public int    Stats;
}