using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.CardSelection;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Common;

[Pool(typeof(TkCardPool))]
public class WuGuFengDeng : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(WuGuFengDeng)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(WuGuFengDeng)}.png";
    public WuGuFengDeng() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CardPileCmd.ShuffleIfNecessary(choiceContext, Owner);
        await CardPileCmd.Add(await CardSelectCmd.FromSimpleGrid(choiceContext, (from c in PileType.Draw.GetPile(Owner).Cards orderby c.Rarity, c.Id select c).ToList(), Owner, new CardSelectorPrefs(SelectionScreenPrompt, 1)), PileType.Hand);
    }
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
