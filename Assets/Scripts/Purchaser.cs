using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class Purchaser : MonoBehaviour, IStoreListener
{
	public GameManager gameManager;

	public GameObject eff;

	public Image img;

	public Text txt;

	public Sprite spr_coin;

	public static Purchaser ins;

	private static IStoreController m_StoreController;

	private static IExtensionProvider m_StoreExtensionProvider;

	public static string kProduct_099 = "stick.archer.099";

	public static string kProduct_299 = "stick.archer.299";

	public static string kProduct_499 = "stick.archer.499";

	public static string kProduct_999 = "stick.archer.999";

	public static string kProduct_1999 = "stick.archer.1999";

	public static string kProduct_4999 = "stick.archer.4999";

	public static string kProduct_StarterPack = "stick.starter.pack";

	public GameObject effectBuySuccess;

	private void Awake()
	{
		if (m_StoreController == null)
		{
			InitializePurchasing();
		}
	}

	public void InitializePurchasing()
	{
		if (!IsInitialized())
		{
			ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
			configurationBuilder.AddProduct(kProduct_099, ProductType.Consumable);
			configurationBuilder.AddProduct(kProduct_299, ProductType.Consumable);
			configurationBuilder.AddProduct(kProduct_499, ProductType.Consumable);
			configurationBuilder.AddProduct(kProduct_999, ProductType.Consumable);
			configurationBuilder.AddProduct(kProduct_1999, ProductType.Consumable);
			configurationBuilder.AddProduct(kProduct_4999, ProductType.Consumable);
			configurationBuilder.AddProduct(kProduct_StarterPack, ProductType.Consumable);
			UnityPurchasing.Initialize(this, configurationBuilder);
		}
	}

	private bool IsInitialized()
	{
		return m_StoreController != null && m_StoreExtensionProvider != null;
	}

	public void BuykProduct_099()
	{
		BuyProductID(kProduct_099);
		FireBaseAnalyticControl.ins.AnalyticClickBuyIAP(kProduct_099);
	}

	public void BuykProduct_299()
	{
		BuyProductID(kProduct_299);
		FireBaseAnalyticControl.ins.AnalyticClickBuyIAP(kProduct_299);
	}

	public void BuykProduct_499()
	{
		BuyProductID(kProduct_499);
		FireBaseAnalyticControl.ins.AnalyticClickBuyIAP(kProduct_499);
	}

	public void BuykProduct_999()
	{
		BuyProductID(kProduct_999);
		FireBaseAnalyticControl.ins.AnalyticClickBuyIAP(kProduct_999);
	}

	public void BuykProduct_1999()
	{
		BuyProductID(kProduct_1999);
		FireBaseAnalyticControl.ins.AnalyticClickBuyIAP(kProduct_1999);
	}

	public void BuykProduct_4999()
	{
		BuyProductID(kProduct_4999);
		FireBaseAnalyticControl.ins.AnalyticClickBuyIAP(kProduct_4999);
	}

	public void BuykProduct_starterPack()
	{
		BuyProductID(kProduct_StarterPack);
		FireBaseAnalyticControl.ins.AnalyticClickBuyIAP(kProduct_StarterPack);
	}

	public static string getLocalizePrice(string key)
	{
		if (m_StoreController != null)
		{
			return m_StoreController.products.WithID(key).metadata.localizedPriceString;
		}
		return string.Empty;
	}

	private void BuyProductID(string productId)
	{
		if (IsInitialized())
		{
			Product product = m_StoreController.products.WithID(productId);
			if (product != null && product.availableToPurchase)
			{
				UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
				m_StoreController.InitiatePurchase(product);
			}
			else
			{
				UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
			}
		}
		else
		{
			UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
		}
	}

	public void RestorePurchases()
	{
		if (!IsInitialized())
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			UnityEngine.Debug.Log("RestorePurchases started ...");
			IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			extension.RestoreTransactions(delegate(bool result)
			{
				UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
			});
		}
		else
		{
			UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		UnityEngine.Debug.Log("OnInitialized: PASS");
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
	{
		if (string.Equals(args.purchasedProduct.definition.id, kProduct_099, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			purchaserSuccess(700);
		}
		else if (string.Equals(args.purchasedProduct.definition.id, kProduct_299, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			purchaserSuccess(2250);
		}
		else if (string.Equals(args.purchasedProduct.definition.id, kProduct_499, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			purchaserSuccess(4500);
		}
		else if (string.Equals(args.purchasedProduct.definition.id, kProduct_999, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			purchaserSuccess(8000);
		}
		else if (string.Equals(args.purchasedProduct.definition.id, kProduct_1999, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			purchaserSuccess(20000);
		}
		else if (string.Equals(args.purchasedProduct.definition.id, kProduct_4999, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			purchaserSuccess(100000);
		}
		else if (string.Equals(args.purchasedProduct.definition.id, kProduct_StarterPack, StringComparison.Ordinal))
		{
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
			gameManager.addCoin(500);
			gameManager.dataHolder.gameData.numSpin += 2;
			gameManager.dataHolder.gameData.numProtect++;
			gameManager.dataHolder.gameData.numFireArrow++;
			gameManager.saveData();
			gameManager.initTool();
			NotificationPopup.ins.onShow("Congratulations, Buy Starter Pack success! ");
			effectBuySuccess.SetActive(value: true);
			Invoke("disableEffectBuySuccess", 3f);
		}
		else
		{
			UnityEngine.Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
		}
		return PurchaseProcessingResult.Complete;
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
	}

	public void purchaserSuccess(int numCoin)
	{
		img.sprite = spr_coin;
		txt.text = "+" + numCoin;
		eff.SetActive(value: true);
		gameManager.addCoin(numCoin);
		gameManager.saveData();
	}

	private void disableEffectBuySuccess()
	{
		effectBuySuccess.SetActive(value: false);
	}
}
