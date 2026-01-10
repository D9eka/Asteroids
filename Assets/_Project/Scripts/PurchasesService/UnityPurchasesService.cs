using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;


namespace _Project.Scripts.PurchasesService
{
    public class UnityPurchasesService : IPurchasesService, IInitializable, IDisposable
    {
        private bool m_IsPurchaseInProgress;

        private StoreController m_StoreController;

        public event Action<PurchaseId> OnSuccessfullyPurchased;
        
        private List<PurchaseId> _confirmedPurchaseIds = new List<PurchaseId>();
        
        
        public void Initialize()
        {
            InitializeIAPAsync();
        }
        public void Dispose()
        {
            UnsubscribeIAPEvents();
        }
        
        public void Purchase(PurchaseId id)
        {
            if (m_IsPurchaseInProgress)
            {
                Debug.LogWarning("[IAP] Purchase already in progress.");
                return;
            }
            m_IsPurchaseInProgress = true;
            string purchaseId = id.ToString();
            m_StoreController.PurchaseProduct(purchaseId);
        }
        
        private async void InitializeIAPAsync()
        {
            m_StoreController = UnityIAPServices.StoreController();
            SubscribeIAPEvents();

            try
            {
                await m_StoreController.Connect();
                Debug.Log("[IAP] Connected to store.");

                List<ProductDefinition> productDefs = BuildProductsWithCatalog();
                if (productDefs.Count == 0)
                {
                    return;
                }

                m_StoreController.FetchProducts(productDefs);
            }
            catch (Exception e)
            {
                Debug.LogError($"[IAP] Connect failed: {e.Message}");
            }
        }
        
        private void SubscribeIAPEvents()
        {
            if (m_StoreController == null) return;

            m_StoreController.OnProductsFetched += OnProductsFetched;
            m_StoreController.OnProductsFetchFailed += OnProductsFetchFailed;

            m_StoreController.OnPurchasesFetched += OnPurchasesFetched;
            m_StoreController.OnPurchasesFetchFailed += OnPurchasesFetchFailed;

            m_StoreController.OnPurchasePending += OnPurchasePending;       
            m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            m_StoreController.OnPurchaseFailed += OnPurchaseFailed;         
            
            m_StoreController.OnStoreDisconnected += OnStoreDisconnected;
        }
        
        private List<ProductDefinition> BuildProductsWithCatalog()
        {
            List<ProductDefinition> productDefinitions = new List<ProductDefinition>();
            ProductCatalog catalog = ProductCatalog.LoadDefaultCatalog();
            if (catalog == null || catalog.allProducts == null || catalog.allProducts.Count == 0)
            {
                Debug.LogWarning("[IAP] No products in IAPProductCatalog.json.");
                return productDefinitions;
            }

            foreach (ProductCatalogItem item in catalog.allProducts)
            {
                productDefinitions.Add(
                    new ProductDefinition(
                        item.id,
                        item.type
                    )
                );
            }

            return productDefinitions;
        }

        private void UnsubscribeIAPEvents()
        {
            if (m_StoreController == null) return;

            m_StoreController.OnProductsFetched -= OnProductsFetched;
            m_StoreController.OnProductsFetchFailed -= OnProductsFetchFailed;

            m_StoreController.OnPurchasesFetched -= OnPurchasesFetched;
            m_StoreController.OnPurchasesFetchFailed -= OnPurchasesFetchFailed;

            m_StoreController.OnPurchasePending -= OnPurchasePending;
            m_StoreController.OnPurchaseConfirmed -= OnPurchaseConfirmed;
            m_StoreController.OnPurchaseFailed -= OnPurchaseFailed;

            m_StoreController.OnStoreDisconnected -= OnStoreDisconnected;
        }
        
        private void OnProductsFetched(List<Product> products)
        {
            m_StoreController.FetchPurchases();

            LogProductsFetched(products);
        }

        private void LogProductsFetched(List<Product> products)
        {
            Debug.Log($"[IAP] Products fetched: {products.Count}");
            foreach (Product p in products)
            {
                Debug.Log($"[IAP] {p.definition.id} | {p.metadata.localizedTitle} | {p.metadata.localizedPriceString}");
            }
        }

        private void OnProductsFetchFailed(ProductFetchFailed failure)
        {
            Debug.LogError($"[IAP] Product fetch failed: {failure.FailureReason}");
        }

        private void OnPurchasesFetched(Orders orders)
        {
            foreach (ConfirmedOrder confirmedOrder in orders.ConfirmedOrders)
            {
                foreach (CartItem cartItem in confirmedOrder.CartOrdered.Items())
                {
                    if (Enum.TryParse(cartItem.Product.definition.id, out PurchaseId id))
                    {
                        _confirmedPurchaseIds.Add(id);
                    }
                }
            }  
        }

        private void OnPurchasesFetchFailed(PurchasesFetchFailureDescription failure)
        {
            Debug.LogError($"[IAP] Purchases fetch failed: {failure.FailureReason}");
        }

        private void OnStoreDisconnected(StoreConnectionFailureDescription desc)
        {
            Debug.LogError($"[IAP] Store disconnected: {desc.Message}");
        }

        private void OnPurchasePending(PendingOrder pending)
        {
            try
            {
                Debug.Log($"Full receipt JSON: {pending.Info.Receipt}");

                CartItem firstItem = pending.CartOrdered.Items().FirstOrDefault();
                string pid = firstItem?.Product?.definition?.id;
                
                if (string.IsNullOrEmpty(pid))
                {
                    Debug.LogError("[IAP] Pending order has no product id.");
                    return;
                }

                Product product = m_StoreController?.GetProductById(pid);
                if (product == null)
                {
                    Debug.LogError($"[IAP] Product not found in controller: {pid}");
                    return;
                }

                Debug.Log($"[IAP] Pending purchase: {product.definition.id}");

                if (Enum.TryParse(product.definition.id, out PurchaseId id))
                {
                    OnSuccessfullyPurchased?.Invoke(id);
                }
                
                m_StoreController.ConfirmPurchase(pending);
                Debug.Log($"[IAP] Confirmed purchase: {product.definition.id}");
            }
            catch (Exception e)
            {
                Debug.LogError($"[IAP] Error processing pending order: {e.Message}");
            }
        }
        private void OnPurchaseConfirmed(Order order)
        {
            m_IsPurchaseInProgress = false;
            
            Product purchasedProduct = order.CartOrdered.Items().FirstOrDefault()?.Product;
            Debug.Log($"[IAP] Purchase confirmed: {purchasedProduct?.definition.id} | Tx: {order.Info?.TransactionID}");
        }

        private void OnPurchaseFailed(FailedOrder failed)
        {
            m_IsPurchaseInProgress = false;

            Debug.LogError($"[IAP] Purchase failed: {failed.FailureReason.ToString()}");
        }
    }
}