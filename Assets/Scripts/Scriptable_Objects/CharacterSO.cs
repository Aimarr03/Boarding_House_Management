using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character SO", menuName = "Create new Character SO")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public GameObject prefabCharacter;
    public List<ExpressionData> expressionData;

    [System.Serializable]
    public class ExpressionData
    {
        public ExpressionType expressionType; // Change "expression" to "expressionType"
        public Sprite sprite;
    }

    public enum ExpressionType // Move the enum inside the CharacterSO class
    {
        Normal,
        Happy,
        Sad,
        Angry
    }
    public Sprite GetSpriteExpression(ExpressionType expressionType)
    {
        foreach(ExpressionData expressionData in expressionData)
        {
            if(expressionData.expressionType == expressionType)
            {
                return expressionData.sprite;
            }
        }
        return null;
    }
}

