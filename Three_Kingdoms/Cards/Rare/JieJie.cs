using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
using MegaCrit.Sts2.Core.Models;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class JieJie : CustomCardModel
{
    public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(JieJie)}.mp3";
    private const int energyCost = 2;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(JieJie)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    public JieJie() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPlayer.PlayCardSfx(SfxPath);
        IEnumerable<CardModel> enumerable = PileType.Draw.GetPile(Owner).Cards.Where(Filter).ToList();
        foreach(CardModel item in enumerable)
        {
            await CardPileCmd.Add(item, PileType.Hand);
        }
    }
    private bool Filter(CardModel item)
    {
        return item.Type == CardType.Skill;
    }
    protected override void OnUpgrade()
    {
        RemoveKeyword(CardKeyword.Exhaust);
    }
}
