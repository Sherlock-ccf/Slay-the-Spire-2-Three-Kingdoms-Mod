using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using slay_the_spire_2_three_kingdoms.KeyWords;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class LongNu : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(LongNu)}.mp3";
    private const int energyCost = 2;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<HuoSha>(IsUpgraded), HoverTipFactory.FromKeyword(CardKeyword.Exhaust), HoverTipFactory.FromKeyword(TkKeywords.Fire) };
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(LongNu)}.png";

    public LongNu() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CreatureCmd.Damage(choiceContext, Owner.Creature, 3, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, Owner.Creature);
        IEnumerable<CardModel> enumerable = PileType.Hand.GetPile(Owner).Cards.ToList();
        int handSize = enumerable.Count();
        await CardCmd.Discard(choiceContext, enumerable);
        await Cmd.CustomScaledWait(0f, 0.25f);
        if (Owner.PlayerCombatState != null && CombatState != null)
        {
            CardModel cardModel = CombatState.CreateCard<HuoSha>(Owner);
            if (IsUpgraded)
            {
                CardCmd.Upgrade(cardModel);
            }
            cardModel.SetToFreeThisCombat();
            CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust);
            cardModel.SetToFreeThisTurn();
            for (int i = 1; i <= handSize; i++)
            {
                CardModel card2 = cardModel.CreateClone();
                await CardPileCmd.AddGeneratedCardToCombat(card2, Owner.PlayerCombatState.Hand.Type, Owner);
            }
        }
    }
}
