using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomBuilding : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI costRoom;

    private BuildingSO buildingSO;

    public void SetBuildingSO(BuildingSO buildingSO)
    {
        this.buildingSO = buildingSO;
        backgroundImage.color = Color.white;
        backgroundImage.sprite = buildingSO.roomType[1];
        roomName.text = buildingSO.roomName;
        costRoom.text = buildingSO.costPurchase.ToString();
    }
}
