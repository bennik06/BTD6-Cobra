using static cobra.Displays;
using cobra;
using MelonLoader;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Towers;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using System.Linq;
using System.Collections.Generic;

[assembly: MelonInfo(typeof(cobra.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace cobra;
class Main : BloonsMod
{
    public class Cobra : ModTower
    {
        public override string Name => "cobra";
        public override string DisplayName => "COBRA";
        public override string Description => "The COBRA, also known as Covert Ops Battles Response Agent.";
        public override string BaseTower => "SniperMonkey";
        public override int Cost => 425;
        public override int TopPathUpgrades => 4;
        public override int MiddlePathUpgrades => 4;
        public override int BottomPathUpgrades => 0;
        public override int GetTowerIndex(List<TowerDetailsModel> towerSet) => towerSet.First(model => model.towerId == TowerType.SniperMonkey).towerIndex + 1;
        public override bool IsValidCrosspath(int[] tiers) => ModHelper.HasMod("UltimateCrosspathing") || base.IsValidCrosspath(tiers);
        public override TowerSet TowerSet => TowerSet.Military;

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<CobraBaseDisplay>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            foreach (var weaponModel in towerModel.GetWeapons())
            {
                weaponModel.ejectX = -2.5f;
                weaponModel.ejectY = 15;
                weaponModel.ejectZ = 7;
            }

            towerModel.isGlobalRange = false;
            towerModel.range = 40;
            towerModel.GetWeapon().rate = 1.2f;
            towerModel.GetAttackModel().range = towerModel.range;
        }
    }
    public class Top1 : ModUpgrade<Cobra>
    {
        public override string Name => "Top1";
        public override string DisplayName => "Wired Funds";
        public override string Description => "You gain +70 extra money per round.";
        public override int Cost => 600;
        public override int Path => TOP;
        public override int Tier => 1;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Top1Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            towerModel.AddBehavior(Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<PerRoundCashBonusTowerModel>().Duplicate());
            towerModel.GetBehavior<PerRoundCashBonusTowerModel>().cashPerRound = 70;
        }
    }

    public class Top2 : ModUpgrade<Cobra>
    {
        public override string Name => "Top2";
        public override string DisplayName => "Bloon Adjustment";
        public override string Description => "Removes regrow, camo and fortified properties from bloons.";
        public override int Cost => 1300;
        public override int Path => TOP;
        public override int Tier => 2;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Top2Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            towerModel.GetWeapon().projectile.collisionPasses = new int[] { 0, -1 };
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(new RemoveBloonModifiersModel("RemoveBloonModifiersModel", true, true, false, true, false, new Il2CppStringArray(0).ToIl2CppList()));
            towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel", true));
        }
    }

    public class Top3 : ModUpgrade<Cobra>
    {
        public override string Name => "Top3";
        public override string DisplayName => "Monkey Stim";
        public override string Description => "All nearby ability cooldowns are decreased.";
        public override int Cost => 4000;
        public override int Path => TOP;
        public override int Tier => 3;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Top3Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            AbilityCooldownScaleSupportModel buff = new("AbilityCooldownScaleSupportModel", false, 1.5f, false, false, null, "Monkey Stim", "Top3-Icon", false, 1);
            buff.ApplyBuffIcon<CobraBuffIcon>();
            towerModel.AddBehavior(buff);
        }
    }

    public class Top4 : ModUpgrade<Cobra>
    {
        public override string Name => "Top4";
        public override string DisplayName => "Offensive Push";
        public override string Description => "Sends a BFB every 20 seconds.";
        public override int Cost => 12500;
        public override int Path => TOP;
        public override int Tier => 4;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Top4Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            //credit: DatJaneDoe: BloonSpawner
            SwordChargeModel behavior = AbilityModelBehaviorExt.GetBehavior<SwordChargeModel>(ModelExt.Duplicate(TowerModelBehaviorExt.GetBehaviors<AbilityModel>(Game.instance.model.towers.First(a => a.name.Contains("Sauda 10")))[1]));

            behavior.projectileModel.display = Game.instance.model.GetBloon("Bfb").display;
            behavior.projectileModel.GetDamageModel().damage = 10;
            behavior.projectileModel.GetDamageModel().immuneBloonProperties = 0;

            ProjectileModelBehaviorExt.GetBehavior<TravelAlongPathModel>(behavior.projectileModel).speedFrames = 0.2f;
            ProjectileModelBehaviorExt.GetBehavior<TravelAlongPathModel>(behavior.projectileModel).lifespanFrames = 6000;
            ProjectileModelBehaviorExt.GetBehavior<AgeModel>(behavior.projectileModel).lifespanFrames = 6000;
            ProjectileModelBehaviorExt.GetBehavior<TravelAlongPathModel>(behavior.projectileModel).disableRotateWithPathDirection = false;
            ProjectileModelBehaviorExt.RemoveBehavior<DestroyIfTargetLostModel>(behavior.projectileModel);

            towerModel.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey").GetBehavior<AttackModel>().Duplicate());
            towerModel.GetAttackModels()[1].weapons[0].projectile = behavior.projectileModel;
            towerModel.GetAttackModels()[1].weapons[0].rate = 20;
        }
    }

    public class Middle1 : ModUpgrade<Cobra>
    {
        public override string Name => "Middle1";
        public override string DisplayName => "Double Tap";
        public override string Description => "Doubles COBRA's attack speed.";
        public override int Cost => 650;
        public override int Path => MIDDLE;
        public override int Tier => 1;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Middle1Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            towerModel.GetWeapon().rate *= 0.5f;
        }
    }

    public class Middle2 : ModUpgrade<Cobra>
    {
        public override string Name => "Middle2";
        public override string DisplayName => "Attrition";
        public override string Description => "Attrition adds 2 lives to your health every round.";
        public override int Cost => 1200;
        public override int Path => MIDDLE;
        public override int Tier => 2;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Middle2Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            towerModel.AddBehavior(Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<BonusLivesPerRoundModel>().Duplicate());
            towerModel.GetBehavior<BonusLivesPerRoundModel>().amount = 2;
        }
    }

    public class Middle3 : ModUpgrade<Cobra>
    {
        public override string Name => "Middle3";
        public override string DisplayName => "Finish Him!";
        public override string Description => "Ability: All monkeys in range shoot faster.";
        public override int Cost => 4000;
        public override int Path => MIDDLE;
        public override int Tier => 3;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Middle3Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            AbilityModel ability = Game.instance.model.GetTowerFromId("AdmiralBrickell 3").Duplicate().GetBehavior<AbilityModel>();
            ability.icon = GetSpriteReference("Middle3-Icon");
            ability.GetBehavior<ActivateRateSupportZoneModel>().filters[0] = new FilterTowerByPlaceableAreaModel("FilterTowerByPlaceableAreaModel", new AreaType[] { AreaType.water, AreaType.land, AreaType.ice });
            towerModel.AddBehavior(ability);

            towerModel.GetWeapon().rate *= 0.5f;
            towerModel.GetWeapon().projectile.GetDamageModel().damage += 3;
            towerModel.GetWeapon().projectile.GetDamageModel().immuneBloonProperties = 0;
        }
    }

    public class Middle4 : ModUpgrade<Cobra>
    {
        public override string Name => "Middle4";
        public override string DisplayName => "Misdirection";
        public override string Description => "Casually sends a bloon back to the start of the track. (Max BFB)";
        public override int Cost => 24500;
        public override int Path => MIDDLE;
        public override int Tier => 4;
        public override void ApplyUpgrade(TowerModel towerModel)
        {
            towerModel.ApplyDisplay<Middle4Display>();
            towerModel.GetBehavior<DisplayModel>().positionOffset = new Vector3(0, 0, 10);

            WindModel Knockback = Game.instance.model.GetTowerFromId("NinjaMonkey-010").GetWeapon().projectile.GetBehavior<WindModel>().Duplicate();
            Knockback.affectMoab = true;
            Knockback.chance = 0.1f;
            Knockback.distanceMin = 99999;
            Knockback.distanceMax = 99999;
            Knockback.distanceScaleForTags = 0;
            Knockback.distanceScaleForTagsTags = "Zomg";
            towerModel.GetAttackModel().weapons[0].projectile.AddBehavior(Knockback);

            towerModel.GetWeapon().projectile.AddBehavior(new DamageModifierForTagModel("DamageModifierForTagModel", "Moabs", 1, 20, false, true));
        }
    }
}
