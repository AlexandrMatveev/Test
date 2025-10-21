using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Test.Core.MVC
{
    /// <summary>
    /// Some generic component for in game gui plate
    /// </summary>
    public class PlateView : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private TextMeshProUGUI content;
        [SerializeField] private Button button;

        public void UpdateLabel(string value)
        {
            label.text = value;
        }

        public void UpdateContent(string value)
        {
            content.text = value;
        }

        public void SubscribeToButtonAction(UnityAction call)
        {
            button.onClick.AddListener(call);
        }

        public void UnsubscribeToButtonAction(UnityAction call)
        {
            button.onClick.RemoveListener(call);
        }

        public void Reset()
        {
            button.onClick.RemoveAllListeners();
        }
    }
}