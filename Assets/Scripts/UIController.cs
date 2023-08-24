using UnityEngine;
using UnityEngine.UI;

public class UIController : E_MonoBehaviour
{
	public GameManager gameManager;

	public GameObject startGroup;

	public GameObject infoGroup;

	public GameObject btnTapToStart;

	public Text txtCoin;

	public Animator topGoldAnim;

	public GameObject[] objGameOver;

	public Text over_txtCoin;

	public Text over_txtBest;

	public Text over_txtScore;

	public GameObject[] objGameWin;

	public GameObject fade;

	public Text txtScore;

	private int value;

	private bool is_count;

	private int score;

	public GameObject gameResult;

	private Animator animGroupStart;

	public Camera _camera;

	public GameObject shopGroup;

	public Text txtLevel;

	public Image fillHpBoss;

	public GameObject continuePopup;

	public Text txt_timeContinue;

	private int count_timeContinue;

	public ShopController shopController;

	private void Start()
	{
		animGroupStart = startGroup.GetComponent<Animator>();
	}

	public void onStart()
	{
		btnTapToStart.SetActive(value: true);
	}

	public void onClick_btnStart()
	{
		btnTapToStart.SetActive(value: false);
		animGroupStart.Play("startGroupClose");
		gameManager.onStartGame();
		delayFunction(1f, delegate
		{
			startGroup.SetActive(value: false);
			infoGroup.SetActive(value: true);
		});
		setotherCamrera(3f, CameraMove.cameraOrtho, 1f);
	}

	public void setScore(int score)
	{
		txtScore.text = score + string.Empty;
	}

	public void setPerHpBoss(float per)
	{
		fillHpBoss.fillAmount = per;
	}

	public void setTextCoin(int coinNum)
	{
		txtCoin.text = coinNum + string.Empty;
		topGoldAnim.Play("coinAnimEff");
	}

	public void setTextLevel(int level)
	{
		txtLevel.text = "Level : " + level;
	}

	public void gameResultOnShow(bool isWin, int coin, int score, int highScore)
	{
		this.score = score;
		gameResult.SetActive(value: true);
		if (isWin)
		{
			for (int i = 0; i < objGameWin.Length; i++)
			{
				objGameWin[i].SetActive(value: true);
			}
			for (int j = 0; j < objGameOver.Length; j++)
			{
				objGameOver[j].SetActive(value: false);
			}
		}
		else
		{
			for (int k = 0; k < objGameWin.Length; k++)
			{
				objGameWin[k].SetActive(value: false);
			}
			for (int l = 0; l < objGameOver.Length; l++)
			{
				objGameOver[l].SetActive(value: true);
			}
		}
		over_txtCoin.text = "Coin : " + coin;
		over_txtBest.text = "Best : " + highScore;
		setValueTo(0f, score, 1f, 0f);
		is_count = true;
	}

	public void onShowContinue()
	{
		count_timeContinue = 10;
		txt_timeContinue.text = count_timeContinue + string.Empty;
		continuePopup.SetActive(value: true);
		InvokeRepeating("OnCountTimeContinueGame", 1f, 1f);
	}

	private void OnCountTimeContinueGame()
	{
		count_timeContinue--;
		txt_timeContinue.text = count_timeContinue + string.Empty;
		if (count_timeContinue == 0)
		{
			btn_cancelContinue();
		}
	}

	public void btn_cancelContinue()
	{
		CancelInvoke("OnCountTimeContinueGame");
		continuePopup.SetActive(value: false);
		gameManager.onShowDialogGameLose();
	}

	public void btn_watchVideoToContinue()
	{
		CancelInvoke("OnCountTimeContinueGame");
		continuePopup.SetActive(value: false);
		gameManager.onWatchVideoToContinue();
	}

	private void Update()
	{
		if (is_count)
		{
			if (value < score)
			{
				over_txtScore.text = value + string.Empty;
				return;
			}
			over_txtScore.text = score + string.Empty;
			iTween.ScaleFrom(over_txtScore.gameObject, iTween.Hash("scale", new Vector3(1.5f, 1.5f, 1.5f), "time", 0.5f, "easetype", iTween.EaseType.easeInOutBack));
			is_count = false;
		}
	}

	public void onClickReplay()
	{
		gameResult.SetActive(value: false);
		gameManager.replay();
	}

	public void onClickPlay()
	{
		gameResult.SetActive(value: false);
		gameManager.NextLevel();
	}

	public void onHome()
	{
		gameResult.SetActive(value: false);
		gameManager.btn_home();
	}

	public void setFadePanel()
	{
		fade.SetActive(value: true);
	}

	private void setValueTo(float _from, float _to, float _time, float timeDelay)
	{
		iTween.ValueTo(base.gameObject, iTween.Hash("from", _from, "to", _to, "time", _time, "delay", timeDelay, "onupdatetarget", base.gameObject, "onupdate", "tweenOnUpdateCallBack", "easetype", iTween.EaseType.easeOutQuad));
	}

	private void tweenOnUpdateCallBack(int newValue)
	{
		value = newValue;
	}

	public void btn_openShop()
	{
		GameManager.is_camMove = false;
		txtScore.gameObject.SetActive(value: false);
		iTween.MoveTo(_camera.gameObject, new Vector3(0f, -2.5f, -10f), 1f);
		setotherCamrera(3f, 3.5f, 1f);
		tweenColor(1f, 0.2f, 1f);
		animGroupStart.Play("startGroupClose");
		shopGroup.SetActive(value: true);
		infoGroup.SetActive(value: false);
		shopGroup.GetComponent<Animator>().Play("shopOpen", 0, 0f);
		delayFunction(1f, delegate
		{
			startGroup.SetActive(value: false);
		});
	}

	public void btn_openShopCoin()
	{
		if (startGroup.activeSelf && !GameManager.is_gameStart)
		{
			GameManager.is_camMove = false;
			txtScore.gameObject.SetActive(value: false);
			iTween.MoveTo(_camera.gameObject, new Vector3(0f, -2.5f, -10f), 1f);
			setotherCamrera(3f, 3.5f, 1f);
			tweenColor(1f, 0.2f, 1f);
			animGroupStart.Play("startGroupClose");
			shopGroup.SetActive(value: true);
			infoGroup.SetActive(value: false);
			shopGroup.GetComponent<Animator>().Play("shopOpen", 0, 0f);
			shopController.btn_tab(1);
			delayFunction(1f, delegate
			{
				startGroup.SetActive(value: false);
			});
		}
	}

	public void btn_closeShop()
	{
		gameManager.setUpArchery();
		shopGroup.SetActive(value: false);
		iTween.MoveTo(_camera.gameObject, new Vector3(0f, 0f, -10f), 1f);
		setotherCamrera(3.5f, 3f, 1f);
		tweenColor(0.2f, 1f, 1f);
		delayFunction(1f, delegate
		{
			GameManager.is_camMove = true;
			txtScore.gameObject.SetActive(value: true);
			startGroup.SetActive(value: true);
			animGroupStart.Play("startGroupOpen");
		});
		SoundManager.ins.play_audioClick();
	}

	public void setotherCamrera(float _from, float _to, float _time)
	{
		iTween.ValueTo(base.gameObject, iTween.Hash("from", _from, "to", _to, "time", _time, "onupdatetarget", base.gameObject, "onupdate", "tweenCamOnUpdateCallBack", "easetype", iTween.EaseType.easeOutQuad));
	}

	private void tweenCamOnUpdateCallBack(float newValue)
	{
		_camera.orthographicSize = newValue;
	}

	private void tweenColor(float _from, float _to, float _time)
	{
		iTween.ValueTo(base.gameObject, iTween.Hash("from", _from, "to", _to, "time", _time, "onupdatetarget", base.gameObject, "onupdate", "tweenColorOnUpdateCallBack", "easetype", iTween.EaseType.easeOutQuad));
	}

	private void tweenColorOnUpdateCallBack(float newValue)
	{
		gameManager.listPlatform[0].setColor(new Color(gameManager.colorStart.r * newValue, gameManager.colorStart.g * newValue, gameManager.colorStart.b * newValue));
		_camera.backgroundColor = new Color(gameManager.colorStart.r * newValue, gameManager.colorStart.g * newValue, gameManager.colorStart.b * newValue);
	}
}
