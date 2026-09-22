using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Relics;
using slay_the_spire_2_three_kingdoms.Character;
using HarmonyLib;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards.Rare;

[Pool(typeof(TkCardPool))]
public class YingJia : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(YingJia)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Rare;
    private const TargetType targetType = TargetType.Self;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(YingJia)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    private bool _hasExtraTurn;
    private bool _paelsEyeWasAlreadyUsed;

    public YingJia() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    public override bool ShouldTakeExtraTurn(Player player)
    {
        return _hasExtraTurn && player == ((CardModel)(object)this).Owner;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 2), null, this));
        _hasExtraTurn = true;
        PaelsEye? paelsEye = Owner.Relics.OfType<PaelsEye>().FirstOrDefault();
        if (paelsEye != null)
        {
            _paelsEyeWasAlreadyUsed = Traverse.Create(paelsEye).Field("_usedThisCombat").GetValue<bool>();
        }
        PlayerCmd.EndTurn(Owner, canBackOut: false);
    }
    public override Task AfterTakingExtraTurn(Player player)
    {
        if (player != Owner) return Task.CompletedTask;
        if (!_hasExtraTurn) return Task.CompletedTask;
        _hasExtraTurn = false;
        if (_paelsEyeWasAlreadyUsed) return Task.CompletedTask;
        PaelsEye? paelsEye = player.Relics.OfType<PaelsEye>().FirstOrDefault();
        if (paelsEye == null) return Task.CompletedTask;
        Traverse.Create(paelsEye).Field("_usedThisCombat").SetValue(false);
        return Task.CompletedTask;
    }
    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}
