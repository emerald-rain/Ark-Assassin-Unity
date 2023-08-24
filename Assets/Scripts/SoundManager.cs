using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public AudioSource bg_audioSource;

	public DataHolder dataHolder;

	public float volumeSound;

	public float volumeMusic;

	public AudioSource audioClick;

	public static SoundManager ins;

	private void Awake()
	{
		ins = this;
		volumeSound = dataHolder.gameData.soundVolume;
		volumeMusic = dataHolder.gameData.musicVolume;
	}

	public void play_audioClick()
	{
		audioClick.volume = dataHolder.gameData.soundVolume;
		audioClick.Play();
	}

	private void Start()
	{
		bg_audioSource.volume = dataHolder.gameData.musicVolume;
		bg_audioSource.Play();
	}

	public void setVolumeBgMusic(float value)
	{
		bg_audioSource.volume = value;
		dataHolder.gameData.musicVolume = value;
		volumeMusic = value;
	}

	public void setVolumeSound(float value)
	{
		volumeSound = value;
		dataHolder.gameData.musicVolume = value;
	}

	public void save()
	{
		dataHolder.gameData.writePre();
	}
}
