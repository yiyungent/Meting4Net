using Meting4Net.Core;
using Meting4Net.Core.Models.Standard;
using Xunit;

namespace NetCore.Tests
{
    public class XiamiTest
    {
        public Meting Api { get; set; }

        public XiamiTest()
        {
            this.Api = new Meting(ServerProvider.Xiami);
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
            string[] subStrArr =
            {
                "url", "size", "br"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Url("1808486366"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void SongTest()
        {
            string[] subStrArr =
            {
                "id",
                "name",
                "artist",
                "album",
                "pic_id",
                "url_id",
                "lyric_id",
                "source",
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Song("1808486366"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void AlbumTest()
        {
            string[] subStrArr =
            {
                "id",
                "name",
                "artist",
                "album",
                "pic_id",
                "url_id",
                "lyric_id",
                "source",
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Album("2104586331"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void ArtistTest()
        {
            string[] subStrArr =
            {
                "id",
                "name",
                "artist",
                "album",
                "pic_id",
                "url_id",
                "lyric_id",
                "source",
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Artist("593402077"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void PlaylistTest()
        {
            string[] subStrArr =
            {
                "id",
                "name",
                "artist",
                "album",
                "pic_id",
                "url_id",
                "lyric_id",
                "source",
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Playlist("632580584"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void LyricTest()
        {
            string[] subStrArr =
            {
                "lyric", "tlyric"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Lyric("1768955559"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void PicTest()
        {
            string[] subStrArr =
            {
               "url"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Pic("1772326454"), subStrArr);

            Assert.True(isPass);
        }
    }
}
