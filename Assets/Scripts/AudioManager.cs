using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Util;

public class AudioManager : Singleton<AudioManager>
{
	[SerializeField] private Transform sourceContainer;
	[SerializeField] private Transform mainCamTm;
	[Space]
	[SerializeField] private AudioClip dayMusic;
	[SerializeField] private AudioClip nightMusic;
	
	private IObjectPool<AudioSource> pool;
	private List<AudioSource> activeSources = new();
	private Dictionary<AudioEvent, float> cooldowns = new();
	private AudioSource dayMusicSource;
	private AudioSource nightMusicSource;

	protected override void Awake()
	{
		base.Awake();

		pool = new ObjectPool<AudioSource>(
			CreateAudioSource,
			s =>
			{
				s.gameObject.SetActive(true);
				activeSources.Add(s);
			},
			s =>
			{
				s.Stop();
				s.clip = null;
				s.gameObject.SetActive(false);
				s.transform.SetParent(sourceContainer);
				activeSources.Remove(s);
			},
			Destroy);

		dayMusicSource = CreateAudioSource();
		dayMusicSource.clip = dayMusic;
		dayMusicSource.loop = true;
		dayMusicSource.transform.SetParent(mainCamTm);
		dayMusicSource.transform.localPosition = Vector3.zero;
		
		nightMusicSource = CreateAudioSource();
		nightMusicSource.clip = nightMusic;
		nightMusicSource.loop = true;
		nightMusicSource.transform.SetParent(mainCamTm);
		nightMusicSource.transform.localPosition = Vector3.zero;
	}

	private void LateUpdate()
	{
		foreach (var s in activeSources)
		{
			if (!s.isPlaying)
			{
				pool.Release(s);
				return;
			}
		}
	}
	
	/// <summary>
	/// Plays given Audio event once
	/// </summary>
	/// <param name="ae">Audio Event</param>
	/// <param name="pos">Position in world space where to play the audio event. Leave null to play at Camera</param>
	/// <returns>Duration of the audio clip</returns>
	public float PlayOnce(AudioEvent ae, Vector3? pos = null)
	{
		if (!CanBePlayed(ae))
			return 0f;

		var s = pool.Get();
		s.clip = ae.Clip;
		s.volume = ae.Volume;
		s.pitch = ae.Pitch;
		s.loop = false;

		if (pos.HasValue)
			s.transform.position = pos.Value;
		else
		{
			s.transform.SetParent(mainCamTm);
			s.transform.localPosition = Vector3.zero;
		}

		if (ae.Delay <= 0f)
			s.Play();
		else
			s.PlayDelayed(ae.Delay);

		// TODO: Return audiosource to pool
		
		return s.clip.length / Mathf.Abs(s.pitch);
	}

	public void PlayMusic(bool day)
	{
		StartCoroutine(MusicSwitchRoutine(day));
	}

	private IEnumerator MusicSwitchRoutine(bool day)
	{
		if (day)
		{
			dayMusicSource.time = 0f;
			dayMusicSource.Play();
		}
		else
		{
			nightMusicSource.time = 0f;
			nightMusicSource.Play();
		}
		
		var duration = 1f;
		var elapsed = 0f;
		while (elapsed < duration)
		{
			var step = elapsed / duration;
			dayMusicSource.volume = Mathf.Lerp(day ? 0f : 1f, day ? 1f : 0f, step);
			nightMusicSource.volume = Mathf.Lerp(day ? 1f : 0f, day ? 0f : 1f, step);
			
			elapsed += Time.deltaTime;
			yield return null;
		}
	}

	private AudioSource CreateAudioSource()
	{
		var go = new GameObject("AudioSource");
		go.transform.SetParent(sourceContainer);

		var source = go.AddComponent<AudioSource>();
		source.playOnAwake = false;
		source.spatialBlend = 1f;
		source.minDistance = 10f;
		source.maxDistance = 30f;
		return source;
	}

	private bool CanBePlayed(AudioEvent ae)
	{
		if (ae.MINInterval <= 0f)
			return true;

		if (cooldowns.TryGetValue(ae, out float endsAt) && Time.realtimeSinceStartup < endsAt)
			return false;

		cooldowns[ae] = Time.realtimeSinceStartup + ae.MINInterval;
		return true;
	}
}