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
public class TuShe : CustomCardModel
{
	public string SfxPath => $"res://Three_Kingdoms/sfx/{nameof(TuShe)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://Three_Kingdoms/images/cards/{nameof(TuShe)}.png";
    protected override bool IsPlayable => CardPile.GetCards(Owner, PileType.Hand).All((CardModel c) => c.Rarity != CardRarity.Basic);
    protected override bool ShouldGlowGoldInternal => IsPlayable;
    public TuShe() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if(CombatState==null)
        {
            return;
        }
        int cardDraw = CombatState.Enemies.Count();
        await CardPileCmd.Draw(choiceContext, cardDraw, Owner);
    }
    protected override void OnUpgrade()
    {
    }
}
