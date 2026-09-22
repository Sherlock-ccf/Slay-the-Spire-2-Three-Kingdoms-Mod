using MegaCrit.Sts2.Core.Entities.Players;
namespace Three_Kingdoms.Powers;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;



public class YingZiPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Single;

    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/YingZi.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/YingZi.png";

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner.Player)
        {
            return count;
        }
        return count + Amount;
    }
}