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
namespace slay_the_spire_2_three_kingdoms.Cards;

// 加入哪个卡池
[Pool(typeof(TkCardPool))]
public class YingJia : CustomCardModel
{
    // 基础耗能
    private const int energyCost = 0;
    // 卡牌类型
    private const CardType type = CardType.Skill;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Rare;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.Self;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(YingJia)}.png";
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
    private bool _hasExtraTurn;       // 是否拥有“额外回合”
    private bool _paelsEyeWasAlreadyUsed; // 佩尔之眼是否已经用过

    public YingJia() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    public override bool ShouldTakeExtraTurn(Player player)
    {
        return _hasExtraTurn && player == ((CardModel)(object)this).Owner;
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
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
