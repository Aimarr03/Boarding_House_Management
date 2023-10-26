using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoodIndicator : MonoBehaviour
{
    [SerializeField] private Character character;
    [SerializeField] private List<Transform> MoodIndicators;

    [SerializeField] private List<Character.MoodIndicator> mMoodIndicators;
    private Dictionary<Character.MoodIndicator,Transform> moodIndicator;
    private void Awake()
    {
        moodIndicator = new Dictionary<Character.MoodIndicator, Transform>();
        int index = 0;
        foreach(Transform child in MoodIndicators)
        {
            moodIndicator.Add(mMoodIndicators[index], child);
            child.gameObject.SetActive(false);
            index++;
        }
    }

    public void SetMoodIndicator(Character.MoodIndicator indicator)
    {
        foreach (Transform child in moodIndicator.Values)
        {
            child.gameObject.SetActive(false);
        }
        moodIndicator[indicator].gameObject.SetActive(true);
    }
}
