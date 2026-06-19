using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Factories;
namespace slay_the_spire_2_three_kingdoms.Powers;

public class XingShangPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/XingShang.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/XingShang.png";
    public override async Task AfterDeath(PlayerChoiceContext choiceContext, Creature target, bool wasRemovalPrevented, float deathAnimLength)
    {
        if (target.Side != Owner.Side && Owner.Player != null)
        {
            CardModel? cardModel = CardFactory.GetDistinctForCombat
                (Owner.Player, from c in Owner.Player.Character.CardPool.GetUnlockedCards
                    (Owner.Player.UnlockState, Owner.Player.RunState.CardMultiplayerConstraint)
                               where c.Rarity == CardRarity.Rare
                               select c, 1, Owner.Player.RunState.Rng.CombatCardGeneration).FirstOrDefault();
            if (cardModel != null)
            {
                cardModel.SetToFreeThisCombat();
                await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner.Player);
            }
        }
    }
}