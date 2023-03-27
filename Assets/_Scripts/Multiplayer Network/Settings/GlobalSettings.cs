using UnityEngine;
using System;
using Fusion;

[Serializable]
[CreateAssetMenu(fileName = "GlobalSettings", menuName = "Global Settings")]
public class GlobalSettings : ScriptableObject
{
	public NetworkRunner RunnerPrefab;
	public string LoadingScene = "LoadingScene";
	public string MenuScene = "Menu";
	public bool SimulateMobileInput;

	public AgentSettings Agent;
	public MapSettings Map;
	public NetworkSettings Network;
	public OptionsData DefaultOptions;
}
