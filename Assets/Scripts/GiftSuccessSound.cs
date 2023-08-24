using UnityEngine;

public class GiftSuccessSound : MonoBehaviour
{
	public AudioSource audioSource;

	private void OnEnable()
	{
		audioSource.volume = SoundManager.ins.volumeSound;
		audioSource.Play();
	}
}
