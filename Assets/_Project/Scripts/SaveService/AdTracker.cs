using System;
using _Project.Scripts.PurchasesService;
using UniRx;
using Zenject;

namespace Asteroids.Scripts.SaveService
{
    public class AdTracker : IInitializable, IDisposable
    {
        private readonly ISaveService _saveService;
        private readonly IPurchasesService _purchaseService;

        public ReactiveProperty<bool> IsAdFree { get; } = new ReactiveProperty<bool>(false);

        public AdTracker(ISaveService saveService, IPurchasesService purchaseService)
        {
            _saveService = saveService;
            _purchaseService = purchaseService;
        }

        public async void Initialize()
        {
            SaveData data = await _saveService.Load();
            bool isAdFree = data.IsAdFree;
            IsAdFree.Value = isAdFree;
            
            _purchaseService.OnSuccessfullyPurchased += PurchaseServiceOnSuccessfullyPurchased;
        }
        private void PurchaseServiceOnSuccessfullyPurchased(PurchaseId purchaseId)
        {
            SetAdFree(true);
        }
        
        public void Dispose()
        {
            _purchaseService.OnSuccessfullyPurchased -= PurchaseServiceOnSuccessfullyPurchased;
        }

        private async void SetAdFree(bool isAdFree)
        {
            IsAdFree.Value = isAdFree;
            SaveData saveData = await _saveService.Load();
            saveData.IsAdFree = isAdFree;
            _saveService.Save(saveData);
        }
    }
}