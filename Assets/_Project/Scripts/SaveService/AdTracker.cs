using System;
using _Project.Scripts.PurchasesService;
using UniRx;
using UnityEngine.Purchasing;
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

        public void Initialize()
        {
            bool isAdFree = _saveService.Load().IsAdFree;
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

        private void SetAdFree(bool isAdFree)
        {
            IsAdFree.Value = isAdFree;
            SaveData saveData = _saveService.Load();
            saveData.IsAdFree = isAdFree;
            _saveService.Save(saveData);
        }
    }
}