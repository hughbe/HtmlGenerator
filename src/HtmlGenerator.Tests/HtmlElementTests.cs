﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HtmlGenerator.Tests
{
    public class HtmlElementTests
    {
        [Theory]
        [InlineData("html")]
        [InlineData("HtMl")]
        [InlineData("no-such-tag")]
        public static void Ctor_String(string tag)
        {
            HtmlElement element = new HtmlElement(tag);
            Assert.Equal(tag, element.Tag);
            Assert.Null(element.InnerText);
            Assert.False(element.IsVoid);
            Assert.Empty(element.Elements());
            Assert.Empty(element.Attributes());
            Assert.Empty(element.ElementsAndAttributes());
        }

        [Theory]
        [InlineData("html", true)]
        [InlineData("HtMl", false)]
        [InlineData("no-such-tag", false)]
        public static void Ctor_String_Bool(string tag, bool isVoid)
        {
            HtmlElement element = new HtmlElement(tag, isVoid);
            Assert.Equal(tag, element.Tag);
            Assert.Equal(isVoid, element.IsVoid);
            Assert.Empty(element.Elements());
            Assert.Empty(element.Attributes());
            Assert.Empty(element.ElementsAndAttributes());
        }
        
        [Theory]
        [InlineData("html", "inner-text")]
        [InlineData("HtMl", " \t \r \n ")]
        [InlineData("no-such-tag", "")]
        [InlineData("no-such-tag", null)]
        public static void Ctor_String_String(string tag, string innerText)
        {
            HtmlElement element = new HtmlElement(tag, innerText);
            Assert.Equal(tag, element.Tag);
            Assert.Equal(innerText, element.InnerText);
            Assert.False(element.IsVoid);
            Assert.Empty(element.Elements());
            Assert.Empty(element.Attributes());
            Assert.Empty(element.ElementsAndAttributes());
        }

        public static IEnumerable<object[]> Objects_TestData()
        {
            yield return new object[] { new HtmlObject[0] };
            yield return new object[] { new HtmlObject[] { new HtmlAttribute("Attribute1") } };
            yield return new object[] { new HtmlObject[] { new HtmlElement("h1") } };
            yield return new object[] { new HtmlObject[] { new HtmlAttribute("Attribute1"), new HtmlElement("h1") } };
            yield return new object[] { new HtmlObject[] { new HtmlAttribute("Attribute1"), new HtmlElement("h1"), new HtmlElement("h1"), new HtmlAttribute("Attribute1") } };
        }

        [Theory]
        [MemberData(nameof(Objects_TestData))]
        public void Ctor_String_ParamsHtmlObject(HtmlObject[] content)
        {
            HtmlElement element = new HtmlElement("html", content);
            Assert.Equal("html", element.Tag);
            Assert.Null(element.InnerText);
            Assert.False(element.IsVoid);
            Assert.Equal(content.Where(obj => obj is HtmlElement).Cast<HtmlElement>().ToArray(), element.Elements().ToArray());
            Assert.Equal(content.Where(obj => obj is HtmlAttribute).Cast<HtmlAttribute>().ToArray(), element.Attributes().ToArray());
            Assert.Equal(content.Length, element.ElementsAndAttributes().Count());
        }

        [Theory]
        [MemberData(nameof(Objects_TestData))]
        public void Ctor_String_String_ParamsHtmlObject(HtmlObject[] content)
        {
            HtmlElement element = new HtmlElement("html", "inner-text", content);
            Assert.Equal("html", element.Tag);
            Assert.Equal("inner-text", element.InnerText);
            Assert.False(element.IsVoid);
            Assert.Equal(content.Where(obj => obj is HtmlElement).Cast<HtmlElement>().ToArray(), element.Elements().ToArray());
            Assert.Equal(content.Where(obj => obj is HtmlAttribute).Cast<HtmlAttribute>().ToArray(), element.Attributes().ToArray());
            Assert.Equal(content.Length, element.ElementsAndAttributes().Count());
        }

        [Fact]
        public void HtmlObjectType_Get_ReturnsElement()
        {
            HtmlElement attribute = new HtmlElement("div");
            Assert.Equal(HtmlObjectType.Element, attribute.ObjectType);
        }

        public static IEnumerable<object[]> Attributes_TestData()
        {
            yield return new object[] { new HtmlAttribute[0] };
            yield return new object[] { new HtmlAttribute[] { new HtmlAttribute("Attribute1") } };
            yield return new object[] { new HtmlAttribute[] { new HtmlAttribute("Attribute1"), new HtmlAttribute("Attribute2") } };
        }

        [Theory]
        [MemberData(nameof(Attributes_TestData))]
        public void Attributes(HtmlAttribute[] attributes)
        {
            HtmlElement element = new HtmlElement("html", attributes);
            Assert.Equal(attributes, element.Attributes().ToArray());
        }

        public static IEnumerable<object[]> TryGetElement_TestData()
        {
            HtmlElement[] count0 = new HtmlElement[0];
            HtmlElement[] count1 = new HtmlElement[] { new HtmlElement("Element1") };
            HtmlElement[] count2 = new HtmlElement[] { new HtmlElement("Element1"), new HtmlElement("Element2") };

            // Element exists
            yield return new object[] { count1, "Element1", count1[0] };
            yield return new object[] { count2, "Element1", count2[0] };
            yield return new object[] { count2, "Element2", count2[1] };

            // No such Element
            yield return new object[] { count0, "No-Such-Element", null };
            yield return new object[] { count1, "No-Such-Element", null };
            yield return new object[] { count2, "No-Such-Element", null };
        }

        [Theory]
        [MemberData(nameof(TryGetElement_TestData))]
        public void TryGetElement(HtmlElement[] elements, string name, HtmlElement expected)
        {
            HtmlElement parent = new HtmlElement("html", elements);

            HtmlElement element;
            Assert.Equal(expected != null, parent.TryGetElement(name, out element));
            Assert.Equal(expected, element);
        }

        public static IEnumerable<object[]> TryGetAttribute_TestData()
        {
            HtmlAttribute[] count0 = new HtmlAttribute[0];
            HtmlAttribute[] count1 = new HtmlAttribute[] { new HtmlAttribute("Attribute1") };
            HtmlAttribute[] count2 = new HtmlAttribute[] { new HtmlAttribute("Attribute1"), new HtmlAttribute("Attribute2") };

            // Attribute exists
            yield return new object[] { count1, "Attribute1", count1[0] };
            yield return new object[] { count2, "Attribute1", count2[0] };
            yield return new object[] { count2, "Attribute2", count2[1] };

            // No such attribute
            yield return new object[] { count0, "No-Such-Attribute", null };
            yield return new object[] { count1, "No-Such-Attribute", null };
            yield return new object[] { count2, "No-Such-Attribute", null };
        }

        [Theory]
        [MemberData(nameof(TryGetAttribute_TestData))]
        public void TryGetAttribute(HtmlAttribute[] attributes, string name, HtmlAttribute expected)
        {
            HtmlElement parent = new HtmlElement("html", attributes);

            HtmlAttribute attribute;
            Assert.Equal(expected != null, parent.TryGetAttribute(name, out attribute));
            Assert.Equal(expected, attribute);
        }

        [Fact]
        public void TryGetAttribute_NullName_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlAttribute attribute = null;
            Assert.Throws<ArgumentNullException>("name", () => element.TryGetAttribute(null, out attribute));
            Assert.Null(attribute);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" \r \t \n")]
        public void TryGetAttribute_WhitespaceName_ThrowsArgumentException(string name)
        {
            HtmlElement element = new HtmlElement("html");
            HtmlAttribute attribute = null;
            Assert.Throws<ArgumentNullException>("name", () => element.TryGetAttribute(null, out attribute));
            Assert.Null(attribute);
        }

        public static IEnumerable<object[]> Elements_Method_TestData()
        {
            HtmlElement[] count0 = new HtmlElement[0];
            HtmlElement[] count1 = new HtmlElement[] { new HtmlElement("h1") };
            HtmlElement[] count2 = new HtmlElement[] { new HtmlElement("h1"), new HtmlElement("h2") };
            HtmlElement[] count3 = new HtmlElement[] { new HtmlElement("h1"), new HtmlElement("h2"), new HtmlElement("h1") };

            // No name
            yield return new object[] { count0, null, count0 };
            yield return new object[] { count1, null, count1 };
            yield return new object[] { count2, null, count2 };

            // Element exists
            yield return new object[] { count1, "h1", new HtmlElement[] { count1[0] } };
            yield return new object[] { count2, "h1", new HtmlElement[] { count2[0] } };
            yield return new object[] { count2, "h2", new HtmlElement[] { count2[1] } };
            yield return new object[] { count3, "h1", new HtmlElement[] { count3[0], count3[2] } };

            // Element does not exist
            yield return new object[] { count0, "No-Such-Element-Tag", new HtmlElement[0] };
            yield return new object[] { count1, "No-Such-Element-Tag", new HtmlElement[0] };
            yield return new object[] { count2, "No-Such-Element-Tag", new HtmlElement[0] };
            yield return new object[] { count2, "No-Such-Element-Tag", new HtmlElement[0] };
        }

        [Theory]
        [MemberData(nameof(Elements_Method_TestData))]
        public void Elements(HtmlElement[] elements, string tag, HtmlElement[] expected)
        {
            HtmlElement element = new HtmlElement("html", elements);
            if (tag == null)
            {
                Assert.Equal(expected, element.Elements().ToArray());
            }
            Assert.Equal(expected, element.Elements(tag).ToArray());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Add_HtmlObject(bool isVoid)
        {
            HtmlElement element = new HtmlElement("html", isVoid);
            HtmlAttribute attribute = new HtmlAttribute("Attribute1");
            element.Add(attribute);
            Assert.Equal(element, attribute.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute }, element.Attributes());

            if (!isVoid)
            {
                HtmlElement newElement = new HtmlElement("body");
                element.Add(newElement);
                Assert.Equal(element, newElement.Parent);
                Assert.Equal(new HtmlElement[] { newElement }, element.Elements());
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddFirst_HtmlObject(bool isVoid)
        {
            HtmlElement element = new HtmlElement("html", isVoid);
            HtmlAttribute attribute1 = new HtmlAttribute("Attribute1");
            element.Add(attribute1);

            HtmlAttribute attribute2 = new HtmlAttribute("Attribute2");
            element.AddFirst(attribute2);
            Assert.Equal(element, attribute2.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute2, attribute1 }, element.Attributes());

            if (!isVoid)
            {
                HtmlElement newElement1 = new HtmlElement("body");
                element.Add(newElement1);

                HtmlElement newElement2 = new HtmlElement("head");
                element.AddFirst(newElement2);
                Assert.Equal(element, newElement2.Parent);
                Assert.Equal(new HtmlElement[] { newElement2, newElement1 }, element.Elements());
            }
        }

        [Fact]
        public void Add_ParamsHtmlObject_NonVoidElement()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlElement newElement = new HtmlElement("body");
            HtmlAttribute newAttribute = new HtmlAttribute("Attribute");
            element.Add(new HtmlObject[] { newElement, newAttribute });

            Assert.Equal(element, newElement.Parent);
            Assert.Equal(element, newAttribute.Parent);
            Assert.Equal(new HtmlObject[] { newElement }, element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute }, element.Attributes());
        }
        
        [Fact]
        public void AddFirst_ParamsHtmlObject_NonVoidElement()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlElement newElement1 = new HtmlElement("body");
            HtmlAttribute newAttribute1 = new HtmlAttribute("Attribute1");
            element.Add(new HtmlObject[] { newElement1, newAttribute1 });

            HtmlElement newElement2 = new HtmlElement("head");
            HtmlAttribute newAttribute2 = new HtmlAttribute("Attribute2");
            element.AddFirst(new HtmlObject[] { newElement2, newAttribute2 });

            Assert.Equal(element, newElement2.Parent);
            Assert.Equal(element, newAttribute2.Parent);
            Assert.Equal(new HtmlObject[] { newElement2, newElement1 }, element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute2, newAttribute1 }, element.Attributes());
        }

        [Fact]
        public void Add_ParamsHtmlObject_VoidElement()
        {
            HtmlElement element = new HtmlElement("html", isVoid: true);
            HtmlAttribute newAttribute1 = new HtmlAttribute("Attribute1");
            HtmlAttribute newAttribute2 = new HtmlAttribute("Attribute2");
            element.Add(new HtmlObject[] { newAttribute1, newAttribute2 });

            Assert.Equal(element, newAttribute1.Parent);
            Assert.Equal(element, newAttribute2.Parent);
            Assert.Empty(element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute1, newAttribute2 }, element.Attributes());
        }

        [Fact]
        public void AddFirst_ParamsHtmlObject_VoidElement()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlAttribute newAttribute1 = new HtmlAttribute("Attribute1");
            element.Add(newAttribute1);
            
            HtmlAttribute newAttribute2 = new HtmlAttribute("Attribute2");
            HtmlAttribute newAttribute3 = new HtmlAttribute("Attribute3");
            element.AddFirst(new HtmlObject[] { newAttribute2, newAttribute3 });

            Assert.Equal(element, newAttribute2.Parent);
            Assert.Equal(element, newAttribute3.Parent);
            Assert.Empty(element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute3, newAttribute2, newAttribute1 }, element.Attributes());
        }

        [Fact]
        public void Add_ParamsHtmlObject_Empty()
        {
            HtmlElement element = new HtmlElement("html");
            element.Add(new HtmlObject[0]);
            Assert.Empty(element.Elements());
            Assert.Empty(element.Attributes());
        }

        [Fact]
        public void AddFirst_ParamsHtmlObject_Empty()
        {
            HtmlElement element = new HtmlElement("html");
            element.AddFirst(new HtmlObject[0]);
            Assert.Empty(element.Elements());
            Assert.Empty(element.Attributes());
        }

        [Fact]
        public void Add_IEnumerableHtmlObject_NonVoidElement()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlElement newElement = new HtmlElement("body");
            HtmlAttribute newAttribute = new HtmlAttribute("Attribute");
            element.Add((IEnumerable<HtmlObject>)new HtmlObject[] { newElement, newAttribute });

            Assert.Equal(element, newElement.Parent);
            Assert.Equal(element, newAttribute.Parent);
            Assert.Equal(new HtmlObject[] { newElement }, element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute }, element.Attributes());
        }

        [Fact]
        public void AddFirst_IEnumerableHtmlObject_NonVoidElement()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlElement newElement1 = new HtmlElement("body");
            HtmlAttribute newAttribute1 = new HtmlAttribute("Attribute1");
            element.Add((IEnumerable<HtmlObject>)new HtmlObject[] { newElement1, newAttribute1 });

            HtmlElement newElement2 = new HtmlElement("head");
            HtmlAttribute newAttribute2 = new HtmlAttribute("Attribute2");
            element.AddFirst((IEnumerable<HtmlObject>)new HtmlObject[] { newElement2, newAttribute2 });

            Assert.Equal(element, newElement2.Parent);
            Assert.Equal(element, newAttribute2.Parent);
            Assert.Equal(new HtmlObject[] { newElement2, newElement1 }, element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute2, newAttribute1 }, element.Attributes());
        }

        [Fact]
        public void Add_IEnumerableHtmlObject_VoidElement()
        {
            HtmlElement element = new HtmlElement("html", isVoid: true);
            HtmlAttribute newAttribute1 = new HtmlAttribute("Attribute1");
            HtmlAttribute newAttribute2 = new HtmlAttribute("Attribute2");
            element.Add((IEnumerable<HtmlObject>)new HtmlObject[] { newAttribute1, newAttribute2 });

            Assert.Equal(element, newAttribute1.Parent);
            Assert.Equal(element, newAttribute2.Parent);
            Assert.Empty(element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute1, newAttribute2 }, element.Attributes());
        }

        [Fact]
        public void AddFirst_IEnumerableHtmlObject_VoidElement()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlAttribute newAttribute1 = new HtmlAttribute("Attribute1");
            element.Add(newAttribute1);

            HtmlAttribute newAttribute2 = new HtmlAttribute("Attribute2");
            HtmlAttribute newAttribute3 = new HtmlAttribute("Attribute3");
            element.AddFirst((IEnumerable<HtmlObject>)new HtmlObject[] { newAttribute2, newAttribute3 });

            Assert.Equal(element, newAttribute2.Parent);
            Assert.Equal(element, newAttribute3.Parent);
            Assert.Empty(element.Elements());
            Assert.Equal(new HtmlAttribute[] { newAttribute3, newAttribute2, newAttribute1 }, element.Attributes());
        }

        [Fact]
        public void Add_IEnumerableHtmlObject_Empty()
        {
            HtmlElement element = new HtmlElement("html");
            element.Add(new HtmlObject[0]);
            Assert.Empty(element.Elements());
            Assert.Empty(element.Attributes());
        }
        
        [Fact]
        public void AddFirst_IEnumerableHtmlObject_Empty()
        {
            HtmlElement element = new HtmlElement("html");
            element.AddFirst(new HtmlObject[0]);
            Assert.Empty(element.Elements());
            Assert.Empty(element.Attributes());
        }

        [Fact]
        public void Add_NullContent_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<ArgumentNullException>("content", () => element.Add((HtmlObject)null));
            Assert.Throws<ArgumentNullException>("content", () => element.Add((HtmlObject[])null));
            Assert.Throws<ArgumentNullException>("content", () => element.Add((IEnumerable<HtmlObject>)null));

            Assert.Throws<ArgumentNullException>("content", () => element.AddFirst((HtmlObject)null));
            Assert.Throws<ArgumentNullException>("content", () => element.AddFirst((HtmlObject[])null));
            Assert.Throws<ArgumentNullException>("content", () => element.AddFirst((IEnumerable<HtmlObject>)null));
        }

        [Fact]
        public void Add_NullObjectInContent_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<ArgumentNullException>("content", () => element.Add(new HtmlObject[] { null }));
            Assert.Throws<ArgumentNullException>("content", () => element.Add((IEnumerable<HtmlObject>)new HtmlObject[] { null }));

            Assert.Throws<ArgumentNullException>("content", () => element.AddFirst(new HtmlObject[] { null }));
            Assert.Throws<ArgumentNullException>("content", () => element.AddFirst((IEnumerable<HtmlObject>)new HtmlObject[] { null }));
        }

        [Fact]
        public void Add_SameElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<InvalidOperationException>(() => element.Add(element));
            Assert.Throws<InvalidOperationException>(() => element.Add(new HtmlObject[] { element }));
            Assert.Throws<InvalidOperationException>(() => element.Add((IEnumerable<HtmlObject>)new HtmlObject[] { element }));

            Assert.Throws<InvalidOperationException>(() => element.AddFirst(new HtmlObject[] { element }));
            Assert.Throws<InvalidOperationException>(() => element.AddFirst((IEnumerable<HtmlObject>)new HtmlObject[] { element }));
        }

        [Fact]
        public void Add_DuplicateElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlElement newElement = new HtmlElement("body");
            element.Add(newElement);

            Assert.Throws<InvalidOperationException>(() => element.Add(newElement));
            Assert.Throws<InvalidOperationException>(() => element.Add(new HtmlObject[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.Add((IEnumerable<HtmlElement>)new HtmlElement[] { newElement }));

            Assert.Throws<InvalidOperationException>(() => element.AddFirst(newElement));
            Assert.Throws<InvalidOperationException>(() => element.AddFirst(new HtmlObject[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.AddFirst((IEnumerable<HtmlElement>)new HtmlElement[] { newElement }));
        }

        [Fact]
        public void Add_DuplicateAttribute_ThrowsInvalidOperationException()
        {
            HtmlElement attribute = new HtmlElement("html");
            HtmlAttribute newAttribute = new HtmlAttribute("body");
            attribute.Add(newAttribute);

            Assert.Throws<InvalidOperationException>(() => attribute.Add(newAttribute));
            Assert.Throws<InvalidOperationException>(() => attribute.Add(new HtmlObject[] { newAttribute }));
            Assert.Throws<InvalidOperationException>(() => attribute.Add((IEnumerable<HtmlObject>)new HtmlObject[] { newAttribute }));

            Assert.Throws<InvalidOperationException>(() => attribute.AddFirst(newAttribute));
            Assert.Throws<InvalidOperationException>(() => attribute.AddFirst(new HtmlObject[] { newAttribute }));
            Assert.Throws<InvalidOperationException>(() => attribute.AddFirst((IEnumerable<HtmlObject>)new HtmlObject[] { newAttribute }));
        }

        [Fact]
        public void Add_ElementToVoidElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("br", isVoid: true);
            HtmlElement newElement = new HtmlElement("p");

            Assert.Throws<InvalidOperationException>(() => element.Add(newElement));
            Assert.Throws<InvalidOperationException>(() => element.Add(new HtmlObject[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.Add((IEnumerable<HtmlObject>)new HtmlObject[] { newElement }));

            Assert.Throws<InvalidOperationException>(() => element.AddFirst(newElement));
            Assert.Throws<InvalidOperationException>(() => element.AddFirst(new HtmlObject[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.AddFirst((IEnumerable<HtmlObject>)new HtmlObject[] { newElement }));
        }

        [Fact]
        public void Add_ElementHasDifferentParent_RemovesFromOldParent()
        {
            HtmlElement parent = new HtmlElement("parent");
            HtmlElement child1 = new HtmlElement("child1");
            HtmlElement granchild1 = new HtmlElement("grandchild1");
            HtmlElement granchild2 = new HtmlElement("grandchild2");
            HtmlElement granchild3 = new HtmlElement("grandchild3");
            HtmlElement child2 = new HtmlElement("child2");

            parent.Add(child1, child2);
            child1.Add(granchild1, granchild2, granchild3);

            child2.Add(granchild1);
            Assert.Equal(child2, granchild1.Parent);
            Assert.Equal(new HtmlElement[] { granchild1 }, child2.Elements());
            Assert.Equal(new HtmlElement[] { granchild2, granchild3 }, child1.Elements());

            child2.Add(granchild2);
            Assert.Equal(child2, granchild2.Parent);
            Assert.Equal(new HtmlElement[] { granchild1, granchild2 }, child2.Elements());
            Assert.Equal(new HtmlElement[] { granchild3 }, child1.Elements());

            child2.Add(granchild3);
            Assert.Equal(child2, granchild3.Parent);
            Assert.Equal(new HtmlElement[] { granchild1, granchild2, granchild3 }, child2.Elements());
            Assert.Empty(child1.Elements());
        }

        [Fact]
        public void Add_AttributeHasDifferentParent_RemovesFromOldParent()
        {
            HtmlElement parent = new HtmlElement("parent");
            HtmlElement child1 = new HtmlElement("child1");
            HtmlAttribute attribute1 = new HtmlAttribute("Attribute1");
            HtmlAttribute attribute2 = new HtmlAttribute("Attribute2");
            HtmlAttribute attribute3 = new HtmlAttribute("Attribute3");
            HtmlElement child2 = new HtmlElement("child2");

            parent.Add(child1, child2);
            child1.Add(attribute1, attribute2, attribute3);

            child2.Add(attribute1);
            Assert.Equal(child2, attribute1.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute1 }, child2.Attributes());
            Assert.Equal(new HtmlAttribute[] { attribute2, attribute3 }, child1.Attributes());

            child2.Add(attribute2);
            Assert.Equal(child2, attribute2.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute1, attribute2 }, child2.Attributes());
            Assert.Equal(new HtmlAttribute[] { attribute3 }, child1.Attributes());

            child2.Add(attribute3);
            Assert.Equal(child2, attribute3.Parent);
            Assert.Equal(new HtmlAttribute[] { attribute1, attribute2, attribute3 }, child2.Attributes());
            Assert.Empty(child1.Attributes());
        }

        [Theory]
        [MemberData(nameof(Objects_TestData))]
        public static void ReplaceAll(HtmlObject[] content)
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h1"), new HtmlAttribute("a"), new HtmlAttribute("b"));
            element.ReplaceAll(content);
            Assert.Equal(content.Where(obj => obj is HtmlElement).Cast<HtmlElement>().ToArray(), element.Elements().ToArray());
            Assert.Equal(content.Where(obj => obj is HtmlAttribute).Cast<HtmlAttribute>().ToArray(), element.Attributes().ToArray());
            Assert.Equal(content.Length, element.ElementsAndAttributes().Count());
        }

        [Fact]
        public void ReplaceAll_ParamsHtmlObject_AttributeToVoidElement()
        {
            HtmlElement element = new HtmlElement("br", isVoid: true);
            element.Add(new HtmlAttribute("a"));

            HtmlAttribute[] attributes = new HtmlAttribute[] { new HtmlAttribute("b"), new HtmlAttribute("c") };
            element.ReplaceAll(attributes);
            Assert.Equal(attributes, element.Attributes());
        }

        [Fact]
        public void ReplaceAll_IEnumerableHtmlObject_AttributeToVoidElement()
        {
            HtmlElement element = new HtmlElement("br", isVoid: true);
            element.Add(new HtmlAttribute("a"));

            HtmlAttribute[] attributes = new HtmlAttribute[] { new HtmlAttribute("b"), new HtmlAttribute("c") };
            element.ReplaceAll((IEnumerable<HtmlObject>)attributes);
            Assert.Equal(attributes, element.Attributes());
        }

        [Fact]
        public void ReplaceAll_NullContent_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<ArgumentNullException>("content", () => element.ReplaceAll(null));
            Assert.Throws<ArgumentNullException>("content", () => element.ReplaceAll((IEnumerable<HtmlObject>)null));
        }

        [Fact]
        public void ReplaceAll_SameElementInContent_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll(new HtmlObject[] { element }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll((IEnumerable<HtmlObject>)new HtmlObject[] { element }));
        }

        [Fact]
        public void ReplaceAll_DuplicateElementInContents_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlElement newElement = new HtmlElement("p");
            element.Add(newElement);

            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll(new HtmlObject[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll((IEnumerable<HtmlObject>)new HtmlObject[] { newElement }));
        }

        [Fact]
        public void ReplaceAll_DuplicateAttributeInContents_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlAttribute newAttribute = new HtmlAttribute("Attribute1");
            element.Add(newAttribute);

            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll(new HtmlObject[] { newAttribute }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll((IEnumerable<HtmlObject>)new HtmlObject[] { newAttribute }));
        }

        [Fact]
        public void ReplaceAll_ElementToVoidElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("br", isVoid: true);
            HtmlElement newElement = new HtmlElement("p");
            
            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll(new HtmlObject[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceAll((IEnumerable<HtmlObject>)new HtmlObject[] { newElement }));
        }

        public static IEnumerable<object[]> Elements_TestData()
        {
            yield return new object[] { new HtmlElement[0] };
            yield return new object[] { new HtmlElement[] { new HtmlElement("h1") } };
            yield return new object[] { new HtmlElement[] { new HtmlElement("h1"), new HtmlElement("h2") } };
            yield return new object[] { new HtmlElement[] { new HtmlElement("h1"), new HtmlElement("h2"), new HtmlElement("h1") } };
        }

        [Theory]
        [MemberData(nameof(Elements_TestData))]
        public void ReplaceElements_ParamsHtmlElement(HtmlElement[] elements)
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h3"), new HtmlAttribute("a"), new HtmlAttribute("b"));
            element.ReplaceElements(elements);
            Assert.Equal(elements, element.Elements().ToArray());
            Assert.Equal(2, element.Attributes().Count());
            Assert.Equal(elements.Length + 2, element.ElementsAndAttributes().Count());
        }

        [Theory]
        [MemberData(nameof(Elements_TestData))]
        public void ReplaceElements_IEnumerableHtmlElement(HtmlElement[] elements)
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h3"), new HtmlAttribute("a"), new HtmlAttribute("b"));
            element.ReplaceElements(elements);
            Assert.Equal(elements, element.Elements().ToArray());
            Assert.Equal(2, element.Attributes().Count());
            Assert.Equal(elements.Length + 2, element.ElementsAndAttributes().Count());
        }

        [Fact]
        public void ReplaceElements_NullElements_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<ArgumentNullException>("elements", () => element.ReplaceElements(null));
            Assert.Throws<ArgumentNullException>("elements", () => element.ReplaceElements((IEnumerable<HtmlElement>)null));
        }

        [Fact]
        public void ReplaceElements_NullElementInElements_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<ArgumentNullException>("content", () => element.ReplaceElements(new HtmlElement[] { null }));
            Assert.Throws<ArgumentNullException>("content", () => element.ReplaceElements((IEnumerable<HtmlElement>)new HtmlElement[] { null }));
        }
        
        [Fact]
        public void ReplaceElements_SameElementInElements_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<InvalidOperationException>(() => element.ReplaceElements(new HtmlElement[] { element }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceElements((IEnumerable<HtmlElement>)new HtmlElement[] { element }));
        }

        [Fact]
        public void ReplaceElements_DuplicateElementInElements_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlElement newElement = new HtmlElement("p");
            element.Add(newElement);

            Assert.Throws<InvalidOperationException>(() => element.ReplaceElements(new HtmlElement[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceElements((IEnumerable<HtmlElement>)new HtmlElement[] { newElement }));
        }

        [Fact]
        public void ReplaceElements_ElementToVoidElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("br", isVoid: true);
            HtmlElement newElement = new HtmlElement("p");

            Assert.Throws<InvalidOperationException>(() => element.ReplaceElements(new HtmlElement[] { newElement }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceElements((IEnumerable<HtmlElement>)new HtmlElement[] { newElement }));
        }

        [Theory]
        [MemberData(nameof(Attributes_TestData))]
        public void ReplaceAttributes_ParamsHtmlAttribute(HtmlAttribute[] attributes)
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h1"), new HtmlAttribute("a"), new HtmlAttribute("b"));
            element.ReplaceAttributes(attributes);
            Assert.Equal(1, element.Elements().Count());
            Assert.Equal(attributes, element.Attributes().ToArray());
            Assert.Equal(1 + attributes.Length, element.ElementsAndAttributes().Count());
        }

        [Fact]
        public void ReplaceAttributes_ParamsHtmlAttribute_VoidElement()
        {
            HtmlElement element = new HtmlElement("br", isVoid: true);
            element.Add(new HtmlAttribute("a"));

            HtmlAttribute[] attributes = new HtmlAttribute[] { new HtmlAttribute("b"), new HtmlAttribute("c") };
            element.ReplaceAttributes(attributes);
            Assert.Equal(attributes, element.Attributes());
        }

        [Theory]
        [MemberData(nameof(Attributes_TestData))]
        public void ReplaceAttributes_IEnumerableHtmlAttribute(HtmlAttribute[] attributes)
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h1"), new HtmlAttribute("a"), new HtmlAttribute("b"));
            element.ReplaceAttributes((IEnumerable<HtmlAttribute>)attributes);
            Assert.Equal(1, element.Elements().Count());
            Assert.Equal(attributes, element.Attributes().ToArray());
            Assert.Equal(1 + attributes.Length, element.ElementsAndAttributes().Count());
        }

        [Fact]
        public void ReplaceAttributes_IEnumerableHtmlAttribute_VoidElement()
        {
            HtmlElement element = new HtmlElement("br", isVoid: true);
            element.Add(new HtmlAttribute("a"));

            HtmlAttribute[] attributes = new HtmlAttribute[] { new HtmlAttribute("b"), new HtmlAttribute("c") };
            element.ReplaceAttributes((IEnumerable<HtmlAttribute>)attributes);
            Assert.Equal(attributes, element.Attributes());
        }

        [Fact]
        public void ReplaceAttributes_NullElements_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<ArgumentNullException>("attributes", () => element.ReplaceAttributes(null));
            Assert.Throws<ArgumentNullException>("attributes", () => element.ReplaceAttributes((IEnumerable<HtmlAttribute>)null));
        }

        [Fact]
        public void ReplaceAttributes_NullAttributeInAttributes_ThrowsArgumentNullException()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Throws<ArgumentNullException>("content", () => element.ReplaceAttributes(new HtmlAttribute[] { null }));
            Assert.Throws<ArgumentNullException>("content", () => element.ReplaceAttributes((IEnumerable<HtmlAttribute>)new HtmlAttribute[] { null }));
        }

        [Fact]
        public void ReplaceAttributes_DuplicateAttributeInAttribues_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html");
            HtmlAttribute newAttribute = new HtmlAttribute("Attribute1");
            element.Add(newAttribute);

            Assert.Throws<InvalidOperationException>(() => element.ReplaceAttributes(new HtmlAttribute[] { newAttribute }));
            Assert.Throws<InvalidOperationException>(() => element.ReplaceAttributes((IEnumerable<HtmlAttribute>)new HtmlAttribute[] { newAttribute }));
        }

        [Theory]
        [InlineData("InnerText")]
        [InlineData(" \t \r \n")]
        [InlineData("")]
        [InlineData(null)]
        public void SetInnerText(string innerText)
        {
            HtmlElement element = new HtmlElement("html");
            element.SetInnerText(innerText);
            Assert.Equal(innerText, element.InnerText);
        }

        [Fact]
        public void SetInnerText_VoidElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html", isVoid: true);
            Assert.Throws<InvalidOperationException>(() => element.SetInnerText("InnerText"));
        }

        [Fact]
        public void RemoveAll()
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h1"), new HtmlAttribute("a"), new HtmlAttribute("b"));

            element.RemoveAll();
            Assert.Equal(0, element.Elements().Count());
            Assert.Equal(0, element.Attributes().Count());

            // Make sure we can remove from an empty element
            element.RemoveAll();
            Assert.Equal(0, element.Elements().Count());
            Assert.Equal(0, element.Attributes().Count());
        }

        [Fact]
        public void RemoveAll_VoidElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html", isVoid: true);
            Assert.Throws<InvalidOperationException>(() => element.RemoveAll());
        }

        [Fact]
        public void RemoveElements()
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h1"), new HtmlAttribute("a"), new HtmlAttribute("b"));

            element.RemoveElements();
            Assert.Equal(0, element.Elements().Count());
            Assert.Equal(2, element.Attributes().Count());

            // Make sure we can remove from an empty element
            element.RemoveElements();
            Assert.Equal(0, element.Elements().Count());
            Assert.Equal(2, element.Attributes().Count());
        }

        [Fact]
        public void RemoveElements_VoidElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html", isVoid: true);
            Assert.Throws<InvalidOperationException>(() => element.RemoveElements());
        }

        [Fact]
        public void RemoveAttributes()
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("h1"), new HtmlAttribute("a"), new HtmlAttribute("b"));

            element.RemoveAttributes();
            Assert.Equal(1, element.Elements().Count());
            Assert.Equal(0, element.Attributes().Count());

            // Make sure we can remove from an empty element
            element.RemoveAttributes();
            Assert.Equal(1, element.Elements().Count());
            Assert.Equal(0, element.Attributes().Count());
        }

        [Fact]
        public void RemoveAttributes_VoidElement_ThrowsInvalidOperationException()
        {
            HtmlElement element = new HtmlElement("html", isVoid: true);
            Assert.Throws<InvalidOperationException>(() => element.RemoveAttributes());
        }

        [Fact]
        public void FirstElement_HasElements_ReturnsExpected()
        {
            HtmlElement expected = new HtmlElement("head");
            HtmlElement element = new HtmlElement("html", expected, new HtmlElement("body"));
            Assert.Equal(expected, element.FirstElement);
        }

        [Fact]
        public void FirstElement_NoElements_ReturnsNull()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Null(element.FirstElement);

            element.Add(new HtmlAttribute("attribute"));
            Assert.Null(element.FirstElement);
        }

        [Fact]
        public void LastElement_HasElements_ReturnsExpected()
        {
            HtmlElement expected = new HtmlElement("body");
            HtmlElement element = new HtmlElement("html", new HtmlElement("head"), expected);
            Assert.Equal(expected, element.LastElement);
        }

        [Fact]
        public void LastElement_NoElements_ReturnsNull()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Null(element.LastElement);

            element.Add(new HtmlAttribute("attribute"));
            Assert.Null(element.FirstElement);
        }
        
        [Fact]
        public void FirstElement_LastElement_OneElement_ReturnsSame()
        {
            HtmlElement expected = new HtmlElement("head");
            HtmlElement element = new HtmlElement("html", expected);
            Assert.Equal(expected, element.FirstElement);
            Assert.Equal(element.FirstElement, element.LastElement);
        }

        [Fact]
        public void FirstAttribute_HasAttributes_ReturnsExpected()
        {
            HtmlAttribute expected = new HtmlAttribute("Attribute1");
            HtmlElement element = new HtmlElement("html", expected, new HtmlAttribute("Attribute2"));
            Assert.Equal(expected, element.FirstAttribute);
        }

        [Fact]
        public void FirstAttribute_NoAttributes_ReturnsNull()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Null(element.FirstAttribute);

            element.Add(new HtmlElement("body"));
            Assert.Null(element.FirstAttribute);
        }

        [Fact]
        public void LastAttribute_HasAttributes_ReturnsExpected()
        {
            HtmlAttribute expected = new HtmlAttribute("Attribute2");
            HtmlElement element = new HtmlElement("html", new HtmlAttribute("Attribute1"), expected);
            Assert.Equal(expected, element.LastAttribute);
        }

        [Fact]
        public void LastAttribute_NoAttributes_ReturnsNull()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.Null(element.LastAttribute);

            element.Add(new HtmlElement("body"));
            Assert.Null(element.LastAttribute);
        }

        [Fact]
        public void FirstAttribute_LastAttribute_OneAttribute_ReturnsSame()
        {
            HtmlAttribute expected = new HtmlAttribute("Attribute");
            HtmlElement element = new HtmlElement("html", expected);
            Assert.Equal(expected, element.FirstAttribute);
            Assert.Equal(element.FirstAttribute, element.LastAttribute);
        }
        
        [Fact]
        public void HasElements_HasElements_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("head"));
            Assert.True(element.HasElements);
        }

        [Fact]
        public void HasElements_NoElements_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.False(element.HasElements);

            element.Add(new HtmlAttribute("attribute"));
            Assert.False(element.HasElements);
        }

        [Fact]
        public void HasAttributes_HasAttributes_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html", new HtmlAttribute("attribute"));
            Assert.True(element.HasAttributes);
        }

        [Fact]
        public void HasAttributes_NoAttributes_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.False(element.HasAttributes);

            element.Add(new HtmlElement("head"));
            Assert.False(element.HasAttributes);
        }

        [Fact]
        public void IsEmpty_HasElements_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("head"));
            Assert.False(element.IsEmpty);
        }

        [Fact]
        public void IsEmpty_HasAttributes_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html", new HtmlAttribute("attribute"));
            Assert.False(element.IsEmpty);
        }

        [Fact]
        public void IsEmpty_HasElementsAndAttributes_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html", new HtmlElement("head"), new HtmlAttribute("attribute"));
            Assert.False(element.IsEmpty);
        }

        [Fact]
        public void IsEmpty_HasNoElementsOrAttributes_ReturnsTrue()
        {
            HtmlElement element = new HtmlElement("html");
            Assert.True(element.IsEmpty);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(9)]
        public void MinimumIndentLength_Set_Get_ReturnsExpected(int value)
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Equal(1, element.MinimumIndentDepth);

            element.MinimumIndentDepth = value;
            Assert.Equal(value, element.MinimumIndentDepth);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(10)]
        public void MinimumIndentLength_Set_InvalidValue_ThrowsArgumentOutOfRangeException(int value)
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Throws<ArgumentOutOfRangeException>("value", () => element.MinimumIndentDepth = value);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(8)]
        [InlineData(10)]
        public void MaximumIndentLength_Set_Get_ReturnsExpected(int value)
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Equal(9, element.MaximumIndentDepth);

            element.MaximumIndentDepth = value;
            Assert.Equal(value, element.MaximumIndentDepth);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void MaximumIndentLength_Set_InvalidValue_ThrowsArgumentOutOfRangeException(int value)
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Throws<ArgumentOutOfRangeException>("value", () => element.MaximumIndentDepth = value);
        }

        [Fact]
        public void NextElement_Get_ReturnsExpected()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h2");
            HtmlElement third = new HtmlElement("h3");
            HtmlElement parent = new HtmlElement("div", first, second, third);

            Assert.Equal(second, first.NextElement);
            Assert.Equal(third, second.NextElement);
            Assert.Null(third.NextElement);
        }

        [Fact]
        public void NextElement_NoParent_ReturnsNull()
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Null(element.NextElement);
        }

        [Fact]
        public void PreviousElement_Get_ReturnsExpected()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h2");
            HtmlElement third = new HtmlElement("h3");
            HtmlElement parent = new HtmlElement("div", first, second, third);

            Assert.Null(first.PreviousElement);
            Assert.Equal(first, second.PreviousElement);
            Assert.Equal(second, third.PreviousElement);
        }

        [Fact]
        public void PreviousElement_NoParent_ReturnsNull()
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Null(element.PreviousElement);
        }

        [Fact]
        public void NextElements_NoTag_ReturnsExpected()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h2");
            HtmlElement third = new HtmlElement("h3");
            HtmlElement parent = new HtmlElement("div", first, second, third);

            Assert.Equal(new HtmlElement[] { second, third }, first.NextElements());
            Assert.Equal(new HtmlElement[] { third }, second.NextElements());
            Assert.Empty(third.NextElements());
        }

        [Fact]
        public void NextElements_CustomTag_ReturnsExpected()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h2");
            HtmlElement third = new HtmlElement("h2");
            HtmlElement fourth = new HtmlElement("h4");
            HtmlElement parent = new HtmlElement("div", first, second, third, fourth);

            Assert.Equal(new HtmlElement[] { second, third }, first.NextElements("h2"));
            Assert.Equal(new HtmlElement[] { third }, second.NextElements("h2"));
            Assert.Equal(new HtmlElement[] { fourth }, third.NextElements("h4"));
            Assert.Empty(third.NextElements("h1"));
        }

        [Fact]
        public void NextElements_NoSuchTag_ReturnsEmpty()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h2");
            HtmlElement third = new HtmlElement("h2");
            HtmlElement parent = new HtmlElement("div", first, second, third);

            Assert.Empty(first.NextElements("h1"));
            Assert.Empty(first.NextElements("h4"));
        }

        [Fact]
        public void NextElements_NoParent_ReturnsEmpty()
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Empty(element.NextElements());
            Assert.Empty(element.NextElements("any"));
        }

        [Fact]
        public void PreviousElements_NoTag_ReturnsExpected()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h2");
            HtmlElement third = new HtmlElement("h3");
            HtmlElement parent = new HtmlElement("div", first, second, third);

            Assert.Empty(first.PreviousElements());
            Assert.Equal(new HtmlElement[] { first }, second.PreviousElements());
            Assert.Equal(new HtmlElement[] { second, first }, third.PreviousElements());
        }

        [Fact]
        public void PreviousElements_CustomTag_ReturnsExpected()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h2");
            HtmlElement third = new HtmlElement("h2");
            HtmlElement fourth = new HtmlElement("h4");
            HtmlElement parent = new HtmlElement("div", first, second, third, fourth);

            Assert.Empty(first.PreviousElements("h4"));
            Assert.Equal(new HtmlElement[] { first }, second.PreviousElements("h1"));
            Assert.Equal(new HtmlElement[] { second }, third.PreviousElements("h2"));
            Assert.Equal(new HtmlElement[] { third, second }, fourth.PreviousElements("h2"));
        }

        [Fact]
        public void PreviousElements_NoSuchTag_ReturnsEmpty()
        {
            HtmlElement first = new HtmlElement("h1");
            HtmlElement second = new HtmlElement("h1");
            HtmlElement third = new HtmlElement("h2");
            HtmlElement parent = new HtmlElement("div", first, second, third);

            Assert.Empty(third.PreviousElements("h2"));
            Assert.Empty(third.PreviousElements("h4"));
        }

        [Fact]
        public void PreviousElements_NoParent_ReturnsEmpty()
        {
            HtmlElement element = new HtmlElement("div");
            Assert.Empty(element.PreviousElements());
            Assert.Empty(element.PreviousElements("any"));
        }

        [Fact]
        public void RemoveFromParent_OnlyChild_Works()
        {
            HtmlElement child = new HtmlElement("h1");
            HtmlElement parent = new HtmlElement("div", child);

            child.RemoveFromParent();
            Assert.Null(child.Parent);
            Assert.Empty(parent.Elements());
            Assert.False(parent.HasElements);
        }

        [Fact]
        public void RemoveFromParent_FirstChild_Works()
        {
            HtmlElement child1 = new HtmlElement("h1");
            HtmlElement child2 = new HtmlElement("h2");
            HtmlElement child3 = new HtmlElement("h3");
            HtmlElement parent = new HtmlElement("div", child1, child2, child3);

            // Updates Elements
            child1.RemoveFromParent();
            Assert.Null(child1.Parent);
            Assert.Equal(new HtmlElement[] { child2, child3 }, parent.Elements());
            Assert.True(parent.HasElements);

            // Updates LinkedList
            Assert.Null(child1.PreviousElement);
            Assert.Null(child1.NextElement);
            Assert.Null(child2.PreviousElement);
            Assert.Equal(child3, child2.NextElement);
            Assert.Equal(child2, child3.PreviousElement);
            Assert.Null(child3.NextElement);
        }

        [Fact]
        public void RemoveFromParent_LastChild_Works()
        {
            HtmlElement child1 = new HtmlElement("h1");
            HtmlElement child2 = new HtmlElement("h2");
            HtmlElement child3 = new HtmlElement("h3");
            HtmlElement parent = new HtmlElement("div", child1, child2, child3);

            // Updates Elements
            child3.RemoveFromParent();
            Assert.Null(child3.Parent);
            Assert.Equal(new HtmlElement[] { child1, child2 }, parent.Elements());
            Assert.True(parent.HasElements);

            // Updates LinkedList
            Assert.Null(child1.PreviousElement);
            Assert.Equal(child2, child1.NextElement);
            Assert.Equal(child1, child2.PreviousElement);
            Assert.Null(child2.NextElement);
            Assert.Null(child3.PreviousElement);
            Assert.Null(child3.NextElement);
        }

        [Fact]
        public void RemoveFromParent_MiddleChild_Works()
        {
            HtmlElement child1 = new HtmlElement("h1");
            HtmlElement child2 = new HtmlElement("h2");
            HtmlElement child3 = new HtmlElement("h3");
            HtmlElement parent = new HtmlElement("div", child1, child2, child3);

            // Updates Elements
            child2.RemoveFromParent();
            Assert.Null(child2.Parent);
            Assert.Equal(new HtmlElement[] { child1, child3 }, parent.Elements());
            Assert.True(parent.HasElements);

            // Updates LinkedList
            Assert.Null(child1.PreviousElement);
            Assert.Equal(child3, child1.NextElement);
            Assert.Null(child2.PreviousElement);
            Assert.Null(child2.NextElement);
            Assert.Equal(child1, child3.PreviousElement);
            Assert.Null(child3.NextElement);
        }

        [Fact]
        public void RemoveFromParent_NoParent_DoesNothing()
        {
            HtmlElement element = new HtmlElement("html");
            element.RemoveFromParent();

            Assert.Null(element.Parent);
        }

        [Fact]
        public void Descendants_OneLayerOfElements_ReturnsExpected()
        {
            HtmlElement parent = new HtmlElement("Parent");
            HtmlElement child1 = new HtmlElement("Child1");
            HtmlElement child2 = new HtmlElement("Child2");

            parent.Add(child1, child2);

            VerifyDescendants(parent, null, new HtmlElement[] { child1, child2 });
            VerifyDescendants(parent, "Child1", new HtmlElement[] { child1 });
            VerifyDescendants(parent, "any", new HtmlElement[0]);
        }

        [Fact]
        public void Descendants_TwoLayersOfElements_ReturnsExpected()
        {
            HtmlElement parent = new HtmlElement("Parent");
            HtmlElement child1 = new HtmlElement("Child1");
            HtmlElement grandchild1 = new HtmlElement("Grandchild1");

            HtmlElement child2 = new HtmlElement("Child2");
            HtmlElement grandchild2 = new HtmlElement("Grandchild2");
            HtmlElement grandchild3 = new HtmlElement("Grandchild3");
            HtmlElement grandchild4 = new HtmlElement("Grandchild3");

            HtmlElement child3 = new HtmlElement("Child3");
            HtmlElement child4 = new HtmlElement("Child3");
            HtmlElement grandchild5 = new HtmlElement("Child3");

            parent.Add(child1, child2, child3, child4);
            child1.Add(grandchild1);
            child2.Add(grandchild2, grandchild3, grandchild4);
            child4.Add(grandchild5);
            
            VerifyDescendants(parent, null, new HtmlElement[] { child1, grandchild1, child2, grandchild2, grandchild3, grandchild4, child3, child4, grandchild5 });
            VerifyDescendants(parent, "Grandchild1", new HtmlElement[] { grandchild1 });
            VerifyDescendants(parent, "Grandchild3", new HtmlElement[] { grandchild3, grandchild4 });
            VerifyDescendants(parent, "Child3", new HtmlElement[] { child3, child4, grandchild5 });
            VerifyDescendants(parent, "any", new HtmlElement[0]);
        }

        [Fact]
        public void Descendants_ThreeLayersOfElements_ReturnsExpected()
        {
            HtmlElement parent = new HtmlElement("Parent");
            HtmlElement child1 = new HtmlElement("Child1");
            HtmlElement grandchild1 = new HtmlElement("Grandchild1");
            HtmlElement greatGrandchild1 = new HtmlElement("GreatGrandchild1");
            HtmlElement greatGrandchild2 = new HtmlElement("GreatGrandchild2");

            HtmlElement child2 = new HtmlElement("Child2");
            HtmlElement grandchild2 = new HtmlElement("Grandchild2");

            parent.Add(child1, child2);
            child1.Add(grandchild1);
            child2.Add(grandchild2);
            grandchild1.Add(greatGrandchild1);
            grandchild1.Add(greatGrandchild2);

            VerifyDescendants(parent, null, new HtmlElement[] { child1, grandchild1, greatGrandchild1, greatGrandchild2, child2, grandchild2 });
        }

        [Fact]
        public void Descendants_NoElements_ReturnsEmpty()
        {
            HtmlElement element = new HtmlElement("html");
            VerifyDescendants(element, null, new HtmlElement[0]);
            VerifyDescendants(element, "any", new HtmlElement[0]);
        }

        private static void VerifyDescendants(HtmlElement element, string tag, HtmlElement[] expected)
        {
            HtmlElement[] expectedIncludingSelf;;
            if (tag == null || element.Tag == tag)
            {
                expectedIncludingSelf = new HtmlElement[expected.Length + 1];
                Array.Copy(expected, 0, expectedIncludingSelf, 1, expected.Length);
                expectedIncludingSelf[0] = element;
            }
            else
            {
                expectedIncludingSelf = expected;
            }

            if (tag == null)
            {
                Assert.Equal(expected, element.Descendants());
                Assert.Equal(expectedIncludingSelf, element.DescendantsAndSelf());
            }
            Assert.Equal(expected, element.Descendants(tag));
            Assert.Equal(expectedIncludingSelf, element.DescendantsAndSelf(tag));
        }
        
        [Fact]
        public void Ancestors_OneLayerOfElements_ReturnsExpected()
        {
            HtmlElement parent = new HtmlElement("Parent");
            HtmlElement child1 = new HtmlElement("Child1");
            HtmlElement child2 = new HtmlElement("Child2");

            parent.Add(child1, child2);

            VerifyAncestors(child1, null, new HtmlElement[] { parent });
            VerifyAncestors(child1, "Parent", new HtmlElement[] { parent });
            VerifyAncestors(child1, "Child1", new HtmlElement[0]);
            VerifyAncestors(child1, "any", new HtmlElement[0]);
        }

        [Fact]
        public void Ancestors_TwoLayersOfElements_ReturnsExpected()
        {
            HtmlElement parent = new HtmlElement("Parent");
            HtmlElement child1 = new HtmlElement("Child");
            HtmlElement grandchild1 = new HtmlElement("Child");

            HtmlElement child2 = new HtmlElement("Child");

            parent.Add(child1, child2);
            child1.Add(grandchild1);

            VerifyAncestors(grandchild1, null, new HtmlElement[] { child1, parent });
            VerifyAncestors(grandchild1, "Child", new HtmlElement[] { child1 });
            VerifyAncestors(grandchild1, "Parent", new HtmlElement[] { parent });
            VerifyAncestors(grandchild1, "any", new HtmlElement[0]);
        }

        [Fact]
        public void Ancestors_ThreeLayersOfElements_ReturnsExpected()
        {
            HtmlElement parent = new HtmlElement("Parent");
            HtmlElement child1 = new HtmlElement("Child");
            HtmlElement grandchild1 = new HtmlElement("Child");
            HtmlElement greatGrandchild = new HtmlElement("GreatGrandchild");

            HtmlElement child2 = new HtmlElement("Child");
            HtmlElement grandchild2 = new HtmlElement("Grandchild");

            parent.Add(child1, child2);
            child1.Add(grandchild1);
            child2.Add(grandchild2);

            grandchild1.Add(greatGrandchild);

            VerifyAncestors(greatGrandchild, null, new HtmlElement[] { grandchild1, child1, parent });
            VerifyAncestors(greatGrandchild, "Child", new HtmlElement[] { grandchild1, child1 });
            VerifyAncestors(greatGrandchild, "Parent", new HtmlElement[] { parent });
            VerifyAncestors(greatGrandchild, "GreatGrandchild", new HtmlElement[0]);
            VerifyAncestors(greatGrandchild, "any", new HtmlElement[0]);
        }

        [Fact]
        public void Ancestors_NoAncestors_ReturnsEmpty()
        {
            HtmlElement parent = new HtmlElement("Parent");

            VerifyAncestors(parent, null, new HtmlElement[0]);
            VerifyAncestors(parent, "Parent", new HtmlElement[0]);
            VerifyAncestors(parent, "any", new HtmlElement[0]);
        }

        private static void VerifyAncestors(HtmlElement element, string tag, HtmlElement[] expected)
        {
            HtmlElement[] expectedIncludingSelf; ;
            if (tag == null || element.Tag == tag)
            {
                expectedIncludingSelf = new HtmlElement[expected.Length + 1];
                Array.Copy(expected, 0, expectedIncludingSelf, 1, expected.Length);
                expectedIncludingSelf[0] = element;
            }
            else
            {
                expectedIncludingSelf = expected;
            }

            if (tag == null)
            {
                Assert.Equal(expected, element.Ancestors());
                Assert.Equal(expectedIncludingSelf, element.AncestorsAndSelf());
            }
            Assert.Equal(expected, element.Ancestors(tag));
            Assert.Equal(expectedIncludingSelf, element.AncestorsAndSelf(tag));
        }
    }
}
