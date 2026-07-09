using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Models.CardPools;
using MegaCrit.Sts2.Core.ValueProps;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Creatures;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

// 加入哪个卡池
[Pool(typeof(TkCardPool))]
public class NanManRuQin : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(NanManRuQin)}.mp3";
    // 基础耗能
    private const int energyCost = 1;
    // 卡牌类型
    private const CardType type = CardType.Attack;
    // 卡牌稀有度
    private const CardRarity rarity = CardRarity.Uncommon;
    // 目标类型（AnyEnemy表示任意敌人）
    private const TargetType targetType = TargetType.AllEnemies;
    // 是否在卡牌图鉴中显示
    private const bool shouldShowInCardLibrary = true;

    // 卡牌的基础属性
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(13m, ValueProp.Move)
    ];

    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(NanManRuQin)}.png";

    public NanManRuQin() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    // 打出时的效果逻辑
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if(CombatState==null)
        {
            return;
        }
        foreach (Creature enermy in CombatState.HittableEnemies)
        {
            if(enermy.Monster?.IntendsToAttack==false)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(enermy)
                .Execute(choiceContext);
            }
        }
    }

    // 升级后的效果逻辑
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}
