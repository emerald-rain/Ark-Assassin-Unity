using System;
using UnityEngine;
using UnityEngine.UI;

public class VideoRewardPopup : MonoBehaviour
{
	public Button btnClaim;

	public GameObject btnDone;

	public RectTransform[] btnTabs;

	public GameObject[] giftGroups;

	public int index;

	public GameManager gameManager;

	public GameObject effectSuccess;

	private void OnEnable()
	{
		string @string = PlayerPrefs.GetString("day_reward_video");
		if (@string.Equals(string.Empty))
		{
			index = 0;
			PlayerPrefs.SetString("day_reward_video", DateTime.Now.ToString());
			PlayerPrefs.SetInt("index_reward_video", 0);
		}
		else
		{
			DateTime dateTime = DateTime.Parse(@string);
			if (DateTime.Now.Day == dateTime.Day && DateTime.Now.Month == dateTime.Month && DateTime.Now.Year == dateTime.Year)
			{
				index = PlayerPrefs.GetInt("index_reward_video");
			}
			else
			{
				index = 0;
				PlayerPrefs.SetString("day_reward_video", DateTime.Now.ToString());
				PlayerPrefs.SetInt("index_reward_video", 0);
			}
		}
		onShow(index);
	}

	public void onShow(int index)
	{
		if (index >= btnTabs.Length)
		{
			index = btnTabs.Length - 1;
		}
		for (int i = 0; i < btnTabs.Length; i++)
		{
			if (i == index)
			{
				btnTabs[i].localScale = new Vector3(1.2f, 1.2f, 1.2f);
				giftGroups[i].SetActive(value: true);
			}
			else
			{
				btnTabs[i].localScale = new Vector3(1f, 1f, 1f);
				giftGroups[i].SetActive(value: false);
			}
		}
		if (index < this.index)
		{
			btnDone.SetActive(value: true);
			btnClaim.gameObject.SetActive(value: false);
			return;
		}
		btnDone.SetActive(value: false);
		btnClaim.gameObject.SetActive(value: true);
		if (index == this.index)
		{
			btnClaim.interactable = true;
		}
		else
		{
			btnClaim.interactable = false;
		}
	}

	public void btn_watchVideo()
	{
		VideoRewardController.onShowVideoReward(delegate
		{
			watchVideoSuccess();
		}, delegate
		{
			watchVideoFail();
		});
	}

	private void watchVideoSuccess()
	{
		getGift();
		index++;
		PlayerPrefs.SetInt("index_reward_video", index);
		onShow(index);
	}

	private void watchVideoFail()
	{
	}

	private void getGift()
	{
		switch (index)
		{
		case 0:
			gameManager.addCoin(100);
			break;
		case 1:
			gameManager.addCoin(50);
			gameManager.dataHolder.gameData.numSpin++;
			break;
		case 2:
			gameManager.addCoin(50);
			gameManager.dataHolder.gameData.numSpin++;
			gameManager.dataHolder.gameData.numProtect++;
			break;
		case 3:
			gameManager.dataHolder.gameData.numFireArrow++;
			gameManager.dataHolder.gameData.numX3++;
			gameManager.dataHolder.gameData.numProtect++;
			break;
		case 4:
			gameManager.addCoin(150);
			gameManager.dataHolder.gameData.numHealing++;
			gameManager.dataHolder.gameData.numX3++;
			gameManager.dataHolder.gameData.numProtect++;
			break;
		}
		gameManager.saveData();
		gameManager.initTool();
		effectSuccess.SetActive(value: true);
		Invoke("disableEff", 3f);
		NotificationPopup.ins.onShow("Congratulations, Get Gift success! ");
	}

	private void disableEff()
	{
		effectSuccess.SetActive(value: false);
	}
}
