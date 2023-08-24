using System;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : E_MonoBehaviour
{
	private Animator animator;

	public static NotificationPopup ins;

	public Text txtContent;

	public GameObject parrent;

	public Button btnOk;

	private void Awake()
	{
		ins = this;
		animator = GetComponent<Animator>();
	}

	public void onShow(string content)
	{
		txtContent.text = content;
		btnOk.onClick.RemoveAllListeners();
		btnOk.onClick.AddListener(delegate
		{
			btnOnClose();
		});
		parrent.SetActive(value: true);
		animator.Play("notificationOpen", 0, 0f);
	}

	public void onShow(string content, Action action)
	{
		txtContent.text = content;
		btnOk.onClick.RemoveAllListeners();
		btnOk.onClick.AddListener(delegate
		{
			btnOnClose();
		});
		btnOk.onClick.AddListener(delegate
		{
			action();
		});
		parrent.SetActive(value: true);
		animator.Play("notificationOpen", 0, 0f);
	}

	public void btnOnClose()
	{
		animator.Play("notificationClose", 0, 0f);
		delayFunction(0.5f, delegate
		{
			parrent.SetActive(value: false);
		});
		SoundManager.ins.play_audioClick();
	}
}
