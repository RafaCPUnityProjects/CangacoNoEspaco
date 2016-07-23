using UnityEngine;
using System.Collections;

public class SquadInfo : MonoBehaviour
{
	public enum SquadDifficulty
	{
		zero,
		one,
		two,
		three,
	};

	public SquadDifficulty squadDifficulty;
	public bool bitSquad = false;
}
