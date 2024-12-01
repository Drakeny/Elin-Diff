using System;

public class TraitMod : TraitItem
{
	public override bool CanStack => false;

	public SourceElement.Row source => EClass.sources.elements.map[owner.refVal];

	public override void OnCreate(int lv)
	{
		Tuple<SourceElement.Row, int> tuple = Thing.GetEnchant(lv, (SourceElement.Row r) => r.tag.Contains("modRanged"), neg: false);
		if (tuple == null)
		{
			tuple = new Tuple<SourceElement.Row, int>(EClass.sources.elements.map[600], EClass.rnd(10) + 1);
		}
		owner.refVal = tuple.Item1.id;
		owner.encLV = tuple.Item2;
	}

	public override bool OnUse(Chara c)
	{
		LayerDragGrid.Create(new InvOwnerMod(owner));
		return false;
	}

	public override void SetName(ref string s)
	{
		s = "_of".lang(source.GetName(), s);
	}

	public override int GetValue()
	{
		int num = source.value * owner.encLV;
		return base.GetValue() * num / 100;
	}
}
