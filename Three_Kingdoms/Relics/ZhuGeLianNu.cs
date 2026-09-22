using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Three_Kingdoms.Cards.Basic;
using Three_Kingdoms.Cards.Token;

namespace Three_Kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class ZhuGeLianNu : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;
    public override string PackedIconPath => $"res://Three_Kingdoms/images/relics/ZhuGeLianNu_sm.png";
    protected override string PackedIconOutlinePath => $"res://Three_Kingdoms/images/relics/ZhuGeLianNu_sm.png";
    protected override string BigIconPath => $"res://Three_Kingdoms/images/relics/ZhuGeLianNu_bg.png";
    public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
    {
        if (cardPlay.Card.Owner == Owner && (cardPlay.Card is Sha || cardPlay.Card is HuoSha))
        {
            await PlayerCmd.GainEnergy(1m, Owner);
        }
    }
}
