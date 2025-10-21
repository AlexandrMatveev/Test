using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Test.Shop.MVC
{
    public class ShopItemView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Button infoButton;
        [SerializeField] private TextMeshProUGUI infoButtonLabel;
        [SerializeField] private Button purchaseButton;
        [SerializeField] private TextMeshProUGUI purchaseButtonLabel;

        public void UpdateLabel(string value)
        {
            if (label != null) label.text = value;
        }

        public void UpdateInfoButtonLabel(string value)
        {
            if (infoButtonLabel != null) infoButtonLabel.text = value;
        }

        public void UpdatePurchaseButtonLabel(string value)
        {
            if (purchaseButtonLabel != null) purchaseButtonLabel.text = value;
        }

        public void SubscribeToPurchaseAction(UnityAction call)
        {
            if (purchaseButton != null) purchaseButton.onClick.AddListener(call);
        }

        public void UnsubscribeToPurchaseAction(UnityAction call)
        {
            if (purchaseButton != null) purchaseButton.onClick.RemoveListener(call);
        }

        public void SubscribeToInfoAction(UnityAction call)
        {
            if (infoButton != null) infoButton.onClick.AddListener(call);
        }

        public void UnsubscribeToInfoAction(UnityAction call)
        {
            if (infoButton != null) infoButton.onClick.RemoveListener(call);
        }

        public void SetPurchaseButtonInteractable(bool flag)
        {
            if (purchaseButton != null) purchaseButton.interactable = flag;
        }

        public void Reset()
        {
            if (infoButton != null) infoButton.onClick.RemoveAllListeners();
            if (purchaseButton != null) purchaseButton.onClick.RemoveAllListeners();
        }
    }
}