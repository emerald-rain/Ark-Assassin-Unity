using UnityEngine;
using UnityEngine.UI;

public class ToolControl : MonoBehaviour
{
	public Text txtNum;

	public GameObject valueObj;

	public void onShow(int num)
	{
		if (num > 0)
		{
			txtNum.transform.parent.gameObject.SetActive(value: true);
			valueObj.SetActive(value: false);
			txtNum.text = num + string.Empty;
		}
		else
		{
			txtNum.transform.parent.gameObject.SetActive(value: false);
			valueObj.SetActive(value: true);
			txtNum.text = string.Empty;
		}
	}
}
