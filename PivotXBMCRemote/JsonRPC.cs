using System.Runtime.Serialization;

namespace PivotXBMCRemote
{
    [DataContract]
    public class JsonVolume
    {
        public JsonVolume() { }

        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string jsonrpc { get; set; }

        [DataMember]
        public JsonVolumeResult result { get; set; }
    }

    [DataContract]
    public class JsonVolumeResult
    {
        public JsonVolumeResult() { }
        [DataMember]
        public int volume { get; set; }
    }


    public class Playlists
    {
        public Playlists() { }
        [DataMember]
        public string id { get; set; }

        [DataMember]
        public string jsonrpc { get; set; }

        [DataMember]
        public PlaylistsResult result { get; set; }
    }
    public enum PlaylistsResult
    {
        playlistid,
        type
    }
}
