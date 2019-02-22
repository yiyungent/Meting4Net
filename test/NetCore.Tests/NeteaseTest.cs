using System;
using Xunit;

using Meting4Net.Core;
using System.Collections.Generic;

namespace NetCore.Tests
{
    public class NeteaseTest
    {
        [Fact]
        public void SearchTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = api.FormatMethod(true).Search("千里邀月");

            Assert.Contains("千里邀月", jsonStr);
        }

        [Fact]
        public void UrlTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = api.FormatMethod(true).Url("35847388");

            Assert.Contains("\"url\":", jsonStr);
            Assert.Contains("\"size\":", jsonStr);
            Assert.Contains("\"br\":", jsonStr);
        }

        [Fact]
        public void SongTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = api.FormatMethod(true).Song("35847388");

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
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = api.FormatMethod(true).Album("73927024");

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
            string jsonStr = api.FormatMethod(true).Artist("1049179");

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
            string jsonStr = api.FormatMethod(true).Playlist("2487120533");

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
            string jsonStr = api.FormatMethod(true).Lyric("35847388");

            Assert.Contains("lyric", jsonStr);
            Assert.Contains("tlyric", jsonStr);
        }

        [Fact]
        public void PicTest()
        {
            Meting api = new Meting(ServerProvider.Netease);
            string jsonStr = api.FormatMethod(true).Pic("1407374890649284");

            Assert.Contains("url", jsonStr);
        }
    }
}
