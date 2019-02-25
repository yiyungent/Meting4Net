using System;
using Xunit;

using Meting4Net.Core;
using System.Collections.Generic;
using System.Threading;

namespace NetCore.Tests
{
    public class NeteaseTest
    {
        public Meting Api { get; set; }

        public NeteaseTest()
        {
            this.Api = new Meting(ServerProvider.Netease);
        }

        [Fact]
        public void SearchTest()
        {
            string[] subStrArr = { "千里邀月" };

            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Search("千里邀月"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void UrlTest()
        {
            string[] subStrArr =
            {
                "url", "size", "br"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Url("35847388"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Song("1332662925"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Album("73927024"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Artist("1049179"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Playlist("2487120533"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void LyricTest()
        {
            string[] subStrArr =
           {
                "lyric", "tlyric"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Lyric("35847388"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void PicTest()
        {
            string[] subStrArr =
           {
               "url"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Pic("1407374890649284"), subStrArr);

            Assert.True(isPass);
        }
    }
}
