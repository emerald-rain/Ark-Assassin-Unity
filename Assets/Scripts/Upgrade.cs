using System;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : PopupBase
{
	public GameObject[] heroParrent;

	private Vector3[] posHero = new Vector3[3]
	{
		new Vector3(-1.2f, 2f, 0f),
		new Vector3(0f, 1f, 0f),
		new Vector3(1.2f, 2f, 0f)
	};

	public DataHolder dataHolder;

	public Text txtButtonUpgrade;

	public Text txtButtonUpgrade_center;

	private HeroPoints heroInfo;

	public Image slider_attack;

	public Image slider_defense;

	public Image slider_hp;

	public Text txtPoint_attack;

	public Text txtPoint_defense;

	public Text txtPoint_hp;

	public Text add_attack;

	public Text add_defense;

	public Text add_hp;

	public GameObject imgLock;

	public GameObject imgEquiped;

	[Header("_____ notify _____")]
	public Animator notifyUpgrade;

	public Text txtNotifyUpgrade;

	public Button btnOkNotifyUpgrade;

	public Button btnCancelNotifyUpgrade;

	public GameManager gameManager;

	public GameObject[] toc_songoku;

	public GameObject[] toc_vegeta;

	public GameObject effectUnlockHero;

	public GameObject effectUpgrade;

	public Text txtValueUpgrade;

	private bool updateting;

	public AudioSource audioSource;

	public AudioClip aure;

	public AudioClip unlockSuccess;

	private void Start()
	{
		for (int i = 0; i < heroParrent.Length; i++)
		{
			heroParrent[i].transform.position = posHero[i];
			if (i == 1)
			{
				iTween.ScaleTo(heroParrent[i], new Vector3(35f, 35f, 35f), 0f);
				iTween.ColorTo(heroParrent[i], Color.white, 0f);
			}
			else
			{
				iTween.ScaleTo(heroParrent[i], new Vector3(20f, 20f, 20f), 0f);
				iTween.ColorTo(heroParrent[i], Color.gray, 0f);
			}
		}
		heroInfo = getinfo(heroParrent[1].name.ToLower());
		init();
	}

	private HeroPoints getinfo(string _name)
	{
		for (int i = 0; i < dataHolder.heroData.heroPoints.Length; i++)
		{
			if (_name.ToLower().Equals(dataHolder.heroData.heroPoints[i].nameHero.ToLower()))
			{
				return dataHolder.heroData.heroPoints[i];
			}
		}
		return null;
	}

	private void init()
	{
		if (heroInfo != null)
		{
			if (dataHolder.gameData.heroEquiped.ToLower().Equals(heroParrent[1].name.ToLower()))
			{
				imgEquiped.SetActive(value: true);
				txtButtonUpgrade.text = "UPGRADE";
				if (heroInfo.level < 9)
				{
					txtValueUpgrade.text = heroInfo.points[heroInfo.level + 1].coinUpgrade + string.Empty;
				}
				imgLock.SetActive(value: false);
				txtButtonUpgrade.gameObject.SetActive(value: true);
				txtValueUpgrade.gameObject.SetActive(value: true);
				txtButtonUpgrade_center.gameObject.SetActive(value: false);
			}
			else if (heroInfo.isUnlock)
			{
				txtButtonUpgrade.text = "EQUIP";
				txtButtonUpgrade_center.text = "EQUIP";
				imgLock.SetActive(value: false);
				imgEquiped.SetActive(value: false);
				txtButtonUpgrade.gameObject.SetActive(value: false);
				txtValueUpgrade.gameObject.SetActive(value: false);
				txtButtonUpgrade_center.gameObject.SetActive(value: true);
			}
			else
			{
				imgEquiped.SetActive(value: false);
				txtButtonUpgrade.text = "UNLOCK";
				txtButtonUpgrade_center.text = "UNLOCK";
				txtValueUpgrade.text = heroInfo.valueHero + string.Empty;
				imgLock.SetActive(value: true);
				txtButtonUpgrade.gameObject.SetActive(value: false);
				txtValueUpgrade.gameObject.SetActive(value: false);
				txtButtonUpgrade_center.gameObject.SetActive(value: true);
			}
			slider_attack.fillAmount = (float)heroInfo.points[heroInfo.level].dame / (float)heroInfo.points[heroInfo.points.Length - 1].dame;
			slider_defense.fillAmount = (float)heroInfo.points[heroInfo.level].defense / (float)heroInfo.points[heroInfo.points.Length - 1].defense;
			slider_hp.fillAmount = (float)heroInfo.points[heroInfo.level].hp / (float)heroInfo.points[heroInfo.points.Length - 1].hp;
			txtPoint_attack.text = heroInfo.points[heroInfo.level].dame + "/" + heroInfo.points[heroInfo.points.Length - 1].dame;
			txtPoint_defense.text = heroInfo.points[heroInfo.level].defense + "/" + heroInfo.points[heroInfo.points.Length - 1].defense;
			txtPoint_hp.text = heroInfo.points[heroInfo.level].hp + "/" + heroInfo.points[heroInfo.points.Length - 1].hp;
			if (heroInfo.level < heroInfo.points.Length - 1)
			{
				add_attack.text = "+" + (heroInfo.points[heroInfo.level + 1].dame - heroInfo.points[heroInfo.level].dame);
				add_defense.text = "+" + (heroInfo.points[heroInfo.level + 1].defense - heroInfo.points[heroInfo.level].defense);
				add_hp.text = "+" + (heroInfo.points[heroInfo.level + 1].hp - heroInfo.points[heroInfo.level].hp);
			}
			else
			{
				add_attack.text = string.Empty;
				add_defense.text = string.Empty;
				add_hp.text = string.Empty;
				if (dataHolder.gameData.heroEquiped.ToLower().Equals(heroInfo.nameHero.ToLower()))
				{
					txtButtonUpgrade.text = "MAX";
					txtButtonUpgrade_center.text = "MAX";
				}
				else
				{
					txtButtonUpgrade.text = "EQUIP";
					txtButtonUpgrade_center.text = "EQUIP";
				}
				txtButtonUpgrade.gameObject.SetActive(value: false);
				txtValueUpgrade.gameObject.SetActive(value: false);
				txtButtonUpgrade_center.gameObject.SetActive(value: true);
			}
		}
		else
		{
			txtButtonUpgrade.text = "COMMING SOON";
			imgLock.SetActive(value: true);
			imgEquiped.SetActive(value: false);
			txtButtonUpgrade_center.text = "COMMING SOON";
			txtButtonUpgrade.gameObject.SetActive(value: false);
			txtValueUpgrade.gameObject.SetActive(value: false);
			txtButtonUpgrade_center.gameObject.SetActive(value: true);
		}
		updateToc();
	}

	private void updateToc()
	{
		for (int i = 0; i < toc_songoku.Length; i++)
		{
			if (toc_songoku[i].activeSelf)
			{
				toc_songoku[i].SetActive(value: false);
			}
		}
		toc_songoku[dataHolder.heroData.heroPoints[0].level].SetActive(value: true);
		for (int j = 0; j < toc_vegeta.Length; j++)
		{
			if (toc_vegeta[j].activeSelf)
			{
				toc_vegeta[j].SetActive(value: false);
			}
		}
		toc_vegeta[dataHolder.heroData.heroPoints[1].level].SetActive(value: true);
	}

	public void btn_onLeft()
	{
		GameObject gameObject = heroParrent[0];
		heroParrent[0] = heroParrent[1];
		heroParrent[1] = heroParrent[2];
		heroParrent[2] = gameObject;
		onMoveObj();
		SoundManager.ins.play_audioClick();
	}

	public void btn_onRight()
	{
		GameObject gameObject = heroParrent[2];
		heroParrent[2] = heroParrent[1];
		heroParrent[1] = heroParrent[0];
		heroParrent[0] = gameObject;
		onMoveObj();
		SoundManager.ins.play_audioClick();
	}

	public void btn_Status()
	{
	}

	public void btn_Upgrade()
	{
		if (txtButtonUpgrade.text.ToLower().Equals("equip"))
		{
			dataHolder.gameData.heroEquiped = heroInfo.nameHero;
			dataHolder.gameData.writePre();
			init();
			gameManager.hero.requestDataHero();
			gameManager.hero.requestSpriteHero();
		}
		else if (txtButtonUpgrade.text.ToLower().Equals("upgrade"))
		{
			onUpgrade();
		}
		else if (txtButtonUpgrade.text.ToLower().Equals("unlock"))
		{
			onUnlockHero();
		}
		SoundManager.ins.play_audioClick();
	}

	private void onUpgrade()
	{
		if (!updateting && heroInfo.level < 9)
		{
			int coinUpgrade = heroInfo.points[heroInfo.level + 1].coinUpgrade;
			notifyUpgrade.gameObject.SetActive(value: false);
			if (gameManager.getCoin() >= coinUpgrade)
			{
				updateting = true;
				gameManager.addCoin(-coinUpgrade);
				gameManager.saveData();
				heroInfo.level++;
				dataHolder.heroData.writePre();
				gameManager.hero.requestDataHero();
				gameManager.hero.requestSpriteHero();
				effectUpgrade.SetActive(value: true);
				delayFunction(2f, delegate
				{
					effectUpgrade.SetActive(value: false);
					updateting = false;
					audioSource.Stop();
				});
				heroParrent[1].transform.GetChild(0).GetComponent<Animator>().Play("HP+");
				delayFunction(1f, delegate
				{
					init();
				});
				playAudio(aure);
				FireBaseAnalyticControl.ins.AnalyticUpgradeHero(heroInfo.nameHero, heroInfo.level + 1);
			}
			else
			{
				onShowNotify("Sorry, not enough coins.");
			}
		}
	}

	private void openShopCoin()
	{
		print("tried to open shop coin");
	}

	private void onUnlockHero()
	{
		notifyUpgrade.gameObject.SetActive(value: false);
		if (gameManager.getCoin() >= heroInfo.valueHero)
		{
			gameManager.addCoin(-heroInfo.valueHero);
			gameManager.saveData();
			heroInfo.isUnlock = true;
			dataHolder.heroData.writePre();
			init();
			effectUnlockHero.SetActive(value: true);
			delayFunction(5f, delegate
			{
				effectUnlockHero.SetActive(value: false);
			});
			onShowNotify("congratulations! \n Unlock hero success!!!");
			playAudio(unlockSuccess);
		}
		else
		{
			onShowNotify("Not enough coins to unlock this character.");
		}
	}

	private void onMoveObj()
	{
		for (int i = 0; i < heroParrent.Length; i++)
		{
			iTween.MoveTo(heroParrent[i], posHero[i], 1f);
			if (i == 1)
			{
				iTween.ScaleTo(heroParrent[i], new Vector3(35f, 35f, 35f), 1f);
				iTween.ColorTo(heroParrent[i], Color.white, 0.5f);
			}
			else
			{
				iTween.ScaleTo(heroParrent[i], new Vector3(20f, 20f, 20f), 1f);
				iTween.ColorTo(heroParrent[i], Color.gray, 0.5f);
			}
		}
		heroInfo = getinfo(heroParrent[1].name.ToLower());
		init();
	}

	public void onShowNotify(string content, Action action)
	{
		btnCancelNotifyUpgrade.gameObject.SetActive(value: true);
		txtNotifyUpgrade.text = content;
		btnOkNotifyUpgrade.onClick.RemoveAllListeners();
		btnOkNotifyUpgrade.onClick.AddListener(delegate
		{
			action();
		});
		btnOkNotifyUpgrade.onClick.AddListener(delegate
		{
			SoundManager.ins.play_audioClick();
		});
		notifyUpgrade.gameObject.SetActive(value: true);
	}

	public void onShowNotify(string content)
	{
		onShowNotify(content, delegate
		{
			onCloseNotification();
		});
		btnCancelNotifyUpgrade.gameObject.SetActive(value: false);
	}

	public void onCloseNotification()
	{
		notifyUpgrade.gameObject.SetActive(value: false);
		SoundManager.ins.play_audioClick();
	}

	private void playAudio(AudioClip audioClip)
	{
		audioSource.volume = SoundManager.ins.volumeSound;
		audioSource.clip = audioClip;
		audioSource.Play();
	}
}
