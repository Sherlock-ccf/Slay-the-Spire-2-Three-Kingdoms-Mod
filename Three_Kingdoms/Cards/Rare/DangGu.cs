using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using Three_Kingdoms.Powers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Players;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class DangGu : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(DangGu)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Power;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(DangGu)}.png";
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("Amount",1m),
        new EnergyVar(1)
    ];
    public DangGu() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (Owner.HasPower<DangGuPower>())
        {
            return;
        }
        await PowerCmd.Apply<DangGuPower>(choiceContext, Owner.Creature, DynamicVars["Amount"].BaseValue, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        DynamicVars["Amount"].UpgradeValueBy(1m);
        DynamicVars.Energy.UpgradeValueBy(1);
    }
    public override async Task AfterAutoPrePlayPhaseEntered(PlayerChoiceContext choiceContext, Player player)
    {
        if (Owner.PlayerCombatState != null && player == Owner && Owner.PlayerCombatState.TurnNumber <= 1)
        {
            await CardCmd.AutoPlay(choiceContext, this, null);
        }
    }
}
