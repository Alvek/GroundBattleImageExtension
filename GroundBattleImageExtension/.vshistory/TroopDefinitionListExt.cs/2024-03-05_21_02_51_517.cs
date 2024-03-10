using DistantWorlds.Types;
using Stride.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DW2TroopsGroundBattleImage
{
    public class TroopDefinitionExt
    {
        private TroopDefinition _troopDefinitionsOrig;
        private Texture _groundImage;
        public TroopDefinitionExt(TroopDefinition troopDefinitions)
        {
            _troopDefinitionsOrig = troopDefinitions;
        }

        public Texture GetGroundImage()
        {
            return _groundImage;
        }
        public void GetGroundImage(Texture img)
        {
            _groundImage = img;
        }
    }
}
