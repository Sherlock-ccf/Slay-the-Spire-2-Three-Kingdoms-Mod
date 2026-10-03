using BaseLib.Abstracts;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Entities.Characters;
using Godot;
using Three_Kingdoms.Cards.Basic;
using Three_Kingdoms.Relics;
using System.Diagnostics.CodeAnalysis;
namespace Three_Kingdoms.Character;

public class TkCharacter : PlaceholderCharacterModel
{
    // ��ɫ������ɫ
    public override Color NameColor => new(1f, 1f, 0.6f);
    // ����ͼ��������ɫ
    public override Color EnergyLabelOutlineColor => new(0.8f, 0.7f, 0f);

    // �����Ա���Ů������
    public override CharacterGender Gender => CharacterGender.Masculine;

    // ��ʼѪ��
    public override int StartingHp => 70;

    // ����ģ��tscn·����Ҫ�Զ�����¡�
    public override string CustomVisualPath => "res://Three_Kingdoms/scenes/Tk_character.tscn";

    // ������β������
    // public override string CustomTrailPath => "res://scenes/vfx/card_trail_ironclad.tscn";

    // ����ͷ��·����
    public override string CustomIconTexturePath => "res://icon.svg";

    // ����ͷ��2�š�
    public override string CustomIconPath => "res://Three_Kingdoms/scenes/Tk_icon.tscn";

    // ��������tscn·����Ҫ�Զ�����¡�
    public override string CustomEnergyCounterPath => "res://Three_Kingdoms/scenes/Tk_energy_counter.tscn";

    // ������Ϣ������**
    public override string CustomRestSiteAnimPath => "res://Three_Kingdoms/scenes/Tk_rest_site.tscn";

    // �̵����ﳡ����**
    public override string CustomMerchantAnimPath => "res://Three_Kingdoms/scenes/Tk_merchant.tscn";

    // ����ģʽ-��ָ��
    // public override string CustomArmPointingTexturePath => null;
    // ����ģʽ����ʯͷ��-ʯͷ��
    // public override string CustomArmRockTexturePath => null;
    // ����ģʽ����ʯͷ��-����
    // public override string CustomArmPaperTexturePath => null;
    // ����ģʽ����ʯͷ��-������
    // public override string CustomArmScissorsTexturePath => null;

    // ����ѡ�񱳾���
    public override string CustomCharacterSelectBg => "res://Three_Kingdoms/scenes/Tk_bg.tscn";
    // ����ѡ��ͼ�ꡣ
    public override string CustomCharacterSelectIconPath => "res://Three_Kingdoms/images/select/char_select_Tk.png";
    // ����ѡ��ͼ��-����״̬��
    public override string CustomCharacterSelectLockedIconPath => "res://Three_Kingdoms/images/select/char_select_Tk_locked.png";

    // ����ѡ����ɶ�����
    // public override string CustomCharacterSelectTransitionPath => "res://materials/transitions/ironclad_transition_mat.tres";

    // ��ͼ�ϵĽ�ɫ���ͼ�ꡢ���������ϵĽ�ɫͷ��**
    // public override string CustomMapMarkerPath => null;

    // ������Ч
    // public override string CustomAttackSfx => null;
    // ʩ����Ч
    // public override string CustomCastSfx => null;
    // ������Ч
    // public override string CustomDeathSfx => null;
    // ��ɫѡ����Ч
    // public override string CharacterSelectSfx => null;
    // ������Ч���������ɾ��
    public override string CharacterTransitionSfx => "event:/sfx/ui/wipe_ironclad";

    public override CardPoolModel CardPool => ModelDb.CardPool<TkCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<TkRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<TkPotionPool>();
    public override IEnumerable<CardModel> StartingDeck => [
        ModelDb.Card<Sha>(),
        ModelDb.Card<Sha>(),
        ModelDb.Card<Sha>(),
        ModelDb.Card<Sha>(),
        ModelDb.Card<Sha>(),
        ModelDb.Card<Shan>(),
        ModelDb.Card<Shan>(),
        ModelDb.Card<Shan>(),
        ModelDb.Card<Shan>(),
        ModelDb.Card<Tao>(),
        ModelDb.Card<Jiu>(),
    ];
    public override IReadOnlyList<RelicModel> StartingRelics => [
        ModelDb.Relic<InitRelicOne>()
    ];
    public override List<string> GetArchitectAttackVfx() => [
        "vfx/vfx_attack_blunt",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_attack_slash",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_rock_shatter"
    ];
}