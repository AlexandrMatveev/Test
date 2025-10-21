using Core.MVC;
using Test.Core.MVC;
using UnityEngine;

namespace Test.Tokens.MVC
{
    public sealed class TokenCounterPlateController : Controller<TokenModel, PlateView>
    {
        public TokenCounterPlateController(TokenModel model, GameObject view) : base(model, view)
        {
            UpdateView();
        }

        public override void Dispose()
        {
            base.Dispose();
            View.UnsubscribeToButtonAction(CarryOut);
        }

        protected override void UpdateView()
        {
            View.SubscribeToButtonAction(CarryOut);
            View.UpdateLabel(Model.GetDescriptor().Name);
            View.UpdateContent(Model.GetAmount().ToString());
        }

        private void CarryOut()
        {
            var charge = Model.GetChargeAmount();
            var amount = Model.AddAmount(charge);
            View.UpdateContent(amount.ToString());
        }
    }
}