﻿using System;

public class TileTypeInvisibleBlock : TileType
{
	public override bool IsFloodBlock
	{
		get
		{
			return true;
		}
	}

	public override bool IsFloodDoor
	{
		get
		{
			return true;
		}
	}

	public override bool Invisible
	{
		get
		{
			return true;
		}
	}

	public override bool RepeatBlock
	{
		get
		{
			return false;
		}
	}

	public override BaseTileSelector.SelectType SelectType
	{
		get
		{
			return BaseTileSelector.SelectType.Multiple;
		}
	}

	public override BlockRenderMode blockRenderMode
	{
		get
		{
			return BlockRenderMode.FullBlock;
		}
	}
}
