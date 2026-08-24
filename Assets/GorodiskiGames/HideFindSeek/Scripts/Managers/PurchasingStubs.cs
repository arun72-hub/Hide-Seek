using System;
using System.Threading.Tasks;

namespace UnityEngine.Purchasing
{
    public enum ProductType
    {
        Consumable,
        NonConsumable,
        Subscription
    }

    public enum PurchaseProcessingResult
    {
        Complete,
        Pending
    }

    public enum InitializationFailureReason
    {
        AppNotKnown,
        PurchasingUnavailable,
        NoProductsAvailable,
        StateUnavailable
    }

    public enum PurchaseFailureReason
    {
        PurchasingUnavailable,
        ExistingPurchasePending,
        ProductUnavailable,
        SignatureInvalid,
        UserCancelled,
        PaymentDeclined,
        Unknown
    }

    public enum FakeStoreUIMode
    {
        Default,
        DeveloperUser
    }

    public interface IDetailedStoreListener
    {
        void OnInitialized(IStoreController controller, IExtensionProvider extensions);
        void OnInitializeFailed(InitializationFailureReason error);
        void OnInitializeFailed(InitializationFailureReason error, string message);
        PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args);
        void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason);
        void OnPurchaseFailed(Product product, Extension.PurchaseFailureDescription failureDescription);
    }

    public interface IStoreController
    {
        ProductCollection products { get; }
        void InitiatePurchase(Product product);
    }

    public class ProductCollection
    {
        public Product[] all => new Product[0];
        public Product WithID(string id) => null;
    }

    public interface IExtensionProvider
    {
        T GetExtension<T>() where T : class;
    }

    public interface IAppleExtensions
    {
        void RestoreTransactions(Action<bool, string> callback);
    }

    public class ProductMetadata
    {
        public string localizedTitle => "";
        public string localizedPriceString => "";
    }

    public class ProductDefinition
    {
        public string id => "";
    }

    public class Product
    {
        public bool availableToPurchase => false;
        public ProductMetadata metadata => new ProductMetadata();
        public ProductDefinition definition => new ProductDefinition();
    }

    public class PurchaseEventArgs
    {
        public Product purchasedProduct => new Product();
    }

    public class StandardPurchasingModule
    {
        public static StandardPurchasingModule Instance() => new StandardPurchasingModule();
        public bool useFakeStoreAlways;
        public FakeStoreUIMode useFakeStoreUIMode;
    }

    public class ConfigurationBuilder
    {
        public static ConfigurationBuilder Instance(StandardPurchasingModule module) => new ConfigurationBuilder();
        public void AddProduct(string id, ProductType type) { }
    }

    public class UnityPurchasing
    {
        public static void Initialize(IDetailedStoreListener listener, ConfigurationBuilder builder)
        {
            listener?.OnInitializeFailed(InitializationFailureReason.PurchasingUnavailable);
        }
    }

    namespace Extension
    {
        public class PurchaseFailureDescription
        {
        }
    }
}
