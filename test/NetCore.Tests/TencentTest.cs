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
            string jsonStr = Api.FormatMethod(true).Search("千里邀月");

            Assert.Contains("千里邀月", jsonStr);
        }

        [Fact]
        public void UrlTest()
        {
            string jsonStr = Api.FormatMethod(true).Url("001Nal2N2f0Qr8");

            Assert.Contains("\"url\":", jsonStr);
            Assert.Contains("\"size\":", jsonStr);
            Assert.Contains("\"br\":", jsonStr);
        }

        [Fact]
        public void SongTest()
        {
            string jsonStr = Api.FormatMethod(true).Song("001Nal2N2f0Qr8");

            Assert.Contains("id", jsonStr);
            Assert.Contains("name", jsonStr);
            Assert.Contains("artist", jsonStr);
            Assert.Contains("album", jsonStr);
            Assert.Contains("pic_id", jsonStr);
            Assert.Contains("url_id", jsonStr);
            Assert.Contains("lyric_id", jsonStr);
            Assert.Contains("source", jsonStr);
        }

        [Fact]
        public void AlbumTest()
        {
            string jsonStr = Api.FormatMethod(true).Album("001AMQBG3GzakR");

            Assert.Contains("id", jsonStr);
            Assert.Contains("name", jsonStr);
            Assert.Contains("artist", jsonStr);
            Assert.Contains("album", jsonStr);
            Assert.Contains("pic_id", jsonStr);
            Assert.Contains("url_id", jsonStr);
            Assert.Contains("lyric_id", jsonStr);
            Assert.Contains("source", jsonStr);
        }

        [Fact]
        public void ArtistTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = Api.FormatMethod(true).Artist("001fNHEf1SFEFN");

            Assert.Contains("id", jsonStr);
            Assert.Contains("name", jsonStr);
            Assert.Contains("artist", jsonStr);
            Assert.Contains("album", jsonStr);
            Assert.Contains("pic_id", jsonStr);
            Assert.Contains("url_id", jsonStr);
            Assert.Contains("lyric_id", jsonStr);
            Assert.Contains("source", jsonStr);
        }

        [Fact]
        public void PlaylistTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = Api.FormatMethod(true).Playlist("1721973967");

            Assert.Contains("id", jsonStr);
            Assert.Contains("name", jsonStr);
            Assert.Contains("artist", jsonStr);
            Assert.Contains("album", jsonStr);
            Assert.Contains("pic_id", jsonStr);
            Assert.Contains("url_id", jsonStr);
            Assert.Contains("lyric_id", jsonStr);
            Assert.Contains("source", jsonStr);
        }

        [Fact]
        public void LyricTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = Api.FormatMethod(true).Lyric("001Nal2N2f0Qr8");

            Assert.Contains("lyric", jsonStr);
            Assert.Contains("tlyric", jsonStr);
        }

        [Fact]
        public void PicTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = Api.FormatMethod(true).Pic("002szRig2zZxtj");

            Assert.Contains("url", jsonStr);
        }
    }
}
