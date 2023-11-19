using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class FurnitureBuildingUI : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private TextMeshProUGUI costRoom;
    [SerializeField] private TextMeshProUGUI SoldOut;

    private Image image;
    private Button button;
    private FurnitureSO furnitureSO;
    private bool bought;
    [SerializeField] Color disabledColor;

    private void Awake()
    {
        bought = false;
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        button.onClick.AddListener(() => OnClicked());
    }

    public void SetFurnitureSO(FurnitureSO furnitureSO)
    {
        this.furnitureSO = furnitureSO;
        backgroundImage.color = Color.white;
        backgroundImage.sprite = furnitureSO.image;
        roomName.text = furnitureSO.name;
        costRoom.text = furnitureSO.cost.ToString();
    }
    public void OnClicked()
    {
        if (!bought)
        {
            SetBought(BuildingState.instance.BuyFurnitue(furnitureSO));
        }
    }
    public void SetBought(bool input)
    {
        bought = input;
        SoldOut.gameObject.SetActive(bought);
    }
    public FurnitureSO GetFurnitureSO()
    {
        return furnitureSO;
    }
}
