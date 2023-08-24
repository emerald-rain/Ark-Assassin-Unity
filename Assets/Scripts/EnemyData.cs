using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData")]
public class EnemyData : ScriptableObject
{
	public int id;

	public string heroName;

	public string displayName;

	public int hp;

	public int dame;

	public float speedAttack;
}
