using UnityEngine;
using UnityEngine.UI;

public class AchieCell : MonoBehaviour
{
	public Text txtName;

	public Text txtDescription;

	public Text txtProgress;

	public Image imgSlider;

	public Button btnGet;

	public Button btnDone;

	public Achievement achievement;

	public RectTransform[] divisions;

	public int index;

	private float witdhPx = 433.5f;

	public Image img_btnGet;

	public Text txt_btnGet;

	public Sprite spr_coin;

	public Image img_giftSuccess;

	public Text txt_gifSuccess;

	public DataHolder dataHolder;

	public GameManager gameManager;

	public void init()
	{
		if (achievement.target == Target.Coin)
		{
			achievement.current = dataHolder.gameData.numEarnGolds;
		}
		else if (achievement.target == Target.EnemyKill)
		{
			achievement.current = dataHolder.gameData.numEnemyKill;
		}
		else if (achievement.target == Target.BossKill)
		{
			achievement.current = dataHolder.gameData.numKillBoss;
		}
		else if (achievement.target == Target.WatchVideo)
		{
			achievement.current = dataHolder.gameData.numWatchVideo;
		}
		float num = witdhPx / (float)achievement.achievementInfos[achievement.achievementInfos.Length - 1].valueTargets;
		for (int i = 0; i < divisions.Length; i++)
		{
			divisions[i].localPosition = new Vector3((float)achievement.achievementInfos[i].valueTargets * num - 216.75f, 0f, 0f);
		}
		txtName.text = achievement.nameArchie;
		txtDescription.text = achievement.description;
		int levelAchie = getLevelAchie();
		if (levelAchie > 0 && levelAchie <= achievement.achievementInfos.Length)
		{
			if (achievement.achievementInfos[levelAchie - 1].statusAchis == StatusAchi.Done)
			{
				if (levelAchie == achievement.achievementInfos.Length)
				{
					onShow(levelAchie - 1);
					btnDone.gameObject.SetActive(value: true);
					btnGet.gameObject.SetActive(value: false);
				}
				else
				{
					onShow(levelAchie);
				}
				return;
			}
			int num2 = 0;
			while (true)
			{
				if (num2 < levelAchie)
				{
					if (achievement.achievementInfos[num2].statusAchis == StatusAchi.Process)
					{
						achievement.achievementInfos[num2].statusAchis = StatusAchi.GetGift;
						onShow(num2);
						return;
					}
					if (achievement.achievementInfos[num2].statusAchis == StatusAchi.GetGift)
					{
						break;
					}
					num2++;
					continue;
				}
				return;
			}
			onShow(num2);
		}
		else
		{
			onShow(0);
		}
	}

	private void onShow(int index)
	{
		this.index = index;
		float num = (float)achievement.current / (float)achievement.achievementInfos[index].valueTargets;
		btnDone.gameObject.SetActive(value: false);
		btnGet.gameObject.SetActive(value: true);
		if (num >= 1f)
		{
			btnGet.interactable = true;
			num = 1f;
		}
		else
		{
			btnGet.interactable = false;
		}
		txtProgress.text = achievement.current + "/" + achievement.achievementInfos[index].valueTargets;
		imgSlider.fillAmount = (float)achievement.current / (float)achievement.achievementInfos[achievement.achievementInfos.Length - 1].valueTargets;
		if (achievement.achievementInfos[index].giftAchis == GiftAchi.Coin)
		{
			img_btnGet.sprite = spr_coin;
			txt_btnGet.text = achievement.achievementInfos[index].giftValue + string.Empty;
		}
	}

	private int getLevelAchie()
	{
		for (int i = 0; i < achievement.achievementInfos.Length; i++)
		{
			if (achievement.current < achievement.achievementInfos[i].valueTargets)
			{
				return i;
			}
		}
		return achievement.achievementInfos.Length;
	}

	public void btnOnGet()
	{
		achievement.achievementInfos[index].statusAchis = StatusAchi.Done;
		onShowGiftSuccuss();
		init();
		SoundManager.ins.play_audioClick();
	}

	private void onShowGiftSuccuss()
	{
		img_giftSuccess.sprite = img_btnGet.sprite;
		txt_gifSuccess.text = txt_btnGet.text;
		img_giftSuccess.transform.parent.parent.parent.gameObject.SetActive(value: true);
		if (achievement.achievementInfos[index].giftAchis == GiftAchi.Coin)
		{
			gameManager.addCoin(achievement.achievementInfos[index].giftValue);
			gameManager.saveData();
			dataHolder.achievementData.writePre();
		}
	}
}
