using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class RunWei : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(RunWei)}.mp3";
    private const int energyCost = 2;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CardsVar(3),
        new BlockVar("BlockBase", 6m, ValueProp.Move),
        new BlockVar("BlockGet", 0m, ValueProp.Move),
    ];
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(RunWei)}.png";
    public RunWei() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        IEnumerable<CardModel> cards = await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
        IEnumerable<CardModel> discard = await CardSelectCmd.FromSimpleGrid(choiceContext, cards.ToList(), Owner, new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999));
        int count = discard.Count();
        await CardCmd.Discard(choiceContext, discard);
        DynamicVars["BlockGet"].BaseValue = DynamicVars["BlockBase"].BaseValue * count;
        await CreatureCmd.GainBlock(Owner.Creature, (BlockVar)DynamicVars["BlockGet"], cardPlay);
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Cards.UpgradeValueBy(2m);
    }
}
