using UnityEngine;

public class ObjBase : MonoBehaviour
{
	public Vector3 _localPosition;

	public Vector3 _localRotation;

	public Vector3 _localScale;

	public Transform parrent;

	public void Awake()
	{
		_localPosition = base.transform.localPosition;
		_localRotation = base.transform.localEulerAngles;
		_localScale = base.transform.localScale;
		parrent = base.transform.parent;
	}

	public void onReset()
	{
		base.transform.SetParent(parrent);
		base.transform.localPosition = _localPosition;
		base.transform.localScale = _localScale;
		base.transform.localEulerAngles = _localRotation;
	}
}
