using System;
using System.Collections.Generic;

namespace Rubix.Deamon.API.Models.Dto
{
    public class CreateNFTTokenInput
    {
      
        public virtual string tokenType { get; set; }

       
        public virtual string creatorId { get; set; }

      
        public virtual string nftToken { get; set; }

        public virtual DateTime createdOn { get; set; }


        public virtual List<string> creatorPubKeyIpfsHash { get; set; }

        public long totalSupply { get; set; }

        public long edition { get; set; }

        public string url { get; set; }

        public string creatorInput { get; set; }
    }
}
