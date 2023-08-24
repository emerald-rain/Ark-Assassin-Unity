using UnityEngine;
using UnityEngine.UI;

public class CellDailyGift : MonoBehaviour
{
	public GameObject highLight;

	public GameObject imgDone;

	public Button btnClaim;

	public Button btnDone;

	public Button btnWatchVideo;

	public void onShow(TypeOfDaily typeOfDaily)
	{
		btnClaim.interactable = true;
		btnClaim.gameObject.SetActive(value: true);
		btnDone.gameObject.SetActive(value: false);
		btnWatchVideo.gameObject.SetActive(value: false);
		highLight.SetActive(value: false);
		switch (typeOfDaily)
		{
		case TypeOfDaily.Wait:
			imgDone.SetActive(value: false);
			btnClaim.interactable = false;
			break;
		case TypeOfDaily.Done:
			imgDone.SetActive(value: true);
			btnClaim.gameObject.SetActive(value: false);
			btnDone.gameObject.SetActive(value: true);
			break;
		case TypeOfDaily.Miss:
			imgDone.SetActive(value: false);
			btnClaim.gameObject.SetActive(value: false);
			btnWatchVideo.gameObject.SetActive(value: true);
			break;
		default:
			imgDone.SetActive(value: false);
			highLight.SetActive(value: true);
			break;
		}
	}
}
