using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Creatures;
using slay_the_spire_2_three_kingdoms.KeyWords;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class JiMie : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(JiMie)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Ancient;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    protected override bool IsPlayable => DynamicVars["AttackPlayed"].BaseValue >= DynamicVars["AttackNeedPlayed"].BaseValue;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(JiMie)}.png";
    protected override bool ShouldGlowGoldInternal => IsPlayable;
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust, TkKeywords.Fire };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("AttackPlayed",0m),
        new DynamicVar("AttackNeedPlayed",5m)
    ];
    public JiMie() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (cardPlay.Target != null)
        {
            await DamageCmd.Attack(cardPlay.Target.MaxHp)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars["AttackNeedPlayed"].BaseValue -= 1;
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
        if (side == Owner.Creature.Side)
        {
            DynamicVars["AttackPlayed"].BaseValue = 0;
        }
    }
}
