using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlatforms : MonoBehaviour
{
    public Hero hero;
    public GameObject _camera;
    public List<Platform> listPlatform;
	private int startSorting = 99;
	public Color colorStart;
	public Color[] colorStarts;


    public void Start() {
        createPlatform();
    }
    
	public void createPlatform()
	{
		if (listPlatform.Count == 0)
        {
            Platform platform = ObjectPooling.ins.GetPlatform();
            platform.setNumPlatform(Random.Range(2, 4), -1f);
            platform.transform.position = new Vector3(0f, -1f, 0f);
            platform.transform.localScale = new Vector3(-1f, 1f, 1f);

            colorStart = colorStarts[Random.Range(0, colorStarts.Length)];

            hero.platformIdle = platform;
            listPlatform.Add(platform);
        }
		while (true)
		{
			Vector3 position = listPlatform[listPlatform.Count - 1].transform.position;
			float y = position.y;
			Vector3 position2 = _camera.transform.position;
			if (y < position2.y + 7f)
			{
				Platform platform2 = ObjectPooling.ins.GetPlatform();
				listPlatform.Add(platform2);
				resetListPlatform();
				continue;
			}
			break;
		}
	}

	private void resetListPlatform()
	{
		listPlatform[0].setColor(colorStart);
		listPlatform[0].setSortingOder(startSorting);
		for (int i = 1; i < listPlatform.Count; i++)
		{
			Transform transform = listPlatform[i].transform;
			Vector3 position = listPlatform[i - 1].transform.position;
			transform.position = new Vector3(0f, position.y + (float)listPlatform[i - 1].getNum() * 0.4f, 0f);
			listPlatform[i].setColor(getColorNextPlatform(listPlatform[i - 1].getColor()));
			listPlatform[i].setSortingOder(listPlatform[i - 1].getSortingOder() - 1);
			Transform transform2 = listPlatform[i].transform;
			Vector3 localScale = listPlatform[i - 1].transform.localScale;
			transform2.localScale = new Vector3(localScale.x * -1f, 1f, 1f);
		}
	}

	private Color getColorNextPlatform(Color color)
	{
		Color white = Color.white;
		white.r = color.r * 0.8f;
		white.b = color.b * 0.8f;
		white.g = color.g * 0.8f;
		return white;
	}

	public int getNumOfCurentPlatform(Platform platform)
	{
		return listPlatform[listPlatform.IndexOf(platform)].getNum();
	}

	public Platform getNextPlatform(Platform platform)
	{
		return listPlatform[listPlatform.IndexOf(platform) + 1];
	}

	public Platform getPreviousPlatform(Platform platform)
	{
		if (listPlatform.IndexOf(platform) - 2 >= 0 && listPlatform[listPlatform.IndexOf(platform) - 2] != null)
		{
			return listPlatform[listPlatform.IndexOf(platform) - 2];
		}
		if (listPlatform.IndexOf(platform) - 1 >= 0 && listPlatform[listPlatform.IndexOf(platform) - 1] != null)
		{
			return listPlatform[listPlatform.IndexOf(platform) - 1];
		}
		return listPlatform[listPlatform.IndexOf(platform)];
	}

	public void updateListPlatform()
	{
		float posYTop = listPlatform[0].getPosYTop();
		Vector3 position = _camera.transform.position;
		if (posYTop < position.y - 5f)
		{
			ObjectPooling.ins.addPlatform(listPlatform[0]);
			listPlatform.RemoveAt(0);
			resetListPlatform();
		}
		Vector3 position2 = listPlatform[listPlatform.Count - 1].transform.position;
		float y = position2.y;
		Vector3 position3 = _camera.transform.position;
		if (y < position3.y + 10f)
		{
			Platform platform = ObjectPooling.ins.GetPlatform();
			listPlatform.Add(platform);
			resetListPlatform();
		}
	}
}
