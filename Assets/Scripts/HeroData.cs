using UnityEngine;

[CreateAssetMenu(fileName = "HeroData", menuName = "HeroData")]
public class HeroData : E_ScriptableObject
{
	public HeroPoints[] heroPoints;

	public override void init()
	{
		namePre = GetType().ToString() + GetHashCode();
		if (PlayerPrefs.GetString(namePre) == null || PlayerPrefs.GetString(namePre).Equals(string.Empty))
		{
			heroPoints[0].isUnlock = true;
			writePre();
		}
		else
		{
			readAndInsert();
		}
	}
}
