using System;

namespace Asteroids.Scripts.PurchasesService
{
    public interface IPurchasesService
    {
        public event Action<PurchaseId> OnSuccessfullyPurchased;
        
        public void Purchase(PurchaseId id);
    }
}