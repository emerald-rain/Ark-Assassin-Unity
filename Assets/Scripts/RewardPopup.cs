using UnityEngine;
using UnityEngine.UI;

public class RewardPopup : MonoBehaviour
{
	public RectTransform imgSelect;

	private int[] posX = new int[4]
	{
		-262,
		-88,
		88,
		262
	};

	private int[] coinReward = new int[4]
	{
		25,
		50,
		75,
		125
	};

	public bool isRun;

	private float count;

	public float speed;

	public RectTransform effStar;

	public GameManager gameManager;

	public GameObject upgradeReward;

	public Text txt_before;

	public GameObject giftSuccess;

	public Text txt_gifSuccess;

	public Image img_gifSuccess;

	public Sprite spr_coin;

	public GameObject fade;

	public GameObject btnClaim;

	public GameObject ratePopup;

	private void OnEnable()
	{
		effStar.gameObject.SetActive(value: false);
		imgSelect.anchoredPosition = new Vector2(posX[0], 0f);
		speed = UnityEngine.Random.Range(12f, 22f);
		isRun = true;
		btnClaim.SetActive(value: false);
	}

	private void Update()
	{
		if (isRun)
		{
			if (count >= (float)posX.Length)
			{
				count = 0f;
			}
			imgSelect.anchoredPosition = new Vector2(posX[(int)count], 0f);
			count += Time.deltaTime * speed;
			if (speed > 0f)
			{
				speed -= Time.deltaTime * 5f;
			}
			if (speed < 0f)
			{
				isRun = false;
				effStar.anchoredPosition = new Vector2(posX[getIndex()], 0f);
				effStar.gameObject.SetActive(value: true);
				onDone();
				btnClaim.SetActive(value: true);
			}
		}
	}

	private int getIndex()
	{
		for (int i = 0; i < posX.Length; i++)
		{
			Vector2 anchoredPosition = imgSelect.anchoredPosition;
			if (anchoredPosition.x == (float)posX[i])
			{
				return i;
			}
		}
		return 0;
	}

	private void onDone()
	{
		if (getIndex() == 3)
		{
			gameManager.addCoin(125);
			gameManager.saveData();
			showGifSuccess(125);
		}
		else
		{
			txt_before.text = "x" + coinReward[getIndex()];
			upgradeReward.SetActive(value: true);
		}
	}

	public void btn_no()
	{
		gameManager.addCoin(coinReward[getIndex()]);
		gameManager.saveData();
		upgradeReward.SetActive(value: false);
		showGifSuccess(coinReward[getIndex()]);
		SoundManager.ins.play_audioClick();
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
		SoundManager.ins.play_audioClick();
	}

	private void watchVideoSuccess()
	{
		gameManager.addCoin(125);
		gameManager.saveData();
		showGifSuccess(125);
		imgSelect.anchoredPosition = new Vector2(posX[3], 0f);
		effStar.anchoredPosition = new Vector2(posX[3], 0f);
		effStar.gameObject.SetActive(value: true);
		upgradeReward.SetActive(value: false);
		gameManager.dataHolder.gameData.numWatchVideo++;
		gameManager.saveData();
	}

	private void watchVideoFail()
	{
		NotificationPopup.ins.onShow("Video not Ready. Please comeback later!");
		upgradeReward.SetActive(value: false);
	}

	private void showGifSuccess(int getCoin)
	{
		img_gifSuccess.sprite = spr_coin;
		txt_gifSuccess.text = "x" + getCoin;
		giftSuccess.SetActive(value: true);
	}

	public void btn_onClose()
	{
		if (!isRun)
		{
			base.gameObject.SetActive(value: false);
			fade.SetActive(value: true);
			if (PlayerPrefs.GetInt("click_rate") == 0 && gameManager.dataHolder.gameData.missionNum % 5 == 0)
			{
				ratePopup.SetActive(value: true);
			}
		}
	}
}
