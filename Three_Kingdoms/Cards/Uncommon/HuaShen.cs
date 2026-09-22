using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Factories;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class HuaShen : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(HuaShen)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(HuaShen)}.png";

    public HuaShen() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
        CardModel? cardModel = CardFactory.GetDistinctForCombat
        (Owner, from c in Owner.Character.CardPool.GetUnlockedCards
        (Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                where c.Type == CardType.Power
                select c, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
        if (cardModel != null)
        {
            await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
        }
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
