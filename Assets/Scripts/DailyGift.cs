using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGift : PopupBase
{
	public DataHolder dataHolder;

	public GameManager gameManager;

	public CellDailyGift[] cellDailyGifts;

	public Dictionary<string, int> indexDayOfWeek;

	public Image img_gift;

	public Text txt_gift;

	public Sprite spr_coin;

	public Sprite spr_arrowFire;

	public Sprite spr_protect;

	public Sprite spr_spin;

	public GameObject redNode;

	private void Awake()
	{
		indexDayOfWeek = new Dictionary<string, int>();
		indexDayOfWeek.Add("monday", 0);
		indexDayOfWeek.Add("tuesday", 1);
		indexDayOfWeek.Add("wednesday", 2);
		indexDayOfWeek.Add("thursday", 3);
		indexDayOfWeek.Add("friday", 4);
		indexDayOfWeek.Add("saturday", 5);
		indexDayOfWeek.Add("sunday", 6);
	}

	private void Start()
	{
		// init();
		// rednodeActive();
		// base.gameObject.SetActive(value: false);
	}

	private void rednodeActive()
	{
		redNode.SetActive(activeRedNode());
	}

	private bool activeRedNode()
	{
		for (int i = 0; i < dataHolder.dailyGiftData.giftDays.Length; i++)
		{
			if (dataHolder.dailyGiftData.giftDays[i].typeOfDaily == TypeOfDaily.Get)
			{
				return true;
			}
		}
		return false;
	}

	private void init()
	{
		DateTime now = DateTime.Now;
		int num = indexDayOfWeek[now.DayOfWeek.ToString().ToLower()];
		if (!getIndexDay(now))
		{
			if (num > 0)
			{
				for (int num2 = num - 1; num2 >= 0; num2--)
				{
					dataHolder.dailyGiftData.giftDays[num2].typeOfDaily = TypeOfDaily.Miss;
					DateTime dateTime = now.AddDays(-(num - num2));
					setDay(dataHolder.dailyGiftData.giftDays[num2], dateTime.Day, dateTime.Month, dateTime.Year);
				}
			}
			if (num < 6)
			{
				for (int i = num + 1; i < 7; i++)
				{
					dataHolder.dailyGiftData.giftDays[i].typeOfDaily = TypeOfDaily.Wait;
					DateTime dateTime2 = now.AddDays(i - num);
					setDay(dataHolder.dailyGiftData.giftDays[i], dateTime2.Day, dateTime2.Month, dateTime2.Year);
				}
			}
		}
		else if (num > 0)
		{
			for (int j = 0; j < num; j++)
			{
				if (dataHolder.dailyGiftData.giftDays[j].typeOfDaily == TypeOfDaily.Wait)
				{
					dataHolder.dailyGiftData.giftDays[j].typeOfDaily = TypeOfDaily.Miss;
				}
			}
		}
		if (dataHolder.dailyGiftData.giftDays[num].typeOfDaily != TypeOfDaily.Done)
		{
			dataHolder.dailyGiftData.giftDays[num].typeOfDaily = TypeOfDaily.Get;
		}
		setDay(dataHolder.dailyGiftData.giftDays[num], now.Day, now.Month, now.Year);
		dataHolder.dailyGiftData.writePre();
		for (int k = 0; k < cellDailyGifts.Length; k++)
		{
			cellDailyGifts[k].onShow(dataHolder.dailyGiftData.giftDays[k].typeOfDaily);
		}
	}

	private void setDay(GiftDay giftDay, int day, int month, int year)
	{
		giftDay.day = day;
		giftDay.month = month;
		giftDay.year = year;
	}

	private bool getIndexDay(DateTime dateTime)
	{
		for (int i = 0; i < dataHolder.dailyGiftData.giftDays.Length; i++)
		{
			if (dateTime.Day == dataHolder.dailyGiftData.giftDays[i].day && dateTime.Month == dataHolder.dailyGiftData.giftDays[i].month && dateTime.Year == dataHolder.dailyGiftData.giftDays[i].year)
			{
				return true;
			}
		}
		return false;
	}

	public void btn_cellClick(int index)
	{
		if (dataHolder.dailyGiftData.giftDays[index].typeOfDaily != TypeOfDaily.Miss && dataHolder.dailyGiftData.giftDays[index].typeOfDaily != TypeOfDaily.Get)
		{
		}
	}

	private void getGift(int index)
	{
		switch (index)
		{
		case 0:
			dataHolder.gameData.coin += 50;
			break;
		case 1:
			dataHolder.gameData.coin += 75;
			break;
		}
	}

	public void btn_getGift(int index)
	{
		if (dataHolder.dailyGiftData.giftDays[index].typeOfDaily == TypeOfDaily.Get)
		{
			dataHolder.dailyGiftData.giftDays[index].typeOfDaily = TypeOfDaily.Done;
			onGetGift(index);
			cellDailyGifts[index].onShow(dataHolder.dailyGiftData.giftDays[index].typeOfDaily);
			rednodeActive();
		}
		SoundManager.ins.play_audioClick();
	}

	public void btn_watchVideo(int index)
	{
		SoundManager.ins.play_audioClick();
		VideoRewardController.onShowVideoReward(delegate
		{
			onVideoSuccess(index);
		}, delegate
		{
			onVideoFail();
		});
	}

	private void onVideoFail()
	{
	}

	private void onVideoSuccess(int index)
	{
		dataHolder.dailyGiftData.giftDays[index].typeOfDaily = TypeOfDaily.Done;
		onGetGift(index);
		cellDailyGifts[index].onShow(dataHolder.dailyGiftData.giftDays[index].typeOfDaily);
		rednodeActive();
		dataHolder.gameData.numWatchVideo++;
		gameManager.saveData();
	}

	private void onGetGift(int index)
	{
		switch (index)
		{
		case 0:
			gameManager.addCoin(50);
			img_gift.sprite = spr_coin;
			txt_gift.text = "50";
			break;
		case 1:
			gameManager.addCoin(75);
			img_gift.sprite = spr_coin;
			txt_gift.text = "75";
			break;
		case 2:
			dataHolder.gameData.numFireArrow++;
			img_gift.sprite = spr_arrowFire;
			txt_gift.text = "x1";
			break;
		case 3:
			gameManager.addCoin(125);
			img_gift.sprite = spr_coin;
			txt_gift.text = "125";
			break;
		case 4:
			gameManager.addCoin(150);
			img_gift.sprite = spr_coin;
			txt_gift.text = "150";
			break;
		case 5:
			dataHolder.gameData.numProtect += 2;
			img_gift.sprite = spr_protect;
			txt_gift.text = "x2";
			break;
		case 6:
			dataHolder.gameData.numSpin += 2;
			img_gift.sprite = spr_spin;
			txt_gift.text = "x2";
			break;
		}
		img_gift.transform.parent.parent.parent.gameObject.SetActive(value: true);
		gameManager.saveData();
		dataHolder.dailyGiftData.writePre();
	}
}
