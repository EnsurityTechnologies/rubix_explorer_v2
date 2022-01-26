using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Dto
{
    public class TopWalletsDto
    {
        public string DIDOrWalletID { get; set; }
        public long Balance { get; set; }
    }
}
