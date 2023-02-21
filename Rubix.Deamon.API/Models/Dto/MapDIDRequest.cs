namespace Rubix.Deamon.API.Models.Dto
{
    public class MapDIDRequest
    {
        public string old_did { get; set; }
        public string new_did { get; set;}
        public string peer_id { get; set; }
    }
}
