using DistantWorlds.Types;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DW2TroopsGroundBattleImage
{
    public class TroopDefinitionExt : TroopDefinition
    {
        private Texture? _groundImage;
        public TroopDefinitionExt(TroopDefinition orig) : base()
        {
            this.AttackStrength = orig.AttackStrength;
            this.DefendStrength = orig.DefendStrength;
            this.EvasionArmor = orig.EvasionArmor;
            this.EvasionInfantry = orig.EvasionInfantry;
            this.EvasionPlanetaryDefense = orig.EvasionPlanetaryDefense;
            this.EvasionSpecialForces = orig.EvasionSpecialForces;
            this.EvasionTitan = orig.EvasionTitan;
            this.InterceptWeaponComponentId = orig.InterceptWeaponComponentId;
            this.IsRobotic = orig.IsRobotic;
            this.MaintenanceCost = orig.MaintenanceCost;
            this.PsychicDefense = orig.PsychicDefense;
            this.PsychicStrength = orig.PsychicStrength;
            this.RaceId = orig.RaceId;
            this.RecruitmentCost = orig.RecruitmentCost;
            this.SabotageStrength = orig.SabotageStrength;
            this.Size = orig.Size;
            this.TroopDefinitionId = orig.TroopDefinitionId;

            this.ImageFilename = orig.ImageFilename;
            this.Description = orig.Description;
            this.Name = orig.Name;
            this.Type = orig.Type;
        }

        public Texture GetGroundImage()
        {
            return _groundImage ?? GetImage();
        }
        public void SetGroundImage(Texture img)
        {
            _groundImage = img;
        }


    }
}
