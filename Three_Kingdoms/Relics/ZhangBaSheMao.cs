using BaseLib.Abstracts;
using BaseLib.Utils;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using Three_Kingdoms.Cards.Basic;

namespace Three_Kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class ZhangBaSheMao : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Common;
    public override string PackedIconPath => $"res://Three_Kingdoms/images/relics/ZhangBaSheMao_sm.png";
    protected override string PackedIconOutlinePath => $"res://Three_Kingdoms/images/relics/ZhangBaSheMao_sm.png";
    protected override string BigIconPath => $"res://Three_Kingdoms/images/relics/ZhangBaSheMao_bg.png";
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
