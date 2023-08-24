using UnityEngine;

public class E_ScriptableObject : ScriptableObject
{
	public string namePre;

	public virtual void init()
	{
		if (PlayerPrefs.GetString(namePre) == null || PlayerPrefs.GetString(namePre).Equals(string.Empty))
		{
			writePre();
		}
		else
		{
			readAndInsert();
		}
	}

	public void writePre()
	{
		string value = JsonUtility.ToJson(this);
		PlayerPrefs.SetString(namePre, value);
	}

	public void readAndInsert()
	{
		JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(namePre), this);
	}
}
