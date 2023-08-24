using UnityEngine;
using UnityEngine.UI;

public class Setting : PopupBase
{
	public Slider musicSlider;

	public Slider soundSlider;

	public GameObject imgTick;

	public SoundManager soundManager;

	public void btn_vibrate()
	{
		soundManager.dataHolder.gameData.is_vibrate = !soundManager.dataHolder.gameData.is_vibrate;
		onShow();
		soundManager.save();
		SoundManager.ins.play_audioClick();
	}

	public void sliderMusicDrag()
	{
		soundManager.setVolumeBgMusic(musicSlider.value);
	}

	public void sliderSoundDrag()
	{
		soundManager.setVolumeSound(soundSlider.value);
	}

	public void sliderClickUp()
	{
		soundManager.save();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		onShow();
	}

	private void onShow()
	{
		musicSlider.value = soundManager.dataHolder.gameData.musicVolume;
		soundSlider.value = soundManager.dataHolder.gameData.soundVolume;
		imgTick.SetActive(soundManager.dataHolder.gameData.is_vibrate);
	}
}
