using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using slay_the_spire_2_three_kingdoms.KeyWords;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class XueHen : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(XueHen)}.mp3";
    private const int energyCost = 2;
    private const CardType type = CardType.Attack;
    private const CardRarity rarity = CardRarity.Uncommon;
    private const TargetType targetType = TargetType.AnyEnemy;
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(6, ValueProp.Move)];
    private const bool shouldShowInCardLibrary = true;
    public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust, TkKeywords.Fire };
    protected override IEnumerable<IHoverTip> ExtraHoverTips => new List<IHoverTip> { HoverTipFactory.FromCard<Sha>(IsUpgraded) };
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(XueHen)}.png";

    public XueHen() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }

    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (cardPlay.Target == null)
        {
            return;
        }
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
        .FromCard(this)
        .Targeting(cardPlay.Target)
        .Execute(choiceContext);
        List<CardModel> list = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999), context: choiceContext, player: Owner, filter: null, source: this)).ToList();
        foreach (CardModel item in list)
        {
            if (CombatState != null)
            {
                CardModel cardModel = CombatState.CreateCard<Sha>(Owner);
                if (IsUpgraded)
                {
                    CardCmd.Upgrade(cardModel);
                }
                cardModel.AddKeyword(CardKeyword.Exhaust);
                cardModel.SetToFreeThisTurn();
                await CardCmd.Transform(item, cardModel);
            }
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(3);
    }
}
