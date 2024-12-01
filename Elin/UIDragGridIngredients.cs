using System.Collections.Generic;
using UnityEngine;

public class UIDragGridIngredients : EMono
{
	public LayerDragGrid layer;

	public UIList list;

	public UIScrollView view;

	public GameObject goList;

	public void Update()
	{
		bool activeSelf = goList.activeSelf;
		bool isNoGoal = EMono.pc.ai.IsNoGoal;
		goList.SetActive(isNoGoal);
		if (activeSelf != isNoGoal)
		{
			Refresh();
		}
		if (!EInput.middleMouse.down)
		{
			return;
		}
		ButtonGrid componentOf = InputModuleEX.GetComponentOf<ButtonGrid>();
		if ((bool)componentOf && (bool)componentOf.GetComponentInParent<UIDragGridIngredients>())
		{
			Thing t = componentOf.card.Thing;
			t.ShowSplitMenu2(componentOf, "actPutIn", delegate(int n)
			{
				Thing t2 = t.Split(n);
				t2 = EMono.pc.Pick(t2, msg: true, tryStack: false);
				int currentIndex = layer.currentIndex;
				layer.buttons[currentIndex].SetCardGrid(t2, layer.owner);
				layer.owner.OnProcess(t2);
			});
		}
	}

	public void Refresh()
	{
		Debug.Log("Refreshing uiDragGridIngredients");
		List<Thing> list = new List<Thing>();
		if (layer.owner.AllowStockIngredients && !layer.owner.owner.c_isDisableStockUse)
		{
			foreach (Thing thing in EMono._map.Stocked.Things)
			{
				if (layer.owner.ShouldShowGuide(thing) && EMono._map.Stocked.ShouldListAsResource(thing))
				{
					Window.SaveData windowSaveData = thing.parentCard.GetWindowSaveData();
					if (windowSaveData == null || !windowSaveData.excludeCraft)
					{
						list.Add(thing);
						thing.SetSortVal(UIList.SortMode.ByCategory);
					}
				}
			}
			list.Sort((Thing a, Thing b) => b.sortVal - a.sortVal);
		}
		this.list.callbacks = new UIList.Callback<Thing, ButtonGrid>
		{
			onClick = delegate(Thing a, ButtonGrid b)
			{
				int currentIndex = layer.currentIndex;
				layer.buttons[currentIndex].SetCardGrid(a, layer.owner);
				layer.owner.OnProcess(a);
			},
			onInstantiate = delegate(Thing a, ButtonGrid b)
			{
				b.SetCard(a, ButtonGrid.Mode.Grid);
				b.SetOnClick(delegate
				{
				});
				b.onRightClick = delegate
				{
					this.list.callbacks.OnClick(a, b);
				};
			}
		};
		this.list.Clear();
		foreach (Thing item in list)
		{
			this.list.Add(item);
		}
		this.list.Refresh();
	}
}
