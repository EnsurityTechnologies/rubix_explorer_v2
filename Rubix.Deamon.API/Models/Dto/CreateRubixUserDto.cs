using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Deamon.API.Models.Dto
{
    public class CreateRubixUserDto
    {
        public string user_did { get; set; }
        public string peerid { get; set; }
        public string ipaddress { get; set; }
        public int balance { get; set; }
    }
}
