using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Powers;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.CardSelection;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class QueDi : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(QueDi)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(QueDi)}.png";
    public QueDi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CreatureCmd.LoseMaxHp(choiceContext, Owner.Creature, 3m, isFromCard: true);
        if (IsUpgraded)
        {
            await CardPileCmd.Draw(choiceContext, 1, Owner);
        }
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 1), null, this));
        await PowerCmd.Apply<QueDiPower>(choiceContext, Owner.Creature, 1m, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
    }
}
