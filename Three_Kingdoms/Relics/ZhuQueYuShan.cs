using BaseLib.Abstracts;
using BaseLib.Utils;
using Three_Kingdoms.Character;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using Three_Kingdoms.Cards.Basic;
using Three_Kingdoms.Cards.Token;

namespace Three_Kingdoms.Relics;

[Pool(typeof(TkRelicPool))]
public class ZhuQueYuShan : CustomRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Uncommon;
    public override string PackedIconPath => $"res://Three_Kingdoms/images/relics/ZhuQueYuShan_sm.png";
    protected override string PackedIconOutlinePath => $"res://Three_Kingdoms/images/relics/ZhuQueYuShan_sm.png";
    protected override string BigIconPath => $"res://Three_Kingdoms/images/relics/ZhuQueYuShan_bg.png";
    public override async Task AfterObtained()
    {
        List<CardModel> source = PileType.Deck.GetPile(Owner).Cards.Where((CardModel c) => c != null && c is Sha && c.IsRemovable).ToList();
        List<CardTransformation> transformations = source.Select((CardModel original) => new CardTransformation(original, CreateShaFromOriginal(original, forPreview: false))).ToList();
        await CardCmd.Transform(transformations, Owner.PlayerRng.Transformations);
    }
    private CardModel CreateShaFromOriginal(CardModel original, bool forPreview)
    {
        CardModel cardModel = forPreview ? ModelDb.Card<HuoSha>().ToMutable() : Owner.RunState.CreateCard<HuoSha>(Owner);
        if (original.IsUpgraded && cardModel.IsUpgradable)
        {
            if (forPreview)
            {
                cardModel.UpgradeInternal();
            }
            else
            {
                CardCmd.Upgrade(cardModel);
            }
        }
        if (original.Enchantment != null)
        {
            EnchantmentModel enchantmentModel = (EnchantmentModel)original.Enchantment.MutableClone();
            if (enchantmentModel.CanEnchant(cardModel))
            {
                if (forPreview)
                {
                    cardModel.EnchantInternal(enchantmentModel, enchantmentModel.Amount);
                    enchantmentModel.ModifyCard();
                }
                else
                {
                    CardCmd.Enchant(enchantmentModel, cardModel, enchantmentModel.Amount);
                }
            }
        }
        return cardModel;
    }
}
