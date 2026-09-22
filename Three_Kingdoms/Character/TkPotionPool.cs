using System.Reflection.Metadata.Ecma335;
using BaseLib.Abstracts;
namespace Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models;

public class TkPotionPool : CustomPotionPoolModel
{
    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://Three_Kingdoms/images/character/energy.png";
    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://Three_Kingdoms/images/character/energy_big.png";
}