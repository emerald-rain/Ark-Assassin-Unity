using System;

[Serializable]
public class Achievement
{
	public string nameArchie;

	public string description;

	public Target target;

	public int current;

	public AchievementInfo[] achievementInfos;
}
