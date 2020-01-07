using NUnit.Framework;
using VCRevitRibbonUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCRevitRibbonUtil.Tests
{
    [TestFixture()]
    public class LineBreaksTests
    {
        [Test()]
        public void FormatTestShortWord()
        {
            string str1 = "this is a loooong str";
            var str1_result = LineBreaks.Format(str1);
            Assert.AreEqual(str1, str1_result);
        }

        [Test()]
        public void FormatTestMoreLines()
        {
            string str2 = "this is the longest string string string string string";
            string str2_check = $"this is the longest\nstring string string\nstring string";
            var str2_result = LineBreaks.Format(str2);
            Assert.AreEqual(str2_check, str2_result);
        }

        [Test()]
        public void FormatTestArticle()
        {
            string str3 = "this is the string with an article an article an article an article an article";
            string str3_check = $"this is the string\nwith an article\nan article an article\nan article an article";
            var str3_result = LineBreaks.Format(str3);
            Assert.AreEqual(str3_check, str3_result);
        }

        [Test()]
        public void FormatTest()
        {
            string str = "longwordlongword1 longwordlongword2";
            string str_check = $"longwordlongword1\nlongwordlongword2";
            var str_result = LineBreaks.Format(str);
            Assert.AreEqual(str_check, str_result);
        }
    }
}