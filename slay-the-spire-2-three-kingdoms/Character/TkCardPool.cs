using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Timeline.Epochs;
using MegaCrit.Sts2.Core.Unlocks;
using Godot;
namespace slay_the_spire_2_three_kingdoms.Character;

public class TkCardPool : CustomCardPoolModel
{
    // 卡池的ID。必须唯一防撞车。
    public override string Title => "TkCharacterPool";

    // 描述中使用的能量图标。大小为24x24。
    public override string? TextEnergyIconPath => "res://slay_the_spire_2_three_kingdoms/images/character/energy.png";

    // tooltip和卡牌左上角的能量图标。大小为74x74。
    public override string? BigEnergyIconPath => "res://slay_the_spire_2_three_kingdoms/images/character/energy_big.png";

    // 卡池的主题色。
    public override Color DeckEntryCardColor => new(1f, 1f, 0f);
    // 如果你使用默认的卡框，可以使用这个颜色来修改卡框的颜色。
    public override Color ShaderColor => new(1f, 1f, 0f);
    // 卡池是否是无色。例如事件、状态等卡池就是无色的。
    public override bool IsColorless => false;
}