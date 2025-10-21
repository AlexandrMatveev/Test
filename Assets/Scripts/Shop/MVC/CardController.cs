using Core.MVC;
using Test.Shop.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Test.Shop.MVC
{
    public sealed class CardController : Controller<ShopModel, ShopItemView>
    {
        public CardController(ShopModel model, GameObject view) : base(model, view)
        {
            UpdateView();
        }

        public void ForceUpdateView()
        {
            UpdateView();
        }

        protected override void UpdateView()
        {
            var item = Model.GetSelected();
            View.UpdateLabel(item.Data.Descriptor.Description);
            View.UpdatePurchaseButtonLabel("Buy");
            View.SubscribeToInfoAction(GoToShop);
            View.SubscribeToPurchaseAction(() => CarryOut(View, item));
            View.SetPurchaseButtonInteractable(Model.CheckPurchasePossibility(item));
        }

        private async void CarryOut(ShopItemView view, ShopItemData data)
        {
            view.UpdatePurchaseButtonLabel("Processing");
            view.SetPurchaseButtonInteractable(false);

            await Model.MakePurchase(data);
        }

        private void GoToShop()
        {
            SceneManager.LoadScene(Model.GetShopData().ShopScene.Path, LoadSceneMode.Single);
        }
    }
}