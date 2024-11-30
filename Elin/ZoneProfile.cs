﻿using System;
using UnityEngine;

public class ZoneProfile : EScriptable
{
	public static ZoneProfile Load(string id)
	{
		return Resources.Load<ZoneProfile>("World/Zone/Profile/" + id.IsEmpty("Default"));
	}

	public void Generate()
	{
		ZoneBlueprint zoneBlueprint = new ZoneBlueprint();
		zoneBlueprint.Create();
		zoneBlueprint.map = EClass._map;
		zoneBlueprint.zoneProfile = this;
		zoneBlueprint.GenerateMap(EClass._zone);
		EClass._map.RevealAll(true);
	}

	public void RerollBiome()
	{
		this.seeds.biome++;
		this.Generate();
	}

	public void RerollBiomeSub()
	{
		this.seeds.biomeSub++;
		this.Generate();
	}

	public void RerollBush()
	{
		this.seeds.bush++;
		this.Generate();
	}

	public MapGenVariation variation;

	public MapHeight height;

	public ZoneProfile.Seeds seeds;

	public int size = 200;

	public int sizeBounds;

	public int offsetX;

	public int offsetZ;

	public int blockHeight;

	public int water;

	public int bushMod = 2;

	public int extraShores;

	public float shoreHeight;

	public float biomeSubScale = 5f;

	public bool isShore;

	public bool setShore = true;

	public bool noWater;

	public bool river = true;

	public bool useRootSize;

	public bool indoor;

	public bool clearEdge;

	public string idSceneProfile;

	public ZoneProfile.GenType genType;

	public MapBG mapBG;

	public enum GenType
	{
		Default,
		Sky,
		Underground
	}

	[Serializable]
	public class Seeds
	{
		public int height;

		public int poi;

		public int biome;

		public int biomeSub;

		public int bush;
	}
}