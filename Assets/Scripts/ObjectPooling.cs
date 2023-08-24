using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
	public static ObjectPooling ins;

	[Header("----- Arrow Pooling -----")]
	public Arrow arrowPrefabs;

	public List<Arrow> arrow_idle;

	public List<Arrow> arrow_play;

	public List<Arrow> arrow_hit;

	[Header("----- Platform Pooling -----")]
	public Platform platformPrefabs;

	public List<Platform> listPlatform;

	[Header("----- Enemies Pooling -----")]
	public Person enemy;

	public List<Person> listEnemy;

	[Header("----- Effect Headshot -----")]
	public GameObject effHeadshotPrefabs;

	public List<GameObject> listEffHeadshot;

	[Header("----- Coin -----")]
	public Coin coinPrefabs;

	public List<Coin> listCoin;

	[Header("----- Person -----")]
	public List<Enemy> listPeron;

	[Header("----- Effect Health ------")]
	public GameObject effHealth;

	public List<GameObject> listEffHealth;

	[Header("----- Effect num ------")]
	public NumberSpr effNumPrefabs;

	public List<NumberSpr> listEffnum;

	public Sprite[] arrow_sprs;

	private void Awake()
	{
		ins = this;
	}

	public Arrow getArrow(int id)
	{
		Arrow arrow;
		if (arrow_idle.Count > 0)
		{
			arrow = arrow_idle[0];
			arrow_idle.RemoveAt(0);
			arrow_play.Add(arrow);
		}
		else
		{
			arrow = UnityEngine.Object.Instantiate(arrowPrefabs);
			arrow_play.Add(arrow);
		}
		arrow.spr.sprite = arrow_sprs[id];
		arrow.gameObject.SetActive(value: true);
		arrow.onSetup(id);
		return arrow;
	}

	public void resetArrow(Arrow arrow)
	{
		arrow_play.Remove(arrow);
		arrow_idle.Add(arrow);
		arrow.transform.SetParent(null);
		arrow.gameObject.SetActive(value: false);
	}

	public Platform GetPlatform()
	{
		if (listPlatform.Count > 0)
		{
			Platform platform = listPlatform[0];
			listPlatform.RemoveAt(0);
			platform.gameObject.SetActive(value: true);
			return platform;
		}
		return UnityEngine.Object.Instantiate(platformPrefabs);
	}

	public void addPlatform(Platform platform)
	{
		listPlatform.Add(platform);
		platform.gameObject.SetActive(value: false);
	}

	public GameObject getEffectHeadshot()
	{
		if (listEffHeadshot != null && listEffHeadshot.Count > 0)
		{
			for (int i = 0; i < listEffHeadshot.Count; i++)
			{
				if (!listEffHeadshot[i].activeSelf)
				{
					return listEffHeadshot[i];
				}
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(effHeadshotPrefabs);
		listEffHeadshot.Add(gameObject);
		return gameObject;
	}

	public Coin getCoin()
	{
		if (listCoin != null && listCoin.Count > 0)
		{
			for (int i = 0; i < listCoin.Count; i++)
			{
				if (!listCoin[i].gameObject.activeSelf)
				{
					return listCoin[i];
				}
			}
		}
		Coin coin = UnityEngine.Object.Instantiate(coinPrefabs);
		listCoin.Add(coin);
		return coin;
	}

	public Enemy getEnemy(string nameEnemy)
	{
		for (int i = 0; i < listPeron.Count; i++)
		{
			if (listPeron[i].name.Equals(nameEnemy))
			{
				return listPeron[i];
			}
		}
		return null;
	}

	public void addEnemy(Enemy person)
	{
		listPeron.Add(person);
	}

	public void removeEnemy(Enemy person)
	{
		listPeron.Remove(person);
	}

	public GameObject getEffHealth()
	{
		GameObject gameObject;
		if (listEffHealth.Count > 0)
		{
			gameObject = listEffHealth[0];
			listEffHealth.RemoveAt(0);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate(effHealth);
		}
		gameObject.SetActive(value: true);
		return gameObject;
	}

	public void addEffHealt(GameObject eff)
	{
		eff.transform.SetParent(null);
		listEffHealth.Add(eff);
	}

	public NumberSpr getEffNum()
	{
		NumberSpr result;
		if (listEffnum.Count > 0)
		{
			result = listEffnum[0];
			listEffnum.RemoveAt(0);
		}
		else
		{
			result = UnityEngine.Object.Instantiate(effNumPrefabs);
		}
		return result;
	}

	public void addEffNum(NumberSpr numberSpr)
	{
		listEffnum.Add(numberSpr);
	}
}
