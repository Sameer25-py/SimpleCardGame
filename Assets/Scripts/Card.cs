using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public Image          CardImage;
    public List<Category> Categories = new();
    public Category GetCategory(string name)
    {
        foreach (Category category in Categories)
        {
            if (category.Name == name)
            {
                return category;
            }
        }

        return null;
    }
}
