using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
	public List<SpriteRenderer> sprs;

	public List<SpriteRenderer> listActive;

	private float[] posYMax = new float[4]
	{
		-1.6f,
		-1.1f,
		-0.6f,
		-0.1f
	};

	public int num;

	private void OnEnable()
	{
		if (listActive == null)
		{
			listActive = new List<SpriteRenderer>();
		}
		else
		{
			listActive.Clear();
		}
		num = UnityEngine.Random.Range(2, 6);
		for (int i = 0; i < sprs.Count; i++)
		{
			if (i <= num)
			{
				listActive.Add(sprs[i]);
			}
		}
		sprs[1].transform.localPosition = new Vector3(UnityEngine.Random.Range(posYMax[num - 2], 0f), 0.4f, 0f);
		for (int j = 2; j < sprs.Count; j++)
		{
			if (j <= num)
			{
				sprs[j].gameObject.SetActive(value: true);
				sprs[j].transform.localPosition = sprs[j - 1].transform.localPosition + new Vector3(-0.5f, 0.4f, 0f);
			}
			else
			{
				sprs[j].gameObject.SetActive(value: false);
			}
		}
		setCollider(act: false);
	}

	public void setNumPlatform(int num, float posXminPlatform1)
	{
		if (listActive == null)
		{
			listActive = new List<SpriteRenderer>();
		}
		else
		{
			listActive.Clear();
		}
		for (int i = 0; i < sprs.Count; i++)
		{
			if (i <= num)
			{
				listActive.Add(sprs[i]);
			}
		}
		sprs[1].transform.localPosition = new Vector3(UnityEngine.Random.Range(posYMax[num - 2], posXminPlatform1), 0.4f, 0f);
		for (int j = 2; j < sprs.Count; j++)
		{
			if (j <= num)
			{
				sprs[j].gameObject.SetActive(value: true);
				sprs[j].transform.localPosition = sprs[j - 1].transform.localPosition + new Vector3(-0.5f, 0.4f, 0f);
			}
			else
			{
				sprs[j].gameObject.SetActive(value: false);
			}
		}
		setCollider(act: false);
	}

	public void setSortingOder(int oder)
	{
		for (int i = 0; i < listActive.Count; i++)
		{
			listActive[i].sortingOrder = oder;
		}
	}

	public void setColor(Color color)
	{
		for (int i = 0; i < listActive.Count; i++)
		{
			listActive[i].color = color;
		}
	}

	public Color getColor()
	{
		return listActive[0].color;
	}

	public int getSortingOder()
	{
		return listActive[0].sortingOrder;
	}

	public int getNum()
	{
		return listActive.Count - 1;
	}

	public float getPosYTop()
	{
		Vector3 position = listActive[listActive.Count - 1].transform.position;
		return position.y;
	}

	public float getPosXPlatform1()
	{
		Vector3 position = listActive[1].transform.position;
		return position.x;
	}

	public Vector2 getPosIdle()
	{
		return listActive[listActive.Count - 1].transform.position;
	}

	public void setCollider(bool act)
	{
		for (int i = 0; i < sprs.Count; i++)
		{
			sprs[i].gameObject.GetComponent<BoxCollider2D>().enabled = act;
		}
	}
}
