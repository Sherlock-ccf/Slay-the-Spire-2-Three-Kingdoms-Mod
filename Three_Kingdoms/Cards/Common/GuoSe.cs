using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.HoverTips;
using Three_Kingdoms.Node;
using Three_Kingdoms.Cards.Rare;
namespace Three_Kingdoms.Cards.Common;

[Pool(typeof(TkCardPool))]
public class GuoSe : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(GuoSe)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(GuoSe)}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<LeBuSiShu>(IsUpgraded) };
    public GuoSe() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CardCmd.Discard(choiceContext, PileType.Hand.GetPile(Owner).Cards);
        if (Owner.PlayerCombatState != null && CombatState != null)
        {
            CardModel cardModel = CombatState.CreateCard<LeBuSiShu>(Owner);
            if (IsUpgraded)
            {
                cardModel.SetToFreeThisCombat();
            }
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust);
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Ethereal);
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, Owner.PlayerCombatState.Hand.Type, Owner);
        }
    }
}
