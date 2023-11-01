using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int money;
    public int reputation;

    public int date;
    public int month;

    public int Section;

    public int room_index;

    public List<RoomData> roomDatas;
    public List<FurnitureData> furnituresData;
    public GameState gameState;

    [System.Serializable]
    public class RoomData
    {
        public int x_axis;
        public int y_axis;
        public int width;
        public int height;
        public string roomType;
        public string roomName;
        public bool HasPlayer;
        public string characterName;
    }
    [System.Serializable]
    public class FurnitureData
    {
        public string furnitureName;
        public bool hasCleaned;
    }
    [System.Serializable]
    public class GameState
    {
        public string gameState;

        public GameManager.TypeOfGameStatus GetGameStatus()
        {
            if (string.IsNullOrEmpty(gameState))
            {
                return GameManager.TypeOfGameStatus.Normal;
            }

            if (Enum.TryParse(gameState, out GameManager.TypeOfGameStatus status))
            {
                return status;
            }

            // Handle the case where the string couldn't be parsed as a valid enum value.
            // You can return a default value or handle the error as needed.
            return GameManager.TypeOfGameStatus.Normal; // or another suitable default value.
        }

        public void SetString(GameManager.TypeOfGameStatus input)
        {
            gameState = input.ToString();
        }
    }
    public GameData()
    {
        money = 1000;
        reputation = 100;
        date = 1;
        month = 1;
        
        roomDatas = new List<RoomData>();
        furnituresData = new List<FurnitureData>();
        gameState = new GameState();
        room_index = 0;
    }
}
