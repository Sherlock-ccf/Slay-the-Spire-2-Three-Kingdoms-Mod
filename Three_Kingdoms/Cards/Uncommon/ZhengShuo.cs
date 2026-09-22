using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Uncommon;

[Pool(typeof(TkCardPool))]
public class ZhengShuo : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(ZhengShuo)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(ZhengShuo)}.png";
    public ZhengShuo() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        foreach (CardModel item in PileType.Hand.GetPile(Owner).Cards.ToList())
        {
            await CardPileCmd.Add(item, PileType.Draw);
        }
        await CardPileCmd.Shuffle(choiceContext, Owner);
        await CardPileCmd.Draw(choiceContext, 4, Owner);
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
