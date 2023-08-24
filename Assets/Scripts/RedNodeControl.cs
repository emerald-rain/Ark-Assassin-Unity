using UnityEngine;

public class RedNodeControl : MonoBehaviour
{
	public GameObject redNode;

	public DataHolder dataHolder;

	private void OnEnable()
	{
		redNode.SetActive(isActive());
	}

	private bool isActive()
	{
		for (int i = 0; i < dataHolder.achievementData.achievements.Length; i++)
		{
			for (int j = 0; j < dataHolder.achievementData.achievements[i].achievementInfos.Length; j++)
			{
				if (dataHolder.achievementData.achievements[i].achievementInfos[j].statusAchis == StatusAchi.GetGift)
				{
					return true;
				}
			}
		}
		return false;
	}
}
