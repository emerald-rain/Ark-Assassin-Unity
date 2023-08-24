using UnityEngine;

public class PopupBase : E_MonoBehaviour
{
	public GameObject fade;

	public virtual void OnEnable()
	{
		GameManager.popupBase = this;
		if (fade != null)
		{
			fade.SetActive(value: true);
		}
	}

	public virtual void onClose()
	{
		GameManager.popupBase = null;
		base.gameObject.SetActive(value: false);
		if (fade != null)
		{
			fade.SetActive(value: true);
		}
		SoundManager.ins.play_audioClick();
	}
}
