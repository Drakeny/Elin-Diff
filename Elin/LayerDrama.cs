﻿using System;
using UnityEngine;

public class LayerDrama : ELayer
{
	public static bool IsActive()
	{
		return ELayer.ui.GetLayer<LayerDrama>(false);
	}

	public override void OnInit()
	{
		LayerDrama.Instance = this;
		LayerDrama.maxBGMVolume = false;
	}

	private void LateUpdate()
	{
		if (this.drama.sequence.isExited && ELayer.ui.GetTopLayer() == this)
		{
			this.Close();
		}
		if (ELayer.ui.TopLayer == this && !this.isDestroyed)
		{
			ELayer.ui.hud.hint.cg.alpha = 0f;
			return;
		}
		ELayer.ui.hud.hint.cg.alpha = 1f;
	}

	public static LayerDrama ActivateMain(string idSheet, string idStep = null, Chara target = null, Card ref1 = null, string tag = "")
	{
		return LayerDrama.Activate("_main", idSheet, idStep, target, ref1, tag);
	}

	public static LayerDrama Activate(string book, string idSheet, string idStep, Chara target = null, Card ref1 = null, string tag = "")
	{
		LayerDrama layerDrama = Layer.Create<LayerDrama>();
		layerDrama.setup = new DramaSetup
		{
			book = book,
			sheet = idSheet,
			step = idStep,
			ref1 = ref1,
			forceJump = LayerDrama.forceJump,
			tag = tag
		};
		LayerDrama.forceJump = null;
		if (target != null)
		{
			layerDrama.setup.person = new Person(target);
		}
		ELayer.ui.AddLayer(layerDrama);
		layerDrama.drama.Play(layerDrama.setup);
		return layerDrama;
	}

	public static LayerDrama ActivateNerun(string idText)
	{
		if (ELayer.debug.skipNerun)
		{
			return null;
		}
		LayerDrama layerDrama = Layer.Create<LayerDrama>();
		layerDrama.setup = new DramaSetup
		{
			book = "_nerun",
			step = "6-1"
		};
		TextAsset textAsset = Resources.Load<TextAsset>(CorePath.Text_DialogHelp + idText);
		layerDrama.setup.textData = GameLang.Convert(((textAsset != null) ? textAsset.text : null) ?? (idText + " not found."));
		layerDrama.option.hideOthers = false;
		layerDrama.option.screenlockType = Layer.Option.ScreenlockType.DarkLight;
		ELayer.ui.AddLayer(layerDrama);
		layerDrama.drama.Play(layerDrama.setup);
		return layerDrama;
	}

	public override void OnUpdateInput()
	{
		base.OnUpdateInput();
	}

	public override void OnKill()
	{
		ELayer.ui.Show(1f);
		LayerDrama.keepBGM = false;
		LayerDrama.haltPlaylist = false;
		SoundManager.current.haltUpdate = false;
		SoundManager.forceBGM = false;
		ELayer.ui.hud.hint.cg.alpha = 1f;
		ELayer.scene.screenElin.focusOption = null;
		LayerDrama.alwaysVisible = null;
		EInput.requireConfirmReset = true;
		LayerDrama.maxBGMVolume = false;
	}

	public static Quest currentQuest;

	public static Religion currentReligion;

	public static bool keepBGM;

	public static bool haltPlaylist;

	public static bool maxBGMVolume;

	public static string forceJump;

	public static LayerDrama Instance;

	public static Card alwaysVisible;

	public static Action refAction1;

	public static Action refAction2;

	public DramaManager drama;

	public CanvasGroup cg;

	public DramaSetup setup;
}