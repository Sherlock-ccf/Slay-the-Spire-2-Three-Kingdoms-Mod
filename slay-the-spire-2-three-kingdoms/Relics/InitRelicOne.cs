using BaseLib.Abstracts;
using BaseLib.Utils;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Models;

namespace slay_the_spire_2_three_kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class InitRelicOne : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Starter;
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DynamicVar("CardDraw", 0m),
        new DynamicVar("HandLimit",4m)
    ];
    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_sm.png";
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/InitRelicOne_bg.png";
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
}
