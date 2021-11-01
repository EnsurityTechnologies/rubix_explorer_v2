using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Deamon.API.Models
{
    public class RubixCommonInput
    {
        [JsonProperty("inputString")]
        public string InputString { get; set; }
    }
}
