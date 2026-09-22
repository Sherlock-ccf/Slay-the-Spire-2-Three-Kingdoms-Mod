using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Models;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class BiYue : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(BiYue)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(BiYue)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("AttackPlayed",0m),
        new DynamicVar("DrawCardLimit",3m)
    ];
    public BiYue() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CardPileCmd.Draw(choiceContext, Math.Min(DynamicVars["AttackPlayed"].BaseValue, DynamicVars["DrawCardLimit"].BaseValue), Owner);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["DrawCardLimit"].UpgradeValueBy(1);
    }
    public override Task AfterCardEnteredCombat(CardModel card)
    {
        if (card != this)
        {
            return Task.CompletedTask;
        }
        if (IsClone)
        {
            return Task.CompletedTask;
        }
        int amount = CombatManager.Instance.History.CardPlaysFinished.Count((CardPlayFinishedEntry e) => e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == Owner && e.HappenedThisTurn(CombatState));
        ReduceCostBy(amount);
        return Task.CompletedTask;
    }
    public override Task BeforeCardPlayed(CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner != Owner)
        {
            return Task.CompletedTask;
        }
        if (cardPlay.Card.Type != CardType.Attack)
        {
            return Task.CompletedTask;
        }
        ReduceCostBy(1);
        return Task.CompletedTask;
    }
    private void ReduceCostBy(decimal amount)
    {
        DynamicVars["AttackPlayed"].BaseValue += amount;
    }
    public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
    {
        if (participants.Contains(Owner.Creature))
        {
            DynamicVars["AttackPlayed"].BaseValue = 0;
        }
    }
}
