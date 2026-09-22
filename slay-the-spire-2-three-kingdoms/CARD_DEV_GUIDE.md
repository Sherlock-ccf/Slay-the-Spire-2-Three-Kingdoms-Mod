# 卡牌开发文档

本文档记录 `Cards/` 目录下卡牌代码的接口约定，以及卡牌里实际用到的游戏 API。
所有签名均取自本仓库现有 87 张卡牌的真实调用，可直接照抄。

- 卡牌类继承 `CustomCardModel`（来自 `BaseLib.Abstracts`）。
- 大量成员来自基类 `megaCrit.Sts2.Core.Models.CardModel`，本仓库只用到其中一部分，本文只列用到的。
- 卡牌文案在 `slay_the_spire_2_three_kingdoms/localization/zhs/cards.json`。

---

## 1. 最小骨架

```csharp
using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using slay_the_spire_2_three_kingdoms.Character;
using slay_the_spire_2_three_kingdoms.Node;
namespace slay_the_spire_2_three_kingdoms.Cards;

[Pool(typeof(TkCardPool))]
public class BingLiangCunDuan : CustomCardModel
{
	public string SfxPath => $"res://slay_the_spire_2_three_kingdoms/sfx/{nameof(BingLiangCunDuan)}.mp3";
    private const int energyCost = 0;
    private const CardType type = CardType.Skill;
    private const CardRarity rarity = CardRarity.Common;
    private const TargetType targetType = TargetType.AnyEnemy;
    private const bool shouldShowInCardLibrary = true;
    public override string PortraitPath => $"res://slay_the_spire_2_three_kingdoms/images/cards/{nameof(BingLiangCunDuan)}.png";
    protected override IEnumerable<IHoverTip> ExtraHoverTips
    => new[] { HoverTipFactory.FromPower<VulnerablePower>() };
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PowerVar<VulnerablePower>(1m),
    ];
    public BingLiangCunDuan() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
    {
    }
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
		CardPlayer.PlayCardSfx(SfxPath);
        if (cardPlay.Target != null)
        {
            await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
        }
    }
    protected override void OnUpgrade()
    {
        DynamicVars.Vulnerable.UpgradeValueBy(2m);
    }
}
```

---

## 2. 命名与注册

| 项 | 约定 |
| --- | --- |
| 文件名 | 与类名一致，如 `BingLiangCunDuan.cs` |
| 命名空间 | `slay_the_spire_2_three_kingdoms.Cards` |
| 类名 | PascalCase，用卡牌名的拼音 |
| 卡池注册 | 类上加 `[Pool(typeof(TkCardPool))]` |
| 资源路径 | 图片 `res://slay_the_spire_2_three_kingdoms/images/cards/<类名>.png`，音效 `.../sfx/<类名>.mp3` |
| 本地化 ID | `SLAY_THE_SPIRE_2_THREE_KINGDOMS-<大写下划线类名>`，如 `...-BING_LIANG_CUN_DUAN` |

本地化 ID 由类名转大写下划线得到（`BingLiangCunDuan` → `BING_LIANG_CUN_DUAN`）。

---

## 3. 构造函数参数

```csharp
public BingLiangCunDuan() : base(energyCost, type, rarity, targetType, shouldShowInCardLibrary)
```

| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `energyCost` | `int` | 基础费用。`-1` 表示 X 费（同时需重写 `HasEnergyCostX => true`） |
| `type` | `CardType` | 卡牌类型：`Attack` / `Skill` / `Power` |
| `rarity` | `CardRarity` | 稀有度：`Basic` / `Common` / `Uncommon` / `Rare` / `Token` / `Ancient` |
| `targetType` | `TargetType` | 目标：`Self` / `AnyEnemy` / `AllEnemies` |
| `shouldShowInCardLibrary` | `bool` | 是否在卡牌图鉴中显示 |

通常把前 5 项写成 `private const` 字段，再在构造函数里传给 `base`。

---

## 4. 可重写成员

### 4.1 资源

| 成员 | 类型 | 作用 |
| --- | --- | --- |
| `PortraitPath` | `string` | **必填**。卡面图片路径 |
| `SfxPath` | `string` | 本项目自定义约定（非基类成员）。配合 `CardPlayer.PlayCardSfx(SfxPath)` 在打出时播音效 |
| `CardPlayer.PlayCardSfx(path)` | `static void` | 项目内工具方法（`Node/CardPlayer.cs`），在战斗房间播放音效；非交互模式或加载失败时静默跳过 |

### 4.2 数值：`CanonicalVars`

声明卡牌的动态数值。返回 `IEnumerable<DynamicVar>`，写法有集合表达式 `=> [ ... ]`
和 `=> new List<DynamicVar> { ... }` 两种，效果相同。

常用 Var 类型：

| 类型 | 用法 | 说明 |
| --- | --- | --- |
| `DamageVar` | `new DamageVar(6, ValueProp.Move)` | 伤害，键名为 `Damage`，可用 `DynamicVars.Damage` 取 |
| `BlockVar` | `new BlockVar("BlockGet", 15m, ValueProp.Move)` | 格挡，**必须**带自定义键名（本仓库用 `BlockGet` / `BlockExtraGet` / `BlockBase`），用 `DynamicVars["BlockGet"]` 取 |
| `CardsVar` | `new CardsVar(3)` | 抽牌数，键名为 `Cards` |
| `HealVar` | `new HealVar(5m)` | 治疗量，用 `DynamicVars.Heal` 取 |
| `EnergyVar` | `new EnergyVar(1)` | 能量 |
| `PowerVar<T>` | `new PowerVar<VulnerablePower>(1m)` | 某种 Power 的层数；键名为 Power 类型名，如 `DynamicVars.Vulnerable` |
| `DynamicVar` | `new DynamicVar("AttackPlayed", 0m)` | 自定义键名，用 `DynamicVars["AttackPlayed"]` 取 |
| `CalculationBaseVar` / `ExtraDamageVar` / `CalculatedDamageVar` | 见下 | 动态计算伤害三件套 |

动态计算伤害（造成「基础 + 每层虚弱 × 额外值」这类效果）：

```csharp
protected override IEnumerable<DynamicVar> CanonicalVars => new List<DynamicVar>
{
    new DynamicVar("WeakGive", 1m),
    new CalculationBaseVar(6m),          // 固定基础部分
    new ExtraDamageVar(2m),              // 每个乘数的增量
    new CalculatedDamageVar(ValueProp.Move)
        .WithMultiplier((CardModel _, Creature? target) => target?.GetPowerAmount<WeakPower>() ?? 0)
};
```

`WithMultiplier` 的委托返回乘数个数，最终伤害 = `CalculationBaseVar + ExtraDamageVar × 乘数`。
本地化里分别用 `{CalculatedDamage:diff()}`、`{ExtraDamage:diff()}` 引用。

数值读写与升级：

| 写法 | 作用 |
| --- | --- |
| `DynamicVars.Damage.BaseValue` | 读基础值（打出时用它做参数） |
| `DynamicVars["AttackPlayed"].BaseValue = 0` | 按自定义键名读写 |
| `DynamicVars.Damage.UpgradeValueBy(3m)` | 升级后基础值 +3（在 `OnUpgrade` 中用） |
| `DynamicVars.Cards.IntValue` | 取整数形式 |
| `EnergyCost.UpgradeBy(-1)` | 升级后费用 -1（在 `OnUpgrade` 中用） |

### 4.3 提示

| 成员 | 作用 |
| --- | --- |
| `ExtraHoverTips` | 悬浮卡牌时额外显示的关键字/卡牌说明。返回 `IEnumerable<IHoverTip>` |
| `HoverTipFactory.FromPower<T>()` | 由 Power 生成提示，如 `HoverTipFactory.FromPower<VulnerablePower>()` |
| `HoverTipFactory.FromCard<T>()` | 由卡牌生成提示，如 `HoverTipFactory.FromCard<BingLinChengXia>()` |
| `HoverTipFactory.FromKeyword(...)` | 由关键字生成提示 |

```csharp
protected override IEnumerable<IHoverTip> ExtraHoverTips
    => new[] { HoverTipFactory.FromPower<VulnerablePower>() };
```

### 4.4 标签与关键字

| 成员 | 作用 |
| --- | --- |
| `CanonicalKeywords` | 卡牌固有状态：`Exhaust`（消耗）/ `Innate`（固有）/ `Ethereal`（虚无）/ `Unplayable`（不能打出） |
| `CanonicalTags` | 卡牌分类标签，用于「属于某类牌」的判定，如 `new HashSet<CardTag> { CardTag.Strike }` |
| `AddKeyword(CardKeyword.Innate)` | 运行时追加关键字（常见于 `OnUpgrade`） |

```csharp
public override IEnumerable<CardKeyword> CanonicalKeywords => new List<CardKeyword> { CardKeyword.Exhaust };
protected override HashSet<CardTag> CanonicalTags => new HashSet<CardTag> { CardTag.Strike };
```

### 4.5 可打出性与显示

| 成员 | 作用 |
| --- | --- |
| `HasEnergyCostX` | 返回 `true` 表示 X 费牌，费用由玩家当前能量决定 |
| `IsPlayable` | 动态判断能否打出，如 `DynamicVars["AttackPlayed"].BaseValue >= DynamicVars["AttackNeedPlayed"].BaseValue` |
| `ShouldGlowGoldInternal` | 返回 `true` 时卡牌在手中发金光（常用于提示「现在可以打出」） |
| `CanBeGeneratedInCombat` | 是否允许战斗中被生成到手牌 |

```csharp
protected override bool HasEnergyCostX => true;
protected override bool IsPlayable => DynamicVars["AttackPlayed"].BaseValue >= DynamicVars["AttackNeedPlayed"].BaseValue;
protected override bool ShouldGlowGoldInternal => IsPlayable;
```

---

## 5. 生命周期钩子

`OnPlay` 是核心，其余为可选钩子。全部为 `async` 时返回 `Task`，同步的返回 `void`/`bool`。

| 钩子 | 触发时机 |
| --- | --- |
| `OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)` | **打出卡牌时**。`cardPlay.Target` 是玩家选中的目标，可能为 `null` |
| `OnUpgrade()` | 卡牌被升级时，改数值用 |
| `AfterDowngraded()` | 卡牌被降级时；重写后一般先调 `base.AfterDowngraded()` |
| `BeforeCardPlayed(CardPlay cardPlay)` | 任意卡牌被打出**前**（用于全局监听，如「本回合每打出一张攻击牌」） |
| `AfterCardEnteredCombat(CardModel card)` | 有牌进入战斗时；配合 `if (card != this) return Task.CompletedTask;` 过滤自己 |
| `AfterSideTurnEnd(PlayerChoiceContext, CombatSide side, IEnumerable<Creature> participants)` | 某一方回合结束时，常用于重置本回合计数 |
| `AfterAutoPrePlayPhaseEntered(PlayerChoiceContext, Player player)` | 自动出牌阶段开始时 |
| `AfterDamageReceived(PlayerChoiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)` | 受到伤害后 |
| `ShouldTakeExtraTurn(Player player)` | 是否获得额外回合（返回 `bool`） |
| `AfterTakingExtraTurn(Player player)` | 额外回合结束后 |

示例（`OnPlay` 里用目标）：

```csharp
protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
{
    CardPlayer.PlayCardSfx(SfxPath);
    if (cardPlay.Target != null)
    {
        await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
    }
}
```

示例（重置本回合计数器）：

```csharp
public override async Task AfterSideTurnEnd(PlayerChoiceContext choiceContext, CombatSide side, IEnumerable<Creature> participants)
{
    if (participants.Contains(Owner.Creature))
    {
        DynamicVars["AttackPlayed"].BaseValue = 0;
    }
}
```

---

## 6. 常用命令 API

以下命令类均在 `MegaCrit.Sts2.Core.Commands` 命名空间下，除特别说明外第一个参数都传 `choiceContext`。

### PowerCmd — 增减 Power

| 调用 | 说明 |
| --- | --- |
| `PowerCmd.Apply<TPower>(choiceContext, target, amount, source, card)` | 给 `target` 叠加 `amount` 层 `TPower` |
| `PowerCmd.Remove<TPower>(target)` | 移除 `target` 身上的 `TPower`（无 `choiceContext` 参数） |

```csharp
await PowerCmd.Apply<VulnerablePower>(choiceContext, cardPlay.Target, DynamicVars.Vulnerable.BaseValue, Owner.Creature, this);
await PowerCmd.Apply<ChengHaoPower>(choiceContext, Owner.Creature, DynamicVars.Cards.BaseValue, Owner.Creature, this);
```

### CardPileCmd — 牌堆操作

| 调用 | 说明 |
| --- | --- |
| `CardPileCmd.Draw(choiceContext, count, Owner)` | 抽 `count` 张牌，返回抽到的牌 |
| `CardPileCmd.AddGeneratedCardToCombat(card, PileType.Hand, Owner)` | 把生成的牌加入战斗中的手牌 |
| `CardPileCmd.Add(card, PileType.Hand)` | 把已有牌加入指定牌堆（无 `choiceContext` 参数） |
| `CardPileCmd.Shuffle(choiceContext, Owner)` / `ShuffleIfNecessary(...)` | 洗牌 / 按需洗牌 |

```csharp
await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.IntValue, Owner);
await CardPileCmd.AddGeneratedCardToCombat(cardModel, PileType.Hand, Owner);
```

取牌堆：`PileType.Hand.GetPile(Owner).Cards`、`PileType.Draw.GetPile(Owner).Cards`、`PileType.Discard`、`PileType.Deck`。

### CardCmd — 对手牌/卡牌本身的操作

| 调用 | 说明 |
| --- | --- |
| `CardCmd.Discard(choiceContext, cards)` | 弃掉指定牌 |
| `CardCmd.DiscardAndDraw(choiceContext, cards, count)` | 弃 `cards` 并抽 `count` 张 |
| `CardCmd.Exhaust(choiceContext, card)` | 消耗指定牌 |
| `CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust)` | 给牌追加关键字（无 `choiceContext` 参数） |
| `CardCmd.Upgrade(cardModel)` | 升级指定牌（无 `choiceContext` 参数） |
| `CardCmd.Transform(card, into)` | 把 `card` 变形为 `into` |
| `CardCmd.AutoPlay(choiceContext, card, target)` | 自动打出（`target` 可为 `null`） |
| `CardCmd.PreviewCardPileAdd(cards)` | 预览将要加入牌堆的牌 |

```csharp
await CardCmd.Discard(choiceContext, await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, 2), null, this));
await CardCmd.ApplyKeyword(cardModel, CardKeyword.Exhaust);
await CardCmd.AutoPlay(choiceContext, this, null);
```

### DamageCmd — 造成伤害

链式调用：`Attack(基础伤害)` → `.FromCard(this)` → `.Targeting(目标)` 或 `.TargetingAllOpponents(CombatState)` → `.Execute(choiceContext)`。
可选 `.WithHitCount(n)` 表示打 n 次。

```csharp
await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this)
    .Targeting(cardPlay.Target)
    .Execute(choiceContext);

await DamageCmd.Attack(DynamicVars.Damage.BaseValue).WithHitCount(num)
    .FromCard(this)
    .Targeting(cardPlay.Target)
    .Execute(choiceContext);

await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
    .FromCard(this)
    .TargetingAllOpponents(CombatState)
    .Execute(choiceContext);
```

### CardSelectCmd — 让玩家选牌

| 调用 | 说明 |
| --- | --- |
| `CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(...), filter, source)` | 从手牌选牌用于丢弃，返回选中的牌 |
| `CardSelectCmd.FromHand(prefs:, context:, player:, filter:, source:)` | 从手牌选牌（具名参数调用） |
| `CardSelectCmd.FromSimpleGrid(choiceContext, cards, Owner, prefs)` | 从给定牌列表中选牌 |

`CardSelectorPrefs` 支持两种构造：

- `new CardSelectorPrefs(提示语, 张数)` —— 固定选 `张数` 张。
- `new CardSelectorPrefs(提示语, 最少张数, 最多张数)` —— 区间选择，如 `(prompt, 0, 999999999)` 表示「任意张」。

提示语常用内置的 `CardSelectorPrefs.DiscardSelectionPrompt`，或本项目卡牌自定义的 `SelectionScreenPrompt`
（对应本地化里的 `.selectionScreenPrompt` 条目）。

```csharp
await CardSelectCmd.FromHandForDiscard(choiceContext, Owner, new CardSelectorPrefs(CardSelectorPrefs.DiscardSelectionPrompt, num), null, this);

await CardSelectCmd.FromSimpleGrid(choiceContext, cards.ToList(), Owner,
    new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999));

await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, 0, 999999999),
    context: choiceContext, player: Owner, filter: null, source: this);
```

### CreatureCmd — 生物（玩家或敌人）数值

| 调用 | 说明 |
| --- | --- |
| `CreatureCmd.GainBlock(creature, (BlockVar)DynamicVars["BlockGet"], cardPlay)` | 获得格挡，第二参传 BlockVar |
| `CreatureCmd.Damage(choiceContext, creature, amount, valueProps, source)` | 直接造成伤害（可用于扣自己血） |
| `CreatureCmd.Heal(creature, amount)` | 治疗（无 `choiceContext` 参数） |
| `CreatureCmd.LoseMaxHp(choiceContext, creature, amount, isFromCard: true)` | 失去最大生命 |
| `CreatureCmd.Stun(target)` | 眩晕，让目标跳过回合（无 `choiceContext` 参数） |
| `CreatureCmd.TriggerAnim(creature, "Cast", delay)` | 播放动画，如 `CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay)` |

主动失去生命（即「失去 X 点生命」类效果）的标准写法——无视格挡、不受力量与增伤影响：

```csharp
await CreatureCmd.Damage(choiceContext, Owner.Creature, 3, ValueProp.Unblockable | ValueProp.Unpowered | ValueProp.Move, this);
```

最后一个参数是伤害来源，传 `this`（卡牌）或 `Owner.Creature` 都可以，会影响伤害的归属判定。

### PlayerCmd — 玩家层面

| 调用 | 说明 |
| --- | --- |
| `PlayerCmd.GainEnergy(amount, Owner)` | 获得能量 |
| `PlayerCmd.EndTurn(Owner, canBackOut: false)` | 直接结束回合 |

```csharp
PlayerCmd.GainEnergy(1, Owner);
PlayerCmd.EndTurn(Owner, canBackOut: false);
```

### CardFactory — 生成卡牌

| 调用 | 说明 |
| --- | --- |
| `CardFactory.GetDistinctForCombat(...)` | 从卡池按条件取不重复的牌用于战斗（配 LINQ 查询） |

```csharp
CardModel? cardModel = CardFactory.GetDistinctForCombat
    (Owner, from c in Owner.Character.CardPool.GetUnlockedCards
        (Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
        where c.Type == CardType.Power
        select c, 1, Owner.RunState.Rng.CombatCardGeneration).FirstOrDefault();
```

### 基类可用成员（`this.` / 直接访问）

| 成员 | 类型 | 说明 |
| --- | --- | --- |
| `Owner` | `Player` | 卡牌拥有者 |
| `Owner.Creature` | `Creature` | 拥有者的战斗生物（扣血、加格挡的对象） |
| `Owner.Relics` | 遗物集合 | 如 `Owner.Relics.OfType<PaelsEye>().FirstOrDefault()` |
| `Owner.HasPower<T>()` | `bool` | 是否持有某 Power |
| `Owner.PlayerCombatState` | 战斗状态 | 如 `.Hand.Type`、`.TurnNumber` |
| `Owner.RunState` | 局内状态 | 如 `.Rng.CombatCardGeneration` |
| `CombatState` | 战斗状态 | 可能为 `null`，用前判空；`.Enemies`、`.HittableEnemies`、`.RoundNumber`、`.CreateCard(...)` |
| `IsClone` | `bool` | 该实例是否为克隆 |
| `EnergyCost` | 费用对象 | `EnergyCost.UpgradeBy(-1)` |

---

## 7. 枚举速查

| 枚举 | 本仓库用到的值 |
| --- | --- |
| `CardType` | `Attack`、`Skill`、`Power` |
| `CardRarity` | `Basic`、`Common`、`Uncommon`、`Rare`、`Token`、`Ancient` |
| `TargetType` | `Self`、`AnyEnemy`、`AllEnemies` |
| `CardKeyword` | `Exhaust`、`Innate`、`Ethereal`、`Unplayable` |
| `PileType` | `Hand`、`Draw`、`Deck`、`Discard` |
| `ValueProp` | `Move`（受力量影响）、`Unblockable`（无视格挡）、`Unpowered`（不受增伤影响），可用 `\|` 组合 |
| `CardTag` | `Strike`、`Defend` |

---

## 8. 本地化 `cards.json`

路径：`slay_the_spire_2_three_kingdoms/localization/zhs/cards.json`，格式为**扁平键值对**（不要改成嵌套结构，加载器按扁平键读取）。

### 键名规则

| 键 | 说明 |
| --- | --- |
| `SLAY_THE_SPIRE_2_THREE_KINGDOMS-<ID>.title` | 卡牌名 |
| `...-<ID>.description` | 卡牌描述 |
| `...-<ID>.selectionScreenPrompt` | 选牌界面的提示语（仅需要选牌的卡） |

`<ID>` 为大写下划线类名。文件中的键按 ID 字母序排列，同一张卡的 `title` 在 `description` 之前，便于查找。

### 占位符与 `CanonicalVars` 的对应

描述里的 `{X:diff()}` 会替换成 X 对应的动态数值，**键名必须与代码里声明的 Var 键一致**：

| 描述中的占位符 | 对应的代码声明 |
| --- | --- |
| `{Damage:diff()}` | `new DamageVar(6, ValueProp.Move)` → `DynamicVars.Damage` |
| `{Cards:diff()}` | `new CardsVar(3)` |
| `{Heal:diff()}` | `new HealVar(5m)` |
| `{Energy:energyIcons()}` | `new EnergyVar(1)` |
| `{CalculatedDamage:diff()}` / `{ExtraDamage:diff()}` | `CalculatedDamageVar` / `ExtraDamageVar` |
| `{VulnerablePower:diff()}` / `{WeakPower:diff()}` | `new PowerVar<VulnerablePower>(1m)` |
| `{BlockGet:diff()}` | `new BlockVar("BlockGet", 15m, ValueProp.Move)` |
| `{AttackPlayed:diff()}` | `new DynamicVar("AttackPlayed", 0m)` |
| 其余自定义键 | 同名 `DynamicVar("键名", 初值)` |

### 其它格式标记

| 标记 | 作用 |
| --- | --- |
| `[gold]文本[/gold]` | 金色高亮（本仓库用了 128 处） |
| `[blue]文本[/blue]` | 蓝色高亮 |
| `{energyPrefix:energyIcons(1)}` | 渲染 1 个能量图标 |
| `{IfUpgraded:show:A}` | 卡牌已升级时显示 A，否则不显示 |
| `{IfUpgraded:show:A+B}` | 已升级显示 A，未升级显示 B |
| `\n` | 换行 |

---

## 9. 新增一张卡牌

1. 在 `Cards/` 新建 `<类名>.cs`，照抄第 1 节的骨架，改类名与 4 个 `const`。
2. `[Pool(typeof(TkCardPool))]` 保持不变。
3. 实现 `PortraitPath`、`OnPlay`；需要数值则加 `CanonicalVars`，需要升级效果则加 `OnUpgrade`。
4. 放入图片 `images/cards/<类名>.png` 和音效 `sfx/<类名>.mp3`（缺音效时 `PlayCardSfx` 会静默跳过）。
5. 在 `localization/zhs/cards.json` 加 `.title` 与 `.description` 两条，键名按第 8 节规则。
6. 编译验证：

```bash
dotnet build slay_the_spire_2_three_kingdoms.csproj -t:Rebuild
```

编译产物会自动复制到游戏 `mods/` 目录。
