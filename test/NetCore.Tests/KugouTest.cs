using Meting4Net.Core;
using Meting4Net.Core.Models.Standard;
using Xunit;

namespace NetCore.Tests
{
    public class KugouTest
    {
        public Meting Api { get; set; }

        public KugouTest()
        {
            this.Api = new Meting(ServerProvider.Kugou);
        }

        [Fact]
        public void SearchTest()
        {
            int limit = 4, page = 1;

            Music_search_item[] result = Api.SearchObj("千里邀月", new Meting4Net.Core.Models.Standard.Options
            {
                limit = limit,
                page = page
            });

            Assert.InRange(result.Length, 0, limit);
        }

        [Fact]
        public void UrlTest()
        {
            Music_url result = Api.UrlObj("e64025c53de70ba1d91aec1f8c38f1ae");

            Assert.NotNull(result);
        }

        [Fact]
        public void SongTest()
        {
            Music_search_item result = Api.SongObj("e64025c53de70ba1d91aec1f8c38f1ae");

            Assert.NotNull(result);
        }

        [Fact]
        public void AlbumTest()
        {
            Music_search_item[] result = Api.AlbumObj("17597802");

            Assert.NotNull(result);
            Assert.InRange(result.Length, 1, 30);
        }

        [Fact]
        public void ArtistTest()
        {
            int limit = 10;
            Music_search_item[] result = Api.ArtistObj("188254", limit);

            Assert.InRange(result.Length, 0, limit);
        }

        [Fact]
        public void PlaylistTest()
        {
            Music_search_item[] result = Api.PlaylistObj("547134");

            Assert.NotNull(result);
        }

        [Fact]
        public void LyricTest()
        {
            Music_lyric result = Api.LyricObj("e64025c53de70ba1d91aec1f8c38f1ae");

            Assert.NotNull(result);
        }

        [Fact]
        public void PicTest()
        {
            Music_pic result = Api.PicObj("e64025c53de70ba1d91aec1f8c38f1ae");

            Assert.NotNull(result);
        }
    }
}
