using Core.MVC;
using Test.Shop.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Test.Shop.MVC
{
    public sealed class ShopController : Controller<ShopModel, ScrollRect>
    {
        private IExposedPropertyTable resolver;
        private ShopItemView[] views;

        public ShopController(ShopModel model, GameObject view, IExposedPropertyTable resolver) : base(model, view)
        {
            this.resolver = resolver;
            UpdateView();
        }

        public void ForceUpdateView()
        {
            UpdateView();
        }

        protected override void UpdateView()
        {
            SpawnItems();
            ResetItems();
            ConfigureItems();
        }

        private void SpawnItems()
        {
            if (views?.Length > 0) return;

            var shopData = Model.GetShopData();
            var items = shopData.Items;
            views = new ShopItemView[items.Count];
            for (var i = 0; i < items.Count; i++)
            {
                var prefab = shopData.ShopItemBinding.Resolve<GameObject>(resolver);
                views[i] = Object.Instantiate(prefab, View.content).GetComponent<ShopItemView>();
            }
        }

        private void ResetItems()
        {
            if (views == null || views.Length == 0) return;

            foreach (var view in views) view.Reset();
        }

        private void ConfigureItems()
        {
            if (views == null || views.Length == 0) return;

            var items = Model.GetShopData().Items;
            for (var i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var view = views[i];

                view.UpdateLabel(item.Data.Descriptor.Description);
                view.UpdatePurchaseButtonLabel("Buy");
                view.SubscribeToInfoAction(() => GoToCard(item));
                view.SubscribeToPurchaseAction(() => CarryOut(view, item));
                view.SetPurchaseButtonInteractable(Model.CheckPurchasePossibility(item));
            }
        }

        private async void CarryOut(ShopItemView view, ShopItemData data)
        {
            view.UpdatePurchaseButtonLabel("Processing");
            view.SetPurchaseButtonInteractable(false);

            await Model.MakePurchase(data);
        }

        private void GoToCard(ShopItemData item)
        {
            Model.SetSelected(item);
            SceneManager.LoadScene(Model.GetShopData().CardScene.Path, LoadSceneMode.Single);
        }

        public override void Dispose()
        {
            base.Dispose();
            ResetItems();
            foreach (var view in views)
                if (view)
                    Object.Destroy(view.gameObject);
        }
    }
}