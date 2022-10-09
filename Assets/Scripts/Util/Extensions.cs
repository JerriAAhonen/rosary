using System.Collections.Generic;

namespace Util
{
	public static class ListExtensions
	{
		public static T Random<T>(this List<T> self)
		{
			return self[UnityEngine.Random.Range(0, self.Count)];
		}
	}
}