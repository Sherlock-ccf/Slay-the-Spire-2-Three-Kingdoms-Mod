using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Models.Powers;
namespace slay_the_spire_2_three_kingdoms.Powers;

public class JieYingPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JieYing.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/JieYing.png";
    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player == Owner.Player)
        {
            foreach (Creature enermy in CombatState.Enemies)
            {
                if (enermy.HasPower<YingPower>())
                {
                    if (enermy.HasPower<StrengthPower>())
                    {
                        StrengthPower? strengthPower = enermy.GetPower<StrengthPower>();
                        int amount = 0;
                        if (strengthPower != null)
                        {
                            amount = strengthPower.Amount;
                        }
                        await PowerCmd.Apply<StrengthPower>(choiceContext, Owner, amount, Owner, null);
                        await PowerCmd.Apply<StrengthPower>(choiceContext, enermy, -amount, Owner, null);
                    }
                    await PowerCmd.Remove<YingPower>(enermy);
                }
            }
            await PowerCmd.Remove<JieYingPower>(Owner);
        }
    }
}