using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rubix.Explorer.API.Dtos
{
    public class UserInfoDto
    {
        public virtual string user_did { get; set; }
        public virtual string peerid { get; set; }
        public virtual string ipaddress { get; set; }
        public virtual int balance { get; set; }
    }
}
