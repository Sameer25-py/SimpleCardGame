using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CardScriptableObjectCreator : MonoBehaviour
{
    public List<Sprite> Sprites;
    public int          CardCount = 32;

    private void Start()
    {
        AssignSprites();
    }

    private void CardCreator()
    {
        for (int i = 0; i < CardCount; i++)
        {
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<CardContainer>(),
                $"Assets/ScriptableObjects/Card{i + 1}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    private void AssignSprites()
    {
        for (int i = 0; i < CardCount; i++)
        {
            CardContainer cardContainer =
                AssetDatabase.LoadAssetAtPath<CardContainer>($"Assets/ScriptableObjects/Card{i + 1}.asset");
            cardContainer.CardSprite = Sprites[i];
            EditorUtility.SetDirty(cardContainer);
        }

        AssetDatabase.SaveAssets();
    }
}