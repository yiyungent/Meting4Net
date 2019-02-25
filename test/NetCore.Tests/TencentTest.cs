using System;
using System.Collections.Generic;
using System.Text;

using Xunit;
using Meting4Net.Core;

namespace NetCore.Tests
{
    public class TencentTest
    {
        public Meting Api { get; set; }

        public TencentTest()
        {
            this.Api = new Meting(ServerProvider.Tencent);
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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Url("001Nal2N2f0Qr8"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Song("001Nal2N2f0Qr8"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Album("001AMQBG3GzakR"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Artist("001fNHEf1SFEFN"), subStrArr);

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
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Playlist("1721973967"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void LyricTest()
        {
            string[] subStrArr =
            {
                "lyric", "tlyric"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Lyric("001Nal2N2f0Qr8"), subStrArr);

            Assert.True(isPass);
        }

        [Fact]
        public void PicTest()
        {
            string[] subStrArr =
            {
               "url"
            };
            bool isPass = TestTool.TargetStrContain(() => Api.FormatMethod(true).Pic("002szRig2zZxtj"), subStrArr);

            Assert.True(isPass);
        }
    }
}
