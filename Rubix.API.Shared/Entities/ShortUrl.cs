using MongoDB.Bson.Serialization.Attributes;
using Rubix.API.Shared.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{

    public class ShortUrl : BaseEntity
    {
        public ShortUrl(string code, string url) =>
           (Code, URL) = (code, url);


        [BsonElement("code")]
        public virtual string Code { get; set; }

        [BsonElement("url")]
        public virtual string URL { get; set; }
    }
}
