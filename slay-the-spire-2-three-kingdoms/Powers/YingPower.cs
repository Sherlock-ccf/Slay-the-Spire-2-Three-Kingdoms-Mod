using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Powers;
namespace slay_the_spire_2_three_kingdoms.Powers;

public class YingPower : CustomPowerModel
{
    public override PowerType Type => PowerType.Debuff;
    public override PowerStackType StackType => PowerStackType.Single;
    public override string? CustomPackedIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/Ying.png";
    public override string? CustomBigIconPath => "res://slay_the_spire_2_three_kingdoms/images/powers/Ying.png";
}