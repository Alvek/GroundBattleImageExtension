using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DW2TroopsGroundBattleImage.GroundImages
{
    [DataContract]
    public class ImageSettings
    {
        [DataMember]
        public string TroopDefinitonId { get; set; }
        [DataMember]
        public string GroundImageFileName { get; set; }
    }
}
