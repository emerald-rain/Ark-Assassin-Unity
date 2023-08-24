using UnityEngine;

[CreateAssetMenu(fileName = "DailyGiftData", menuName = "DailyGiftDatas")]
public class DailyGiftData : E_ScriptableObject
{
	public GiftDay[] giftDays;

	public void reset()
	{
		for (int i = 0; i < giftDays.Length; i++)
		{
			giftDays[i].typeOfDaily = TypeOfDaily.Wait;
		}
		writePre();
	}
}
