using UnityEngine;
using Util;

public class WorldManager : Singleton<WorldManager>
{
	[SerializeField] private Player player;

	public Player Player => player;
}