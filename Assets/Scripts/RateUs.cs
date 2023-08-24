using UnityEngine;

public class RateUs : MonoBehaviour
{
	public void btn_rate()
	{
		Application.OpenURL("market://details?id=" + Application.identifier);
		SoundManager.ins.play_audioClick();
		base.gameObject.SetActive(value: false);
		PlayerPrefs.SetInt("click_rate", 1);
	}

	public void btn_cancel()
	{
		NotificationPopup.ins.onShow("Thank you for your rating!\n We will do our best to improve the game and your experience in the next version.");
		SoundManager.ins.play_audioClick();
		base.gameObject.SetActive(value: false);
	}
}
