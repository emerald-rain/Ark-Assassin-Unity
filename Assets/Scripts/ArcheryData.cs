using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcheryData", menuName = "ArcheryData")]
public class ArcheryData : E_ScriptableObject
{
	public List<ArcheryD> listArchery;

	public ArcheryD GetArcheryD(int id)
	{
		return listArchery[id];
	}
}
