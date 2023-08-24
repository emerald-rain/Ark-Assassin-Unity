using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
	public SpriteRenderer[] SegmentsPrefabs;

	public SegmentSelectionMode SegmentsMode;

	public LineOverflowMode OverflowMode;

	[HideInInspector]
	public bool useBendLimit = true;

	[HideInInspector]
	public int bendLimit = 45;

	[HideInInspector]
	public bool HangFirstSegment;

	[HideInInspector]
	public Vector2 FirstSegmentConnectionAnchor;

	[HideInInspector]
	public Vector2 LastSegmentConnectionAnchor;

	[HideInInspector]
	public bool HangLastSegment;

	[Range(-0.5f, 0.5f)]
	public float overlapFactor;

	public List<Vector3> nodes = new List<Vector3>(new Vector3[2]
	{
		new Vector3(-3f, 0f, 0f),
		new Vector3(3f, 0f, 0f)
	});

	public bool WithPhysics = true;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
