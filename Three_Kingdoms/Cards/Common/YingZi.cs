using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Powers;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Common;

[Pool(typeof(TkCardPool))]
public class YingZi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(YingZi)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Power;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;


    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(YingZi)}.png";

    public YingZi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await PowerCmd.Apply<YingZiPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
