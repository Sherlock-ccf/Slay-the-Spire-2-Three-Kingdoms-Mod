using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.HoverTips;
using slay_the_spire_2_three_kingdoms.Powers;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class JueJin : CustomCardModel
{
    private const int energyCost = 3;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(JueJin)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new BlockVar("BlockGet",15m, ValueProp.Move),
        new BlockVar("BlockExtraGet",7m, ValueProp.Move)
    ];
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<Jiu>() };
    public JueJin() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["BlockGet"], cardPlay);
        List<CardModel> list_hand = PileType.Hand.GetPile(Owner).Cards.ToList();
        List<CardModel> list_discard = PileType.Discard.GetPile(Owner).Cards.ToList();
        List<CardModel> list_draw = PileType.Draw.GetPile(Owner).Cards.ToList();
        int ExhaustCount = list_hand.Count + list_discard.Count + list_draw.Count;
        foreach (CardModel card in list_hand)
        {
            if (card is Jiu || card is Shan || card is Tao)
            {
                await CardCmd.Exhaust(choiceContext, card);
                await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["BlockExtraGet"], cardPlay);
            }
        }
        foreach (CardModel card in list_discard)
        {
            if (card is Jiu || card is Shan || card is Tao)
            {
                await CardCmd.Exhaust(choiceContext, card);
                await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["BlockExtraGet"], cardPlay);
            }
        }
        foreach (CardModel card in list_draw)
        {
            if (card is Jiu || card is Shan || card is Tao)
            {
                await CardCmd.Exhaust(choiceContext, card);
                await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["BlockExtraGet"], cardPlay);
            }
        }
        await PowerCmd.Apply<JueJinPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars["BlockGet"].UpgradeValueBy(5m);
        DynamicVars["BlockExtraGet"].UpgradeValueBy(3m);
    }
}
