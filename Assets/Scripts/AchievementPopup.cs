using System.Collections.Generic;
using UnityEngine;

public class AchievementPopup : PopupBase
{
	public AchieCell achieCell;

	public DataHolder dataHolder;

	public Transform achieParrent;

	public List<AchieCell> listsAchie;

	private void Awake()
	{
		onCreate();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		loadAchie();
	}

	private void onCreate()
	{
		AchievementData achievementData = dataHolder.achievementData;
		for (int i = 0; i < achievementData.achievements.Length; i++)
		{
			AchieCell achieCell = Object.Instantiate(this.achieCell);
			achieCell.transform.SetParent(achieParrent, worldPositionStays: false);
			achieCell.achievement = achievementData.achievements[i];
			listsAchie.Add(achieCell);
		}
	}

	private void loadAchie()
	{
		for (int i = 0; i < listsAchie.Count; i++)
		{
			listsAchie[i].init();
		}
	}
}
