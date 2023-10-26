using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager instance;
    [SerializeField] private List<CharacterSO> ListCharacter;
    [SerializeField] private GameObject QueueEntry;
    [SerializeField] private Transform InstantiatePoint;
    public Vector2 firstPosition;
    public int size;
    private WaitingQueue _waitingQueue;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        firstPosition = QueueEntry.transform.position;
        List<Vector2> listQueue = new List<Vector2>();
        for(int i = 0; i < 5; i++)
        {
            listQueue.Add(firstPosition + new Vector2(1,0)*i*size);
            
        }
        _waitingQueue = new WaitingQueue(listQueue);
        _waitingQueue.SetParent(QueueEntry);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddNewLine()
    {
        if (_waitingQueue.CharacterWaiting.Count > _waitingQueue._childWaitingQueue.Count-1) return;
        int randomIndex = Random.Range(0, ListCharacter.Count);
        GameObject newCharacter = Instantiate(ListCharacter[randomIndex].prefabCharacter);
        newCharacter.gameObject.transform.position = InstantiatePoint.position;
        //newCharacter.gameObject.transform.SetParent(QueueEntry.transform);
        Character currentCharacter = newCharacter.GetComponent<Character>();
        _waitingQueue.AddCharacter(currentCharacter);
    }
    public void RemoveFromLine(Character character)
    {
        _waitingQueue.RemoveCharacter(character);
    }
}
