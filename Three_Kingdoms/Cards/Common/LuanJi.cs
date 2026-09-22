using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Common;

[Pool(typeof(TkCardPool))]
public class LuanJi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(LuanJi)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<WanJianQiFa>(IsUpgraded) };
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(LuanJi)}.png";
    public LuanJi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 2), null, this));
        if (CombatState != null)
        {
            CardModel cardModel = CombatState.CreateCard<WanJianQiFa>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(cardModel);
            }
            cardModel.SetToFreeThisCombat();
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust);
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Ethereal);
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
        }
    }
}
