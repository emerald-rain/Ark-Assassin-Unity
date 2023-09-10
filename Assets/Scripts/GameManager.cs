using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : E_MonoBehaviour
{
	public static bool is_gameStart;

	public static bool is_camMove;

	[Header("------ Camera ------")]
	public GameObject _camera;
	public PlayfabManager PlayfabManager;

	public Animator camAnim;

	[Header("------ Control ------")]
	public GameObject dotStart;

	public GameObject dotEnd;

	public LineRenderer lineDot;

	private Vector2 pointStart;

	[Header("------ Hero ------")]
	public Hero hero;

	[Header("------ Platform ------")]
	public List<Platform> listPlatform;

	private int startSorting = 99;

	public Color colorStart;

	public Color[] colorStarts;

	public DataHolder dataHolder;

	public int currentRound;

	public Mission currentMission;

	public UIController ui_Controller;

	public Enemy enemyIns;

	public GameObject targetCoinPosition;

	public Animator helpGroup;

	[Header("______ num tool _____")]
	public ToolControl tool_healing;

	public ToolControl tool_FireArrow;

	public ToolControl tool_Protect;

	public ToolControl tool_X3;

	public GameObject rewardlevelPass;

	public int score;

	private int coin;

	public int levelTest;

	public GameObject rewardParrent;

	public Text txtReward;

	public Image imgReward;

	public Sprite spr_coin;

	[SerializeField]
	public E_AudioClip e_AudioClip;

	public AudioSource audioSource;

	public static PopupBase popupBase;

	public GameObject quitGamePopup;

	private ObstaclesBase obstaclesBase;

	public ObstaclesBase shaw;

	public ObstaclesBase wood;

	public ObstaclesBase rope;

	private void Awake()
	{
		hero.gameManager = this;
		is_camMove = false;
		is_gameStart = false;
		colorStart = colorStarts[Random.Range(0, colorStarts.Length)];
		AppsFlyerManager.InitAppsFlyer();
	}

	public void initTool()
	{
		tool_healing.onShow(dataHolder.gameData.numHealing);
		tool_FireArrow.onShow(dataHolder.gameData.numFireArrow);
		tool_Protect.onShow(dataHolder.gameData.numProtect);
		tool_X3.onShow(dataHolder.gameData.numX3);
	}

	private void Start()
	{
		coin = dataHolder.gameData.coin;
		addCoin(0);
		score = 0;
		ui_Controller.setScore(score);
		ui_Controller.setTextLevel(dataHolder.gameData.missionNum + 1);
		initTool();
		createPlatform();
		setUpArchery();
		hero.init();
		wellcomeGame();
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyUp(KeyCode.Escape) || UnityEngine.Input.GetKeyUp(KeyCode.B))
		{
			if (popupBase != null)
			{
				popupBase.onClose();
			}
			else if (ui_Controller.startGroup.activeSelf && !is_gameStart)
			{
				quitGamePopup.SetActive(value: true);
			}
		}
	}

	public void btn_QuitGame()
	{
		Application.Quit();
	}

	public void camVibrate()
	{
		camAnim.Play("cameraVir");
		if (dataHolder.gameData.is_vibrate)
		{
			// Handheld.Vibrate();
		}
	}

	public void wellcomeGame()
	{
		hero.transform.localScale = new Vector3(0.14f, 0.14f, 1f);
		hero.transform.position = new Vector3(-1f, 10f, 0f);
		iTween.MoveTo(hero.gameObject, iTween.Hash("position", new Vector3(0f, -1f, 0f), "time", 0.5f, "easetype", iTween.EaseType.linear));
		hero.playAnim("startGame");
		delayFunction(0.3f, delegate
		{
			hero.playAudio(hero.audio_start);
		});
		delayFunction(1f, delegate
		{
			is_camMove = true;
			ui_Controller.onStart();
		});
		if (!SoundManager.ins.bg_audioSource.isPlaying)
		{
			SoundManager.ins.bg_audioSource.Play();
		}
	}

	public void addScore(int num)
	{
		score += num;
		ui_Controller.setScore(score);
	}

	public void addCoin(int num)
	{
		coin += num;
		ui_Controller.setTextCoin(coin);
		if (num > 0)
		{
			dataHolder.gameData.numEarnGolds += num;
		}
	}

	public int getCoin()
	{
		return coin;
	}

	public void onStartGame()
	{
		is_gameStart = true;
		helpGroup.Play("HelpOpen");
		if (dataHolder.gameData.missionNum > dataHolder.missionData.missions.Length - 1)
		{
			currentMission = dataHolder.missionData.missions[dataHolder.missionData.missions.Length - 1];
		}
		else
		{
			currentMission = dataHolder.missionData.missions[dataHolder.gameData.missionNum];
		}
		ui_Controller.setTextLevel(dataHolder.gameData.missionNum + 1);
		currentRound = -1;
		nextRound();
	}

	private void readLevel(int level)
	{
		Mission mission = dataHolder.missionData.missions[level - 1];
		for (int i = 0; i < mission.enemyInfos.Length; i++)
		{
		}
	}

	public void instanceObstales()
	{
		if (currentMission.enemyInfos[currentRound].obstacles == Obstacles.shaw)
		{
			obstaclesBase = shaw;
			Transform parent = obstaclesBase.transform.parent;
			Vector3 position = hero.transform.position;
			parent.position = new Vector3(0f, position.y + 1f, 0f);
			obstaclesBase.transform.parent.gameObject.SetActive(value: true);
		}
		else if (currentMission.enemyInfos[currentRound].obstacles == Obstacles.wood)
		{
			obstaclesBase = wood;
			Transform parent2 = obstaclesBase.transform.parent;
			Vector3 position2 = hero.transform.position;
			parent2.position = new Vector3(0f, position2.y + 2f, 0f);
			obstaclesBase.transform.parent.gameObject.SetActive(value: true);
		}
		else if (currentMission.enemyInfos[currentRound].obstacles == Obstacles.rope)
		{
			obstaclesBase = rope;
			Transform parent3 = obstaclesBase.transform.parent;
			Vector3 position3 = hero.transform.position;
			parent3.position = new Vector3(0f, position3.y + 5f, 0f);
			obstaclesBase.transform.parent.gameObject.SetActive(value: true);
		}
	}

	public void nextRound()
	{
		if (obstaclesBase != null)
		{
			obstaclesBase.onDisbleObj();
			obstaclesBase = null;
		}
		currentRound++;
		if (currentRound <= currentMission.enemyInfos.Length - 1)
		{
			if (ObjectPooling.ins.getEnemy(currentMission.enemyInfos[currentRound]._name) != null)
			{
				enemyIns = ObjectPooling.ins.getEnemy(currentMission.enemyInfos[currentRound]._name);
				enemyIns.gameObject.SetActive(value: true);
				ObjectPooling.ins.removeEnemy(enemyIns);
			}
			else
			{
				string path = "Enemies/" + currentMission.enemyInfos[currentRound]._name;
				enemyIns = (Object.Instantiate(Resources.Load(path, typeof(Enemy))) as Enemy);
				enemyIns.name = currentMission.enemyInfos[currentRound]._name;
			}
			enemyIns.is_Boss = (currentRound == currentMission.enemyInfos.Length - 1);
			ui_Controller.fillHpBoss.transform.parent.gameObject.SetActive(currentRound == currentMission.enemyInfos.Length - 1);
			enemyIns.platformIdle = getNextPlatform(hero.platformIdle);
			enemyIns.gameManager = this;
			enemyIns.init();
			enemyIns.autoSetPosition(currentRound == currentMission.enemyInfos.Length - 1);
			hero.moveNextPlatform();
			int num = Random.Range(0, 100);
			if (currentRound == currentMission.enemyInfos.Length - 1)
			{
				if (num < 50)
				{
					int num2 = Random.Range(0, 100);
					bool flag = (num2 < 30) ? true : false;
					if (!flag)
					{
						Transform transform = ItemEarn.ins.parrent.transform;
						float x = Random.Range(-2.5f, 2.5f);
						Vector3 position = hero.transform.position;
						transform.position = new Vector3(x, position.y + Random.Range(4f, 6.5f));
					}
					else
					{
						Transform transform2 = ItemEarn.ins.parrent.transform;
						Vector3 position2 = hero.transform.position;
						transform2.position = new Vector3(0f, position2.y + Random.Range(4f, 6.5f));
					}
					ItemEarn.ins.onShow(Random.Range(0, 4), flag);
				}
			}
			else if (num < 15)
			{
				int num3 = Random.Range(0, 100);
				bool flag2 = (num3 < 70) ? true : false;
				if (!flag2)
				{
					Transform transform3 = ItemEarn.ins.parrent.transform;
					float x2 = Random.Range(-2.5f, 2.5f);
					Vector3 position3 = hero.transform.position;
					transform3.position = new Vector3(x2, position3.y + Random.Range(4f, 6.5f));
				}
				else
				{
					Transform transform4 = ItemEarn.ins.parrent.transform;
					Vector3 position4 = hero.transform.position;
					transform4.position = new Vector3(0f, position4.y + Random.Range(4f, 6.5f));
				}
				ItemEarn.ins.onShow(Random.Range(0, 4), flag2);
			}
		}
		else
		{
			UnityEngine.Debug.Log("_____________ WIN ________________");
			if (dataHolder.gameData.missionNum < dataHolder.missionData.missions.Length - 1)
			{
				dataHolder.gameData.missionNum++;
				dataHolder.gameData.writePre();
			}
			gameWin();
		}
	}

	public void createPlatform()
	{
		if (listPlatform.Count == 0)
		{
			Platform platform = ObjectPooling.ins.GetPlatform();
			platform.setNumPlatform(Random.Range(2, 4), -1f);
			platform.transform.position = new Vector3(0f, -1f, 0f);
			platform.transform.localScale = new Vector3(-1f, 1f, 1f);
			hero.platformIdle = platform;
			listPlatform.Add(platform);
		}
		while (true)
		{
			Vector3 position = listPlatform[listPlatform.Count - 1].transform.position;
			float y = position.y;
			Vector3 position2 = _camera.transform.position;
			if (y < position2.y + 7f)
			{
				Platform platform2 = ObjectPooling.ins.GetPlatform();
				listPlatform.Add(platform2);
				resetListPlatform();
				continue;
			}
			break;
		}
	}

	private void resetListPlatform()
	{
		listPlatform[0].setColor(colorStart);
		listPlatform[0].setSortingOder(startSorting);
		for (int i = 1; i < listPlatform.Count; i++)
		{
			Transform transform = listPlatform[i].transform;
			Vector3 position = listPlatform[i - 1].transform.position;
			transform.position = new Vector3(0f, position.y + (float)listPlatform[i - 1].getNum() * 0.4f, 0f);
			listPlatform[i].setColor(getColorNextPlatform(listPlatform[i - 1].getColor()));
			listPlatform[i].setSortingOder(listPlatform[i - 1].getSortingOder() - 1);
			Transform transform2 = listPlatform[i].transform;
			Vector3 localScale = listPlatform[i - 1].transform.localScale;
			transform2.localScale = new Vector3(localScale.x * -1f, 1f, 1f);
		}
	}

	private Color getColorNextPlatform(Color color)
	{
		Color white = Color.white;
		white.r = color.r * 0.8f;
		white.b = color.b * 0.8f;
		white.g = color.g * 0.8f;
		return white;
	}

	public int getNumOfCurentPlatform(Platform platform)
	{
		return listPlatform[listPlatform.IndexOf(platform)].getNum();
	}

	public Platform getNextPlatform(Platform platform)
	{
		return listPlatform[listPlatform.IndexOf(platform) + 1];
	}

	public Platform getPreviousPlatform(Platform platform)
	{
		if (listPlatform.IndexOf(platform) - 2 >= 0 && listPlatform[listPlatform.IndexOf(platform) - 2] != null)
		{
			return listPlatform[listPlatform.IndexOf(platform) - 2];
		}
		if (listPlatform.IndexOf(platform) - 1 >= 0 && listPlatform[listPlatform.IndexOf(platform) - 1] != null)
		{
			return listPlatform[listPlatform.IndexOf(platform) - 1];
		}
		return listPlatform[listPlatform.IndexOf(platform)];
	}

	public void updateListPlatform()
	{
		float posYTop = listPlatform[0].getPosYTop();
		Vector3 position = _camera.transform.position;
		if (posYTop < position.y - 5f)
		{
			ObjectPooling.ins.addPlatform(listPlatform[0]);
			listPlatform.RemoveAt(0);
			resetListPlatform();
		}
		Vector3 position2 = listPlatform[listPlatform.Count - 1].transform.position;
		float y = position2.y;
		Vector3 position3 = _camera.transform.position;
		if (y < position3.y + 10f)
		{
			Platform platform = ObjectPooling.ins.GetPlatform();
			listPlatform.Add(platform);
			resetListPlatform();
		}
	}

	public void onClickDown()
	{
		if (hero.canAttack && is_gameStart && !hero.is_die)
		{
			pointStart = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			dotStart.transform.position = pointStart;
			dotEnd.transform.position = pointStart;
			dotStart.SetActive(value: true);
			dotEnd.SetActive(value: true);
			Vector3[] positions = new Vector3[2]
			{
				pointStart,
				pointStart
			};
			lineDot.SetPositions(positions);
			lineDot.gameObject.SetActive(value: true);
			hero.archeryDown();
		}
	}

	public void onClickDrag()
	{
		if (hero.canAttack && is_gameStart && dotStart.activeSelf)
		{
			Vector2 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			dotEnd.transform.position = vector;
			Vector3[] positions = new Vector3[2]
			{
				pointStart,
				vector
			};
			lineDot.SetPositions(positions);
			float angle = Vector2.Angle(vector - pointStart, Vector2.up);
			float num = Vector2.Distance(vector, pointStart);
			hero.archeryUp(num * 2f, angle);
		}
	}

	public void onClickUp()
	{
		if (hero.canAttack && is_gameStart)
		{
			dotStart.SetActive(value: false);
			dotEnd.SetActive(value: false);
			lineDot.gameObject.SetActive(value: false);
			hero.attack();
		}
	}

	public void disableActiveDot()
	{
		dotStart.SetActive(value: false);
		dotEnd.SetActive(value: false);
		lineDot.gameObject.SetActive(value: false);
	}

	public void gameLose()
	{
		is_gameStart = false;
		helpGroup.Play("HelpClose");
		if (dotStart.activeSelf)
		{
			dotStart.SetActive(value: false);
			dotEnd.SetActive(value: false);
			lineDot.gameObject.SetActive(value: false);
		}

		dataHolder.gameData.numHealing = 0; // reset num healing
		dataHolder.gameData.numFireArrow = 0; // reset num fire arrow
		dataHolder.gameData.numProtect = 0; // reset num fire arrow
		dataHolder.gameData.numX3 = 0; // reset num fire arrow

		saveData();
		PlayfabManager.SendLeaderboard(score);
		delayFunction(3f, delegate
		{
			ui_Controller.btn_cancelContinue();
		});
		hero.OnDisableProtect();
		SoundManager.ins.bg_audioSource.Stop();
		FireBaseAnalyticControl.ins.AnalyticLevelFailed(dataHolder.gameData.missionNum + 1);
	}

	public void onShowDialogGameLose()
	{
		ui_Controller.gameResultOnShow(isWin: false, coin, score, dataHolder.gameData.highScore);
		playAudio(e_AudioClip.gameLose);
	}

	public void saveData()
	{
		dataHolder.gameData.coin = coin;
		if (score > dataHolder.gameData.highScore)
		{
			dataHolder.gameData.highScore = score;
		}
		dataHolder.gameData.writePre();
	}

	public void gameWin()
	{
		is_gameStart = false;
		helpGroup.Play("HelpClose");
		if (dotStart.activeSelf)
		{
			dotStart.SetActive(value: false);
			dotEnd.SetActive(value: false);
			lineDot.gameObject.SetActive(value: false);
		}
		saveData();
		delayFunction(3f, delegate
		{
			ui_Controller.gameResultOnShow(isWin: true, coin, score, dataHolder.gameData.highScore);
			playAudio(e_AudioClip.gameWin);
		});
		delayFunction(4.5f, delegate
		{
			rewardlevelPass.SetActive(value: false);
		});
		hero.OnDisableProtect();
		SoundManager.ins.bg_audioSource.Stop();
		FireBaseAnalyticControl.ins.AnalyticLevelPassed(dataHolder.gameData.missionNum + 1);
	}

	public void replay()
	{
		if (enemyIns.gameObject.activeSelf)
		{
			enemyIns.gameObject.GetComponent<Enemy>().disableObj();
		}
		score = 0;
		ui_Controller.setScore(score);
		hero.init();
		setUpArchery();
		ui_Controller.setFadePanel();
		wellcomeGame();
		is_camMove = false;
		_camera.transform.position = new Vector3(0f, 0f, -10f);
		hero.platformIdle = listPlatform[0];
		listPlatform[0].transform.position = new Vector3(0f, -1f, 0f);
		listPlatform[0].transform.localScale = new Vector3(-1f, 1f, 1f);
		resetListPlatform();
		delayFunction(1.5f, delegate
		{
			onStartGame();
		});
		if (obstaclesBase != null)
		{
			obstaclesBase.onDisbleObj();
			obstaclesBase = null;
		}
	}

	public void btn_home()
	{
		if (enemyIns.gameObject.activeSelf)
		{
			enemyIns.gameObject.GetComponent<Enemy>().disableObj();
		}
		score = 0;
		ui_Controller.setScore(score);
		hero.init();
		setUpArchery();
		ui_Controller.setFadePanel();
		wellcomeGame();
		is_camMove = false;
		_camera.transform.position = new Vector3(0f, 0f, -10f);
		hero.platformIdle = listPlatform[0];
		listPlatform[0].transform.position = new Vector3(0f, -1f, 0f);
		listPlatform[0].transform.localScale = new Vector3(-1f, 1f, 1f);
		resetListPlatform();
		ui_Controller.infoGroup.SetActive(value: false);
		delayFunction(1.2f, delegate
		{
			ui_Controller.startGroup.SetActive(value: true);
			ui_Controller.setotherCamrera(CameraMove.cameraOrtho, 3f, 1f);
		});
		ui_Controller.setTextLevel(dataHolder.gameData.missionNum + 1);
		if (obstaclesBase != null)
		{
			obstaclesBase.onDisbleObj();
			obstaclesBase = null;
		}
	}

	public void NextLevel()
	{
		if (enemyIns.gameObject.activeSelf)
		{
			enemyIns.gameObject.GetComponent<Enemy>().disableObj();
		}
		ui_Controller.setFadePanel();
		wellcomeGame();
		is_camMove = false;
		_camera.transform.position = new Vector3(0f, 0f, -10f);
		hero.platformIdle = listPlatform[0];
		listPlatform[0].transform.position = new Vector3(0f, -1f, 0f);
		listPlatform[0].transform.localScale = new Vector3(-1f, 1f, 1f);
		resetListPlatform();
		ui_Controller.infoGroup.SetActive(value: false);
		delayFunction(1.2f, delegate
		{
			ui_Controller.startGroup.SetActive(value: true);
			ui_Controller.setotherCamrera(CameraMove.cameraOrtho, 3f, 1f);
		});
		ui_Controller.setTextLevel(dataHolder.gameData.missionNum + 1);
	}

	public void onWatchVideoToContinue()
	{
        VideoRewardController.onShowVideoReward(delegate
        {
            btn_Continue();
        }, delegate
        {
            onWatchVideoFail();
        });
    }

    private void onWatchVideoFail()
	{
		NotificationPopup.ins.onShow("Video not ready. \n Please comeback later! ", delegate
		{
			onShowDialogGameLose();
		});
		FireBaseAnalyticControl.ins.AnalyticWatchVideoReward_notReady();
	}

	public void btn_Continue()
	{
		is_gameStart = true;
		is_camMove = true;
		helpGroup.Play("HelpOpen");
		ui_Controller.gameResult.SetActive(value: false);
		hero.init();
		enemyIns.reAttack();
		hero.body_animator.Play("startGame");
		hero.onShowEffContinue();
		dataHolder.gameData.numWatchVideo++;
		saveData();
	}

	public void setSpriteArchery(int id)
	{
		hero.archerySpr.sprite = dataHolder.getArcherySpr(id);
		hero.updateSprArrow(id);
	}

	public void setUpArchery()
	{
		if (dataHolder.archeryData.GetArcheryD(dataHolder.gameData.ArcheryId).bought)
		{
			hero.archerySpr.sprite = dataHolder.getArcherySpr(dataHolder.gameData.ArcheryId);
			hero.updateDefenseArchery();
		}
		else
		{
			hero.archerySpr.sprite = dataHolder.archeryAvatar[0];
			dataHolder.gameData.ArcheryId = 0;
			dataHolder.gameData.writePre();
		}
		hero.idArchery = dataHolder.gameData.ArcheryId;
		hero.updateSprArrow();
	}

	public void btnOnEnableProtect()
	{
		if (dataHolder.gameData.numProtect >= 1)
		{
			dataHolder.gameData.numProtect--;
			initTool();
			hero.OnEnableProtect();
		}
		else if (score >= 1000)
		{
			addScore(-1000); // 1000 score for the protect supply
			initTool();
			hero.OnEnableProtect();
		}
		SoundManager.ins.play_audioClick();
	}

	public void btn_x3Arrow()
	{
		if (!hero.is_die && is_gameStart && hero.body_animator.enabled && hero.is_idle)
		{
			if (dataHolder.gameData.numX3 >= 1)
			{
				dataHolder.gameData.numX3--;
				initTool();
				hero.x3Arrow();
			}
			else if (score >= 500)
			{
				addScore(-500); // 500 score for the x3 arrow supply
				initTool();
				hero.x3Arrow();
			}
		}
		SoundManager.ins.play_audioClick();
	}

	public void btn_healing()
	{
		if (!hero.is_die && is_gameStart && hero.body_animator.enabled && hero.HP_current < (float)hero.HP && hero.is_idle)
		{
			if (dataHolder.gameData.numHealing >= 1)
			{
				dataHolder.gameData.numHealing--;
				initTool();
				hero.healing();
			}
			else if (score >= 3000)
			{
				addScore(-3000); // 3000 score for the healing supply
				initTool();
				hero.healing();
			}
		}
		SoundManager.ins.play_audioClick();
	}

	public void btn_FireArrow()
	{
		if (dataHolder.gameData.numFireArrow >= 1)
		{
			dataHolder.gameData.numFireArrow--;
			initTool();
			hero.addFireArrow();
		}
		else if (score >= 500)
		{
			addScore(-500); // 500 score for the fire arrow supply
			initTool();
			hero.addFireArrow();
		}
		SoundManager.ins.play_audioClick();
	}

	public void onWatchVideoGetCoin()
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
		dataHolder.gameData.numWatchVideo++;
		addCoin(100);
		saveData();
		showRewardCoin(100);
		FireBaseAnalyticControl.ins.AnalyticWatchVideoReward("Watch_Video_End_Game");
	}

	private void watchVideoFail()
	{
		NotificationPopup.ins.onShow("Video not ready. \n Please comeback later! ");
		FireBaseAnalyticControl.ins.AnalyticWatchVideoReward_notReady();
	}

	public void showRewardCoin(int num)
	{
		imgReward.sprite = spr_coin;
		txtReward.text = num + string.Empty;
		rewardParrent.SetActive(value: true);
	}

	public void playAudio(AudioClip audioClip)
	{
		audioSource.clip = audioClip;
		audioSource.volume = dataHolder.gameData.soundVolume;
		audioSource.Play();
	}
}
