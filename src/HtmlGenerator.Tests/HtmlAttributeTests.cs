﻿using System;
using System.Collections.Generic;
using Xunit;

namespace HtmlGenerator.Tests
{
    public class HtmlAttributeTests
    {
        [Theory]
        [InlineData("name")]
        [InlineData("NaMe")]
        public void Ctor_String(string name)
        {
            HtmlAttribute attribute = new HtmlAttribute(name);
            Assert.Equal(name, attribute.Name);
            Assert.Null(attribute.Value);
            Assert.True(attribute.IsVoid);
        }

        [Theory]
        [InlineData("name", "")]
        [InlineData("name", " \r \t \n")]
        [InlineData("name", "value")]
        [InlineData("NaMe", "VaLuE")]
        public void Ctor_String_String(string name, string value)
        {
            HtmlAttribute attribute = new HtmlAttribute(name, value);
            Assert.Equal(name, attribute.Name);
            Assert.Equal(value, attribute.Value);
            Assert.False(attribute.IsVoid);
        }

        [Fact]
        public void Ctor_NullName_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("name", () => new HtmlAttribute(null));
            Assert.Throws<ArgumentNullException>("name", () => new HtmlAttribute(null, "value"));
        }

        [Theory]
        [InlineData("")]
        [InlineData("\t \r \n")]
        public void Ctor_WhitespaceName_ThrowsArgumentException(string name)
        {
            Assert.Throws<ArgumentException>("name", () => new HtmlAttribute(name));
            Assert.Throws<ArgumentException>("name", () => new HtmlAttribute(name, "value"));
        }

        [Fact]
        public void Ctor_NullValue_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("value", () => new HtmlAttribute("name", null));
        }

        [Fact]
        public void RemoveFromParent_FirstChild()
        {
            HtmlAttribute attribute1 = new HtmlAttribute("Attribute1");
            HtmlAttribute attribute2 = new HtmlAttribute("Attribute2");
            HtmlAttribute attribute3 = new HtmlAttribute("Attribute3");
            HtmlElement element = new HtmlElement("div", attribute1, attribute2, attribute3);

            attribute1.RemoveFromParent();
            Assert.Null(attribute1.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute2, attribute3 }, element.Attributes());
        }

        [Fact]
        public void RemoveFromParent_LastChild()
        {
            HtmlAttribute attribute1 = new HtmlAttribute("Attribute1");
            HtmlAttribute attribute2 = new HtmlAttribute("Attribute2");
            HtmlAttribute attribute3 = new HtmlAttribute("Attribute3");
            HtmlElement element = new HtmlElement("div", attribute1, attribute2, attribute3);

            attribute3.RemoveFromParent();
            Assert.Null(attribute3.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute1, attribute2 }, element.Attributes());
        }

        [Fact]
        public void RemoveFromParent_MiddleChild()
        {
            HtmlAttribute attribute1 = new HtmlAttribute("Attribute1");
            HtmlAttribute attribute2 = new HtmlAttribute("Attribute2");
            HtmlAttribute attribute3 = new HtmlAttribute("Attribute3");
            HtmlElement element = new HtmlElement("div", attribute1, attribute2, attribute3);

            attribute2.RemoveFromParent();
            Assert.Null(attribute2.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute1, attribute3 }, element.Attributes());
        }

        [Fact]
        public void RemoveFromParent_NoParent_DoesNothing()
        {
            HtmlAttribute attribute = new HtmlAttribute("Attribute");
            attribute.RemoveFromParent();

            Assert.Null(attribute.Parent);
        }

        public static IEnumerable<object[]> ToString_TestData()
        {
            yield return new object[] { new HtmlAttribute("name"), "name" };
            yield return new object[] { new HtmlAttribute("name", "value"), "name=\"value\"" };
        }

        [Theory]
        [MemberData(nameof(ToString_TestData))]
        public void ToString_ReturnsExpected(HtmlAttribute attribute, string expected)
        {
            Assert.Equal(expected, attribute.ToString());
            Assert.Equal(expected, attribute.Serialize());
            Assert.Equal(expected, attribute.Serialize(HtmlSerializeOptions.None));
            Assert.Equal(expected, attribute.Serialize(HtmlSerializeOptions.NoFormatting));
        }
    }
}
