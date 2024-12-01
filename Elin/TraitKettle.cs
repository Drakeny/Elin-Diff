public class TraitKettle : TraitUniqueChara
{
	public override int CostRerollShop => 0;

	public override bool CanInvest => CanJoinParty;

	public override CopyShopType CopyShop
	{
		get
		{
			if (!CanJoinParty)
			{
				return CopyShopType.None;
			}
			return CopyShopType.Item;
		}
	}

	public override ShopType ShopType
	{
		get
		{
			if (!CanJoinParty)
			{
				return ShopType.None;
			}
			return ShopType.Copy;
		}
	}

	public override PriceType PriceType => PriceType.CopyShop;

	public override bool CanJoinParty
	{
		get
		{
			if (!EClass.game.quests.IsCompleted("vernis_gold"))
			{
				return EClass.debug.enable;
			}
			return true;
		}
	}

	public override bool CanBeBanished => false;

	public override int RestockDay => 30;

	public override bool CanCopy(Thing t)
	{
		if (t.noSell || t.HasElement(1229))
		{
			return false;
		}
		if (t.trait is TraitSeed)
		{
			return true;
		}
		return t.isCrafted;
	}
}
