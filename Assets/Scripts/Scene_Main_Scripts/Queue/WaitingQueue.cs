using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingQueue
{
    private GameObject parent;
    public List<Vector2> _WaitingQueu;
    public List<Character> CharacterWaiting;
    public Vector2 entrancePoint;

    public List<Vector2> _childWaitingQueue;
    public Vector2 childEntrancePoint;
    public WaitingQueue(List<Vector2> input)
    {
        _WaitingQueu = input;
        entrancePoint = _WaitingQueu[_WaitingQueu.Count - 1] + new Vector2(1,0);
        _childWaitingQueue = new List<Vector2>();
        CharacterWaiting = new List<Character>();
    }
    public void SetParent(GameObject parent)
    {
        this.parent = parent;
        _childWaitingQueue.Clear();
        for(int x = 0; x< 5; x++)
        {
            _childWaitingQueue.Add(new Vector2(1, 0) * x);
        }
        childEntrancePoint = _childWaitingQueue[_childWaitingQueue.Count - 1] + new Vector2(1,0);
    }
    public void AddCharacter(Character character)
    {
        
        CharacterWaiting.Add(character);
        Debug.Log(_WaitingQueu[CharacterWaiting.IndexOf(character)]);
        Vector2 position = _WaitingQueu[CharacterWaiting.IndexOf(character)];
        character.SetTargetPosition(position);
        character.GetCharacterInterraction().UpdateOriginalPosition(position);
    }
    public void RemoveCharacter(Character character)
    {
        int index = CharacterWaiting.IndexOf(character);
        if (index < 0) return;
        Debug.Log("index character : "+ index);
        CharacterWaiting.RemoveAt(index);
        RelocateCharacter();
    }
    public void RelocateCharacter()
    {
        for(int x = 0; x< CharacterWaiting.Count; x++)
        {
            Character character = CharacterWaiting[x];
            character.SetTargetPosition(_WaitingQueu[x]);
            character.GetCharacterInterraction().UpdateOriginalPosition(_WaitingQueu[x]);
        }
    }

}
