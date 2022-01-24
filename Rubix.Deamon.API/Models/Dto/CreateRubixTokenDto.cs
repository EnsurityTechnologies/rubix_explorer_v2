using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Deamon.API.Models.Dto
{
    public class CreateRubixTokenDto
    {
        public List<string> token_id { get; set; }
        public string bank_id { get; set; }
        public double denomination { get; set; }
        public string user_did { get; set; }
        public string level { get; set; }
    }
}
