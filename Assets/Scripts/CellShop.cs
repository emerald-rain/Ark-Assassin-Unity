using UnityEngine;
using UnityEngine.UI;

public class CellShop : MonoBehaviour
{
	public int id;

	public Text txtPrince;

	public Image avatar;

	public GameObject btnBuy;

	public GameObject btnEquip;

	public GameObject btnEquiped;

	public GameObject btnWatchVideo;

	public GameObject btnInfo;

	public Text txtBtnWatchVideo;

	private int prince;

	public ShopController shopController;

	public void onShow(int id, Sprite avatar, int prince, bool bought, int numWatchVideo, int numVideoGetFree, ShopController shopController)
	{
		this.id = id;
		this.prince = prince;
		this.shopController = shopController;
		this.avatar.sprite = avatar;
		this.avatar.SetNativeSize();
		txtPrince.text = prince + string.Empty;
		txtBtnWatchVideo.text = numWatchVideo + "/" + numVideoGetFree + " Video";
		if (bought)
		{
			btnBuy.SetActive(value: false);
			btnEquip.SetActive(value: true);
			btnWatchVideo.SetActive(value: false);
			btnInfo.SetActive(value: true);
		}
		else
		{
			btnBuy.SetActive(value: true);
			btnEquip.SetActive(value: false);
			btnWatchVideo.SetActive(value: false);
			btnInfo.SetActive(value: false);
		}
		btnEquiped.SetActive(value: false);
	}

	public void btn_onBuy()
	{
		shopController.onBuy(id, prince, delegate
		{
			buySuccess();
		});
		SoundManager.ins.play_audioClick();
	}

	public void btn_onWatchVideo()
	{
		shopController.onWatchVideo(id, this);
		SoundManager.ins.play_audioClick();
	}

	private void buySuccess()
	{
		btnBuy.SetActive(value: false);
		btnEquip.SetActive(value: true);
		btnWatchVideo.SetActive(value: false);
		btnInfo.SetActive(value: true);
	}

	public void btnOnReview()
	{
		shopController.onReview(id);
		SoundManager.ins.play_audioClick();
	}

	public void onEquiped()
	{
		btnBuy.SetActive(value: false);
		btnEquip.SetActive(value: false);
		btnEquiped.SetActive(value: true);
		btnWatchVideo.SetActive(value: false);
		btnInfo.SetActive(value: true);
	}

	public void btn_onEquip()
	{
		shopController.onEquip(id, delegate
		{
			onEquiped();
		});
		SoundManager.ins.play_audioClick();
	}

	public void btn_onGetInfo()
	{
		shopController.onShowInfoArchery(id);
	}

	public void onDisEquip()
	{
		btnBuy.SetActive(value: false);
		btnEquip.SetActive(value: true);
		btnEquiped.SetActive(value: false);
		btnWatchVideo.SetActive(value: false);
		btnInfo.SetActive(value: true);
	}

	public void btn_upgrade()
	{
		shopController.btn_upgradeArchery(id);
	}
}
