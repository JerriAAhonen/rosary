using UnityEngine;
using Util;

public class WorldManager : Singleton<WorldManager>
{
	[SerializeField] private Player player;
	[SerializeField] private bool hunt;

	public Player Player => player;
	public bool Hunt => hunt;
}