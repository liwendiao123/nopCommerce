using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nop.Web.Models.Api
{

    public class TestModel
    {
       [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public string desc { get; set; }
    }
}
