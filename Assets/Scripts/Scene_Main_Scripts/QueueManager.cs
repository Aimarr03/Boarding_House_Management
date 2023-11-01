using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager instance;
    [SerializeField] private List<CharacterSO> ListCharacter;
    [SerializeField] private GameObject QueueEntry;
    [SerializeField] private List<Character> WaitingCharacter;
    [SerializeField] private List<Character> AngryCustomer;
    public Transform InstantiatePoint;
    
    public int defaultDuration;
    
    private int normalDuration;
    private int slowDuration;
    private int fastDuration;

    private int LongWaitingDay;
    private int MaxWaitingDay;

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
        normalDuration = 2;
        slowDuration = 3;
        fastDuration = 1;
        defaultDuration = normalDuration;

        LongWaitingDay = 3;
        MaxWaitingDay = 5;

        WaitingCharacter = _waitingQueue.CharacterWaiting;
        AngryCustomer = _waitingQueue.AngryCustomer;
    }
    private void Start()
    {
        ReputationManager.instance.IndicatorChange += Instance_IndicatorChange;
    }

    private void Instance_IndicatorChange(ReputationManager.StatusReputation currentStatus)
    {
        switch (currentStatus)
        {
            case ReputationManager.StatusReputation.low:
                defaultDuration = slowDuration;
                break;
            case ReputationManager.StatusReputation.normal:
                defaultDuration = normalDuration;
                break;
            case ReputationManager.StatusReputation.high:
                defaultDuration = fastDuration;
                break;
        }
    }

    public void OnDestroy()
    {
        ReputationManager.instance.IndicatorChange -= Instance_IndicatorChange;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddNewLine()
    {
        Debug.Log("Empty Room: "+ !EconomyManager.instance.CheckRoomEmpty());
        if (!EconomyManager.instance.CheckRoomEmpty()) return;
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
    public void CheckWaitingDay()
    {
        if (_waitingQueue.CharacterWaiting.Count <= 0) return;
        for(int x = 0; x < ListCharacter.Count-1; x++)
        {
            Debug.Log(x);
            Character character = _waitingQueue.CharacterWaiting[x];
            if (character.HasArrived())
            {
                character.waitingDay++;
                if (character.waitingDay >= LongWaitingDay)
                {
                    character.ChangeMood(Character.MoodIndicator.Angry);
                }
                if (character.waitingDay >= MaxWaitingDay)
                {
                    _waitingQueue.RemoveCharacter(character);
                    _waitingQueue.AngryCustomer.Add(character);
                    character.transform.localScale = new Vector3(-1, 1, 1);
                    character.SetTargetPosition(InstantiatePoint.position);
                    character.GetCharacterInterraction().UpdateOriginalPosition(InstantiatePoint.position);
                    character.GetCharacterInterraction().ToggleOccupied();
                }
            }
        }
    }
    public void CheckAngryCustomer()
    {
        if (AngryCustomer.Count <= 0) return;
        for (int x = 0; x < AngryCustomer.Count; x++)
        {
            Character character = AngryCustomer[x];
            if(character.HasArrived())
            {
                _waitingQueue.AngryCustomer.RemoveAt(x);
                character.ChangeMood(Character.MoodIndicator.Dissapointed);
            }
        }
    }
    
}
