using BaseLib.Abstracts;
using BaseLib.Utils;
using slay_the_spire_2_three_kingdoms.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using slay_the_spire_2_three_kingdoms.Cards;

namespace slay_the_spire_2_three_kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class ZhangBaSheMao : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public override string PackedIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/ZhangBaSheMao_sm.png";
    protected override string PackedIconOutlinePath => $"res://slay_the_spire_2_three_kingdoms/images/relics/ZhangBaSheMao_sm.png";
    protected override string BigIconPath => $"res://slay_the_spire_2_three_kingdoms/images/relics/ZhangBaSheMao_bg.png";
    public override async Task AfterObtained()
    {
        foreach (CardModel item in await CardSelectCmd.FromDeckForRemoval(prefs: new CardSelectorPrefs(CardSelectorPrefs.RemoveSelectionPrompt, 2), player: Owner))
        {
            await CardPileCmd.RemoveFromDeck(item);
        }
        CardModel card = Owner.RunState.CreateCard(ModelDb.Card<Sha>(), Owner);
        CardCmd.PreviewCardPileAdd(await CardPileCmd.Add(card, PileType.Deck));
    }
}
