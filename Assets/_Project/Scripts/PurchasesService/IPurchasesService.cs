using System;

namespace _Project.Scripts.PurchasesService
{
    public interface IPurchasesService
    {
        public event Action<PurchaseId> OnSuccessfullyPurchased;
        
        public void Purchase(PurchaseId id);
    }
}