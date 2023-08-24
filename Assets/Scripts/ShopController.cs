using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : PopupBase
{
	public Sprite spr_normal;

	public Sprite spr_select;

	public Image[] imgButtons;

	public GameObject[] groups;

	public DataHolder dataHolder;

	public GameManager gameManager;

	[Header("----- Archery -----")]
	public Transform shopArcheryParrent;

	public CellShop cellArcheryPrefabs;

	public List<CellShop> listArcheryShop;

	private CellShop cellArcheryEquiped;

	public AudioSource audioSource;

	public AudioClip audio_buyArcherySuccess;

	public AudioClip audio_upgradeArcherySuccess;

	[Header("----- Upgrade Archery -----")]
	public Image fill_attack;

	public Image fill_defense;

	public Image fill_hp;

	public Text txt_attackpoint;

	public Text txt_defensepoint;

	public Text txt_hppoint;

	public Text txt_coinUpgrade;

	public Text txt_upgrade;

	private int[] coinUpgradeArchery = new int[6]
	{
		200,
		400,
		650,
		1000,
		1500,
		1500
	};

	public UIController uIController;

	private void Start()
	{
		btn_tab(0);
		onCreateTabArchery();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		onShowInfoArchery(dataHolder.gameData.ArcheryId);
	}

	public void btn_tab(int index)
	{
		for (int i = 0; i < imgButtons.Length; i++)
		{
			if (i == index)
			{
				imgButtons[i].sprite = spr_select;
				if (groups[i] != null)
				{
					groups[i].SetActive(value: true);
				}
			}
			else
			{
				imgButtons[i].sprite = spr_normal;
				if (groups[i] != null)
				{
					groups[i].SetActive(value: false);
				}
			}
		}
		SoundManager.ins.play_audioClick();
	}

	public void onCreateTabArchery()
	{
		List<ArcheryD> listArchery = dataHolder.archeryData.listArchery;
		for (int i = 0; i < listArchery.Count; i++)
		{
			CellShop cellShop = UnityEngine.Object.Instantiate(cellArcheryPrefabs);
			cellShop.transform.SetParent(shopArcheryParrent, worldPositionStays: false);
			cellShop.onShow(listArchery[i].id, dataHolder.archeryAvatar[i], listArchery[i].prince, listArchery[i].bought, listArchery[i].numWatchVideo, listArchery[i].numWatchVideoToGetFree, this);
			listArcheryShop.Add(cellShop);
		}
		if (dataHolder.archeryData.GetArcheryD(dataHolder.gameData.ArcheryId).bought)
		{
			cellArcheryEquiped = GetCellArchery(dataHolder.gameData.ArcheryId);
			cellArcheryEquiped.onEquiped();
			return;
		}
		cellArcheryEquiped = GetCellArchery(0);
		cellArcheryEquiped.onEquiped();
		dataHolder.gameData.ArcheryId = 0;
		dataHolder.gameData.writePre();
	}

	private CellShop GetCellArchery(int id)
	{
		for (int i = 0; i < listArcheryShop.Count; i++)
		{
			if (listArcheryShop[i].id == id)
			{
				return listArcheryShop[i];
			}
		}
		return null;
	}

	public void onBuy(int idItem, int prince, Action buySuccess)
	{
		ArcheryD archeryD = dataHolder.archeryData.GetArcheryD(idItem);
		if (archeryD != null)
		{
			if (prince > gameManager.getCoin())
			{
				NotificationPopup.ins.onShow("Not Enough Coin! \n Do you want go to Shop?", delegate
				{
					btn_tab(1);
				});
				return;
			}
			gameManager.addCoin(-prince);
			gameManager.saveData();
			NotificationPopup.ins.onShow("Congratulations, Buy success! ");
			buySuccess();
			archeryD.bought = true;
			dataHolder.gameData.unlockArchery++;
			dataHolder.archeryData.writePre();
			playAudio(audio_buyArcherySuccess);
			FireBaseAnalyticControl.ins.AnalyticBuyArchery(idItem);
		}
	}

	public void onWatchVideo(int id, CellShop cell)
	{
		VideoRewardController.onShowVideoReward(delegate
		{
			videoSuccess(id, cell);
		}, delegate
		{
			videoFail();
		});
	}

	private void videoSuccess(int id, CellShop cell)
	{
		List<ArcheryD> listArchery = dataHolder.archeryData.listArchery;
		listArchery[id].numWatchVideo++;
		if (dataHolder.archeryData.listArchery[id].numWatchVideo >= dataHolder.archeryData.listArchery[id].numWatchVideoToGetFree)
		{
			dataHolder.archeryData.listArchery[id].bought = true;
			dataHolder.archeryData.writePre();
			dataHolder.gameData.unlockArchery++;
			dataHolder.archeryData.writePre();
			NotificationPopup.ins.onShow("Congratulations, Unlock success! ");
		}
		cell.onShow(listArchery[id].id, dataHolder.archeryAvatar[id], listArchery[id].prince, listArchery[id].bought, listArchery[id].numWatchVideo, listArchery[id].numWatchVideoToGetFree, this);
		FireBaseAnalyticControl.ins.AnalyticWatchVideoReward("Watch Video Get Archery");
		gameManager.dataHolder.gameData.numWatchVideo++;
		gameManager.saveData();
	}

	private void videoFail()
	{
		FireBaseAnalyticControl.ins.AnalyticWatchVideoReward_notReady();
	}

	public void onReview(int id)
	{
		gameManager.setSpriteArchery(id);
		onShowInfoArchery(id);
	}

	public void onEquip(int id, Action success)
	{
		cellArcheryEquiped.onDisEquip();
		success();
		cellArcheryEquiped = GetCellArchery(id);
		dataHolder.gameData.ArcheryId = id;
		dataHolder.gameData.writePre();
		gameManager.setUpArchery();
		onShowInfoArchery(id);
	}

	public void onShowInfoArchery(int id)
	{
		ArcheryD archeryD = dataHolder.archeryData.GetArcheryD(id);
		txt_attackpoint.text = "+" + (archeryD.dame + archeryD.level * 2);
		txt_defensepoint.text = "+" + (archeryD.defense + archeryD.level);
		txt_hppoint.text = "+" + (archeryD.hp + archeryD.level * 3);
		fill_attack.fillAmount = (float)(archeryD.dame + archeryD.level * 2) / 40f;
		fill_defense.fillAmount = (float)(archeryD.defense + archeryD.level) / 30f;
		fill_hp.fillAmount = (float)(archeryD.hp + archeryD.level * 3) / 50f;
		if (archeryD.bought)
		{
			txt_upgrade.gameObject.SetActive(value: true);
			if (archeryD.level > 4)
			{
				txt_upgrade.text = "upgrade MAX level";
				txt_coinUpgrade.gameObject.SetActive(value: false);
			}
			else
			{
				txt_upgrade.text = "upgrade next level :";
				txt_coinUpgrade.text = coinUpgradeArchery[archeryD.level] + string.Empty;
				txt_coinUpgrade.gameObject.SetActive(value: true);
			}
		}
		else
		{
			txt_upgrade.gameObject.SetActive(value: false);
		}
	}

	public void btn_upgradeArchery(int id)
	{
		ArcheryD archeryD = dataHolder.archeryData.GetArcheryD(id);
		if (archeryD.level < 5)
		{
			if (gameManager.getCoin() >= coinUpgradeArchery[archeryD.level])
			{
				dataHolder.archeryData.writePre();
				gameManager.addCoin(-coinUpgradeArchery[archeryD.level]);
				archeryD.level++;
				onShowInfoArchery(id);
				gameManager.saveData();
				gameManager.hero.onShowEffectUpgradeArchery();
				playAudio(audio_upgradeArcherySuccess);
			}
			else
			{
				NotificationPopup.ins.onShow("Not Enough Coin! \n Do you want go to Shop?", delegate
				{
					btn_tab(1);
				});
			}
		}
		SoundManager.ins.play_audioClick();
	}

	public override void onClose()
	{
		GameManager.popupBase = null;
		if (fade != null)
		{
			fade.SetActive(value: true);
		}
		uIController.btn_closeShop();
	}

	private void playAudio(AudioClip audioClip)
	{
		audioSource.volume = SoundManager.ins.volumeSound;
		audioSource.clip = audioClip;
		audioSource.Play();
	}
}
