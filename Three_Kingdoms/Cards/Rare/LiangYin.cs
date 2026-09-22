using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Powers;
using Three_Kingdoms.Character;
using Three_Kingdoms.Node;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.CardSelection;
namespace Three_Kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class LiangYin : CustomCardModel
{
    public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(LiangYin)}.mp3";
    private const int energyCost = 1;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(LiangYin)}.png";

    public LiangYin() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CardPlayer.PlayCardSfx(SfxPath);
        List<CardModel> list = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999), context: choiceContext, player: Owner, filter: null, source: this)).ToList();
        if (list.Count != 0)
        {
            await CardCmd.Discard(choiceContext, list);
        }
        await CardPileCmd.Draw(choiceContext, 1m, Owner);
        await PowerCmd.Apply<LiangYinPower>(choiceContext, Owner.Creature, list.Count, Owner.Creature, this);
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
