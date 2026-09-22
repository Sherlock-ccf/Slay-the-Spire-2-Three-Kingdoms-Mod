using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Models;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class FuJi : CustomCardModel
{
    public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(FuJi)}.mp3";
    private const int energyCost = 2;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(FuJi)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    public FuJi() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null || Owner.PlayerCombatState == null)
        {
            return;
        }
        int num = CombatState.Enemies.Count();
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, num), null, this));
        foreach (CardModel item in Owner.PlayerCombatState.AllCards)
        {
            if (item != this && item.IsUpgradable && (item.Type == CardType.Attack || item.Type == CardType.Skill))
            {
                CardCmd.Upgrade(item);
            }
        }
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}