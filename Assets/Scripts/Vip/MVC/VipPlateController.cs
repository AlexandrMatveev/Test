using System;
using System.Threading;
using System.Threading.Tasks;
using Core.MVC;
using Test.Core.MVC;
using UnityEngine;

namespace Test.Vip.Controllers
{
    public sealed class VipPlateController : Controller<VipModel, PlateView>
    {
        private CancellationTokenSource cancellationSource;

        public VipPlateController(VipModel model, GameObject view) : base(model, view)
        {
            UpdateView();
        }

        public override void Dispose()
        {
            base.Dispose();
            ResetView();
            ResetPeriodicCharge(false);
        }

        protected override void UpdateView()
        {
            ResetView();
            UpdateViewData();
            ResetPeriodicCharge(true);
            _ = PeriodicCharge(cancellationSource.Token);
        }

        private void ResetView()
        {
            View.Reset();
        }

        private void UpdateViewData()
        {
            View.SubscribeToButtonAction(CarryOut);
            View.UpdateLabel(Model.GetDescriptor().Name);
            View.UpdateContent(Model.GetCurrentDuration().ToString(@"mm\:ss"));
        }

        private void ResetPeriodicCharge(bool renew)
        {
            cancellationSource?.Cancel(false);
            cancellationSource?.Dispose();
            cancellationSource = renew ? new CancellationTokenSource() : null;
        }

        private async Task PeriodicCharge(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var period = TimeSpan.FromSeconds(1);
                await Task.WhenAny(Task.Delay(period, cancellationToken));
                var duration = Model.AddAmount(-period).TotalMilliseconds;
                if (duration <= 0) return;

                View.UpdateContent(Model.GetCurrentDuration().ToString(@"mm\:ss"));
            }
        }

        private void CarryOut()
        {
            Model.AddAmount(Model.GetDefaultChargeDuration());
        }
    }
}