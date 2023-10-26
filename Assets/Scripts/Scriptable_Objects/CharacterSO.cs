using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Character SO", menuName = "Create new Character SO")]
public class CharacterSO : ScriptableObject
{
    public string characterName;
    public GameObject prefabCharacter;
}
