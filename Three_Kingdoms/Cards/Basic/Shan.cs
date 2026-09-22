using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;

using Three_Kingdoms.Node;
namespace Three_Kingdoms.Cards.Basic;
[Pool(typeof(TkCardPool))]
public class Shan : CustomCardModel
{
    public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(Shan)}.mp3";
    private const int energyCost = -1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Basic;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override bool CanBeGeneratedInCombat => false;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(Shan)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword>
    {
        CardKeyword.Unplayable
    };
    protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Defend };
    public Shan() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPlayer.PlayCardSfx(SfxPath);
        return;
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
