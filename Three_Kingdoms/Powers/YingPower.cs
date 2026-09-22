using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
namespace Three_Kingdoms.Powers;

public class YingPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://Three_Kingdoms/images/powers/Ying.png";
    public override string? CustomBigIconPath => "res://Three_Kingdoms/images/powers/Ying.png";
}