using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Powers;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class KuangGu : CustomCardModel
{
    private const int energyCost = 2;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;

    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(KuangGu)}.png";

    public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(KuangGu)}.mp3";

    public KuangGu() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPlayer.PlayCardSfx(SfxPath);
        int HpLost = Owner.Creature.CurrentHp - 1;
        await CreatureCmd.Damage(choiceContext, Owner.Creature, HpLost, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
        await PowerCmd.Apply<KuangGuPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
}
