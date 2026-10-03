using BaseLib.Abstracts;
using BaseLib.Utils;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.ValueProps;
using Three_Kingdoms.Cards.Basic;

namespace Three_Kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class InitRelicOne : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("CardDraw", 0m),
        new DynamicVar("HandLimit", 4m),
        new DynamicVar("AutoPlay", 0m)
    ];
    public override string PackedIconPath => $"res://Three_Kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string PackedIconOutlinePath => $"res://Three_Kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string BigIconPath => $"res://Three_Kingdoms/images/relics/InitRelicOne_bg.png";

    public async Task AddHandLimit(int n)
    {
        DynamicVars["HandLimit"].BaseValue += n;
        if (DynamicVars["HandLimit"].BaseValue > 10)
        {
            DynamicVars["HandLimit"].BaseValue = 10;
        }
    }

    public override async Task BeforeFlushLate(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner)
        {
            return;
        }
        int cardsnum = 0;
        if (player.PlayerCombatState != null)
        {
            cardsnum = player.PlayerCombatState.Hand.Cards.Count;
        }
        if (cardsnum <= DynamicVars["HandLimit"].BaseValue)
        {
            return;
        }
        cardsnum -= (int)DynamicVars["HandLimit"].BaseValue;
        DynamicVars["CardDraw"].UpgradeValueBy(cardsnum);
        List<CardModel> list = (await CardSelectCmd.FromHand(prefs: new CardSelectorPrefs(SelectionScreenPrompt, cardsnum, cardsnum), context: choiceContext, player: player, filter: null, source: this)).ToList();
        await CardCmd.Discard(choiceContext, list);
        DynamicVars["CardDraw"].UpgradeValueBy(-cardsnum);
    }

    public override async Task AfterRoomEntered(AbstractRoom room)
    {
        if (room is CombatRoom)
        {
            DynamicVars["HandLimit"].BaseValue = 4;
        }
    }

    public override bool ShouldFlush(Player player)
    {
        if (player != Owner)
        {
            return true;
        }
        return false;
    }

    public override decimal ModifyHandDraw(Player player, decimal count)
    {
        if (player != Owner)
        {
            return count;
        }
        if (player.Creature.CombatState != null && player.Creature.CombatState.RoundNumber > 1)
        {
            return count - 3;
        }
        return count + 1;
    }

    public override decimal ModifyHpLostAfterOstyLate(Creature target, decimal amount, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return amount;
        }
        if (target != Owner.Creature || Owner.Creature.Player == null || cardSource?.Owner == Owner || dealer == Owner.Creature)
        {
            return amount;
        }
        if (amount < target.Block || amount == 0)
        {
            return amount;
        }
        bool hasShan = PileType.Hand.GetPile(Owner.Creature.Player).Cards.Any((CardModel c) => c is Shan);
        if (hasShan)
        {
            DynamicVars["AutoPlay"].BaseValue = 1;
            return 0;
        }
        return amount;
    }

    public override async Task AfterDamageReceived(PlayerChoiceContext choiceContext, Creature target, DamageResult result, ValueProp props, Creature? dealer, CardModel? cardSource)
    {
        if (!CombatManager.Instance.IsInProgress)
        {
            return;
        }
        if (target != Owner.Creature || Owner.Creature.Player == null || cardSource?.Owner == Owner || dealer == Owner.Creature)
        {
            return;
        }
        if (DynamicVars["AutoPlay"].BaseValue != 1)
        {
            return;
        }
        foreach (CardModel item in PileType.Hand.GetPile(Owner.Creature.Player).Cards.Where((CardModel c) => c is Shan))
        {
            DynamicVars["AutoPlay"].BaseValue = 0;
            await CardCmd.AutoPlay(choiceContext, item, null);
            break;
        }
    }

    public override bool ShouldClearBlock(Creature creature)
    {
        if (Owner.Creature != creature)
        {
            return true;
        }
        return false;
    }
}
