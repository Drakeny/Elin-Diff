﻿using System;

public class ActRush : ActMelee
{
	public override bool ShowMapHighlight
	{
		get
		{
			return true;
		}
	}

	public override void OnMarkMapHighlights()
	{
		if (!EClass.scene.mouseTarget.pos.IsValid || EClass.scene.mouseTarget.TargetChara == null)
		{
			return;
		}
		Point dest = EClass.scene.mouseTarget.pos;
		Los.IsVisible(EClass.pc.pos, dest, delegate(Point p, bool blocked)
		{
			if (p.Equals(EClass.pc.pos))
			{
				return;
			}
			p.cell.highlight = ((blocked || p.IsBlocked || (!p.Equals(dest) && p.HasChara)) ? 4 : ((p.Distance(EClass.pc.pos) <= 2) ? 2 : 8));
			EClass.player.lastMarkedHighlights.Add(p.Copy());
		});
	}

	public override bool CanPerform()
	{
		bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
		if (flag)
		{
			Act.TC = EClass.scene.mouseTarget.TargetChara;
		}
		if (Act.TC == null)
		{
			return false;
		}
		Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
		return !Act.CC.isRestrained && Act.CC.host == null && Act.CC.Dist(Act.TP) > 2 && Los.GetRushPoint(Act.CC.pos, Act.TP) != null && base.CanPerform();
	}

	public override int PerformDistance
	{
		get
		{
			return 6;
		}
	}

	public override bool Perform()
	{
		bool flag = Act.CC.IsPC && !(Act.CC.ai is GoalAutoCombat);
		if (flag)
		{
			Act.TC = EClass.scene.mouseTarget.TargetChara;
		}
		if (Act.TC == null)
		{
			return false;
		}
		Act.TP.Set(flag ? EClass.scene.mouseTarget.pos : Act.TC.pos);
		int num = Act.CC.Dist(Act.TP);
		Point rushPoint = Los.GetRushPoint(Act.CC.pos, Act.TP);
		Act.CC.pos.PlayEffect("vanish");
		Act.CC.MoveImmediate(rushPoint, true, false);
		Act.CC.Say("rush", Act.CC, Act.TC, null, null);
		Act.CC.PlaySound("rush", 1f, true);
		Act.CC.pos.PlayEffect("vanish");
		return base.Attack(1f + 0.1f * (float)num, true);
	}
}