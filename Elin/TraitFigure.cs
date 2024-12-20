using UnityEngine;
using UnityEngine.UI;

public class TraitFigure : Trait
{
	public override bool CanBeHallucinated => false;

	public CardRow source => EClass.sources.cards.map.TryGetValue(owner.c_idRefCard);

	public virtual bool ShowShadow => true;

	public virtual int GetMatColor()
	{
		return -1;
	}

	public override void OnSetCardGrid(ButtonGrid b)
	{
		if (!owner.c_idRefCard.IsEmpty())
		{
			SourceChara.Row obj = EClass.sources.charas.map.TryGetValue(owner.c_idRefCard) ?? EClass.sources.charas.map["putty"];
			Transform transform = b.Attach<Transform>("figure", rightAttach: false);
			obj.SetImage(transform.GetChild(0).GetComponent<Image>(), null, 0, setNativeSize: false);
		}
	}

	public override int GetValue()
	{
		if (source == null)
		{
			return base.GetValue();
		}
		return (base.GetValue() + source.LV * 50) * ((!source.multisize) ? 1 : 2) * ((source.quality < 4) ? 1 : 2);
	}
}