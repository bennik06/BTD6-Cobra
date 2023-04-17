using BTD_Mod_Helper.Api.Display;
using Il2CppAssets.Scripts.Unity.Display;

namespace cobra
{
    internal class Displays
    {
        public class CobraBaseDisplay : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            //public override bool UseForTower(int[] tiers)
            //{
            //    return tiers.Sum() == 0;
            //}
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Top1Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            //public override bool UseForTower(int[] tiers)
            //{
            //    return tiers[1] <= 1 && tiers[0] == 1;
            //}
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Top2Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            //public override bool UseForTower(int[] tiers)
            //{
            //    return tiers[1] <= 2 && tiers[0] == 2;
            //}
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Top3Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Top4Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Middle1Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Middle2Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Middle3Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class Middle4Display : ModDisplay
        {
            public override string BaseDisplay => "9dccc16d26c1c8a45b129e2a8cbd17ba";
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                Set2DTexture(node, Name);
                node.towerPlacementPreCalcOffset = new UnityEngine.Vector3(0, 0.1f, 0);
            }
        }

        public class CobraBuffIcon : ModBuffIcon
        {
            protected override int Order => 1;
            public override string Icon => "Top3-Icon";
            public override int MaxStackSize => 1;
        }
    }
}