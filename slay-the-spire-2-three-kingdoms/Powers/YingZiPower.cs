using MegaCrit.Sts2.Core.Entities.Players;
namespace slay_the_spire_2_three_kingdoms.Powers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;



public class YingZiPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/YingZi.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/YingZi.png";

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner.Player)
        {
            return count;
        }
        return count + Amount;
    }
}