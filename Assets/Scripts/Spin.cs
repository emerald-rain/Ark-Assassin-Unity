using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spin : PopupBase
{
	private int[] rotation_slot;

	private int[] percent_slot;

	private List<int> list;

	public List<int> listRd;

	public bool isSpinning;

	public GameObject objSpin;

	public DataHolder dataHolder;

	public Image img_gift;

	public Text txt_gift;

	public Sprite spr_coin;

	public Sprite spr_protect;

	public Sprite spr_healing;

	public Sprite spr_spin;

	public Sprite spr_arrowFire;

	public Text txtNumSpin;

	public GameManager gameManager;

	public GameObject notifyNotEnough;

	private void Awake()
	{
		list = new List<int>();
		listRd = new List<int>();
		rotation_slot = new int[8]
		{
			0,
			45,
			90,
			135,
			180,
			225,
			270,
			315
		};
		percent_slot = new int[8]
		{
			10,
			15,
			15,
			10,
			10,
			15,
			15,
			10
		};
		for (int i = 0; i < percent_slot.Length; i++)
		{
			for (int j = 0; j < percent_slot[i]; j++)
			{
				list.Add(rotation_slot[i]);
			}
		}
		while (list.Count > 0)
		{
			int index = Random.Range(0, list.Count);
			listRd.Add(list[index]);
			list.RemoveAt(index);
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		updateSpin();
	}

	public void btn_spin()
	{
		if (!isSpinning)
		{
			if (dataHolder.gameData.numSpin > 0)
			{
				dataHolder.gameData.numSpin--;
				onSpin();
			}
			else
			{
				NotificationPopup.ins.onShow("No more spin for free. You can spin by Coin or watch video to get spin free");
			}
		}
		SoundManager.ins.play_audioClick();
	}

	public void btn_spinByCoin()
	{
		if (!isSpinning)
		{
			if (dataHolder.gameData.coin >= 100)
			{
				gameManager.addCoin(-100);
				gameManager.saveData();
				onSpin();
			}
			else
			{
				notifyNotEnough.SetActive(value: true);
			}
		}
		SoundManager.ins.play_audioClick();
	}

	public void btn_spinByVideo()
	{
		if (!isSpinning)
		{
			VideoRewardController.onShowVideoReward(delegate
			{
				onSpin();
				dataHolder.gameData.numWatchVideo++;
				gameManager.saveData();
			}, delegate
			{
				NotificationPopup.ins.onShow("Video not Ready. Please come back later");
			});
		}
		SoundManager.ins.play_audioClick();
	}

	public void onSpin()
	{
		isSpinning = true;
		int rd = Random.Range(0, listRd.Count);
		iTween.RotateTo(objSpin, iTween.Hash("rotation", new Vector3(0f, 0f, listRd[rd] - 3600), "time", 3, "easetype", iTween.EaseType.easeInOutCirc));
		delayFunction(3f, delegate
		{
			isSpinning = false;
			getGift(getIndex(listRd[rd]));
			updateSpin();
		});
		updateSpin();
		FireBaseAnalyticControl.ins.AnalyticPlaySpin();
	}

	private void updateSpin()
	{
		txtNumSpin.text = dataHolder.gameData.numSpin + string.Empty;
	}

	private int getIndex(int value)
	{
		for (int i = 0; i < rotation_slot.Length; i++)
		{
			if (rotation_slot[i] == value)
			{
				return i;
			}
		}
		return 0;
	}

	public override void onClose()
	{
		if (!isSpinning)
		{
			GameManager.popupBase = null;
			base.gameObject.SetActive(value: false);
			if (fade != null)
			{
				fade.SetActive(value: true);
			}
		}
		SoundManager.ins.play_audioClick();
	}

	private void getGift(int index)
	{
		string content = string.Empty;
		switch (index)
		{
		case 0:
			gameManager.addCoin(500);
			img_gift.sprite = spr_coin;
			txt_gift.text = "500";
			content = "lucky wheel : get 500 coin";
			break;
		case 1:
			dataHolder.gameData.numProtect++;
			img_gift.sprite = spr_protect;
			txt_gift.text = "x1";
			content = "lucky wheel : get protect x1";
			break;
		case 2:
			gameManager.addCoin(25);
			img_gift.sprite = spr_coin;
			txt_gift.text = "25";
			content = "lucky wheel : get 25 coin";
			break;
		case 3:
			dataHolder.gameData.numHealing += 3;
			img_gift.sprite = spr_healing;
			txt_gift.text = "x3";
			content = "lucky wheel : get healing x3";
			break;
		case 4:
			gameManager.addCoin(75);
			img_gift.sprite = spr_coin;
			txt_gift.text = "75";
			content = "lucky wheel : get 75 coin";
			break;
		case 5:
			dataHolder.gameData.numSpin++;
			img_gift.sprite = spr_spin;
			txt_gift.text = "x1";
			content = "lucky wheel : get num spin x1";
			break;
		case 6:
			dataHolder.gameData.numFireArrow++;
			img_gift.sprite = spr_arrowFire;
			txt_gift.text = "x1";
			content = "lucky wheel : get arrow fire x1";
			break;
		case 7:
			gameManager.addCoin(50);
			img_gift.sprite = spr_coin;
			txt_gift.text = "50";
			content = "lucky wheel : get 50 coin";
			break;
		}
		img_gift.transform.parent.parent.parent.gameObject.SetActive(value: true);
		gameManager.saveData();
		gameManager.initTool();
		FireBaseAnalyticControl.ins.AnalyticLogSpin(content);
	}
}
