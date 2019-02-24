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

        public int TryCount { get; set; } = 5;

        public int SleepMsec { get; set; } = 20000;

        public NeteaseTest()
        {
            this.Api = new Meting(ServerProvider.Netease);
        }

        [Fact]
        public void SearchTest()
        {
            bool isPass = false;
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                jsonStr = Api.FormatMethod(true).Search("千里邀月", new Meting4Net.Core.Models.Standard.Options
                {
                    limit = 3,
                    page = 1
                });
                if (jsonStr.Contains("千里邀月"))
                {
                    isPass = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        [Fact]
        public void UrlTest()
        {
            bool isPass = false;
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                jsonStr = Api.FormatMethod(true).Url("35847388");
                if (jsonStr.Contains("url") && jsonStr.Contains("size") && jsonStr.Contains("br"))
                {
                    isPass = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        [Fact]
        public void SongTest()
        {
            bool isPass = false;
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                if (jsonStr.Contains("id") && jsonStr.Contains("name") && jsonStr.Contains("artist") && jsonStr.Contains("album") && jsonStr.Contains("pic_id") && jsonStr.Contains("url_id") && jsonStr.Contains("lyric_id") && jsonStr.Contains("source"))
                {
                    isPass = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        [Fact]
        public void AlbumTest()
        {
            bool isPass = false;
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                jsonStr = Api.FormatMethod(true).Album("73927024");
                isPass = jsonStr.Contains("id") && jsonStr.Contains("name") && jsonStr.Contains("artist") && jsonStr.Contains("album") && jsonStr.Contains("pic_id") && jsonStr.Contains("url_id") && jsonStr.Contains("lyric_id") && jsonStr.Contains("source");
                if (isPass)
                {
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        [Fact]
        public void ArtistTest()
        {
            bool isPass = false;
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
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                jsonStr = Api.FormatMethod(true).Artist("1049179");
                if (ContainsStrArr(jsonStr, subStrArr))
                {
                    isPass = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        [Fact]
        public void PlaylistTest()
        {
            bool isPass = false;
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
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                jsonStr = Api.FormatMethod(true).Playlist("2487120533");
                if (ContainsStrArr(jsonStr, subStrArr))
                {
                    isPass = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        [Fact]
        public void LyricTest()
        {
            bool isPass = false;
            string[] subStrArr =
            {
               "lyric",
                "tlyric",
            };
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                jsonStr = Api.FormatMethod(true).Lyric("35847388");
                if (ContainsStrArr(jsonStr, subStrArr))
                {
                    isPass = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        [Fact]
        public void PicTest()
        {
            bool isPass = false;
            string[] subStrArr =
            {
               "url"
            };
            string jsonStr = string.Empty;
            for (int i = 0; i < TryCount; i++)
            {
                jsonStr = Api.FormatMethod(true).Pic("1407374890649284");
                if (ContainsStrArr(jsonStr, subStrArr))
                {
                    isPass = true;
                    break;
                }
                Thread.Sleep(SleepMsec * (i + 1));
            }

            Assert.True(isPass);
        }

        #region 指定的字符串是否包括全部的子串数组
        private bool ContainsStrArr(string searchStr, string[] subStrArr)
        {
            bool isAllContain = true;
            foreach (string subStr in subStrArr)
            {
                isAllContain = searchStr.Contains(subStr);
                // 主要有一个不包含，则退出，标记为不包含false
                if (!isAllContain)
                {
                    break;
                }
            }

            return isAllContain;
        }
        #endregion
    }
}
