using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Powers;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class XingShang : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(XingShang)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Power;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(XingShang)}.png";
    public XingShang() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await PowerCmd.Apply<XingShangPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
