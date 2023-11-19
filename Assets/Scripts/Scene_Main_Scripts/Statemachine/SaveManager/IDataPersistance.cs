using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance 
{
    public void LoadScene(GameData gameData);
    public void SaveScene(ref GameData gameData);
}
