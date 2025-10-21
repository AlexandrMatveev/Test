using Core.MVC;
using Test.Core.MVC;
using UnityEngine;

namespace Test.Location.Controllers
{
    public sealed class LocationPlateController : Controller<LocationModel, PlateView>
    {
        public LocationPlateController(LocationModel model, GameObject view) : base(model, view)
        {
            UpdateView();
        }

        protected override void UpdateView()
        {
            View.SubscribeToButtonAction(CarryOut);
            View.UpdateContent(Model.GetDescriptor().Name);
        }

        private void CarryOut()
        {
            var location = Model.UpdateLocation(Model.GetDefaultIdentifier());
            View.UpdateContent(location.Name);
        }

        public override void Dispose()
        {
            base.Dispose();
            View.UnsubscribeToButtonAction(CarryOut);
        }
    }
}