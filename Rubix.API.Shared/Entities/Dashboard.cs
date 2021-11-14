using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using Rubix.API.Shared.Entities.Base;
using Rubix.API.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rubix.API.Shared.Entities
{
    public class Dashboard : BaseEntity
    {

        [BsonElement("entityType")]
        public EntityType EntityType { get; set; }

        public ActivityFilter ActivityFilter { get; set;}

        public string Data { get; set;}

    }

    public class CardsDashboard : Dashboard
    {

    }

}
