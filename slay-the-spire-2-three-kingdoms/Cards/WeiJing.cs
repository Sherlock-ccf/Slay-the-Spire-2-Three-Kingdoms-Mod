using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using slay_the_spire_2_three_kingdoms.Character;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class WeiJing : CustomCardModel
{
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<Sha>(IsUpgraded),
    HoverTipFactory.FromCard<Shan>() };

    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(WeiJing)}.png";
    public WeiJing() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState != null)
        {
            CardModel card_sha = CombatState.CreateCard<Sha>(Owner);
            CardModel card_shan = CombatState.CreateCard<Shan>(Owner);
            CardCmd.ApplyKeyword(card_sha, CardKeyword.Exhaust);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(card_sha);
            }
            List<CardModel> cards = new() { card_sha, card_shan };
            CardModel? cardModel = await CardSelectCmd.FromChooseACardScreen(choiceContext, cards, Owner, canSkip: false);
            if (cardModel != null)
            {
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, addedByPlayer: true);
            }
        }
    }
}
