using UnityEngine;

public class DataHolder : MonoBehaviour
{
	public HeroData heroData;

	public MissionData missionData;

	public GameData gameData;

	public ArcheryData archeryData;

	public Sprite[] archeryAvatar;

	public AchievementData achievementData;

	public DailyGiftData dailyGiftData;

	private void Awake()
	{
		gameData.init();
		archeryData.init();
		dailyGiftData.init();
		achievementData.init();
		heroData.init();
	}

	public Sprite getArcherySpr(int id)
	{
		return Resources.Load<Sprite>("Archery/Spr/archery_" + id);
	}
}
