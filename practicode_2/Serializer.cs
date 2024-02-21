using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace practicode_2
{
    internal class Serializer
    {

        public async Task<HashSet<string>> LoadAndParse(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();

            return ParseHtml(html);
        }
        private readonly HtmlHelper htmlHelper = HtmlHelper.htmlHelper;
        private HashSet<string> ParseHtml(string html)
        {
            // מצא את כל התגיות ה-HTML בתוך המחרוזת
            var matches = Regex.Matches(html, @"<[^>]+>");

            HashSet<string> result = new HashSet<string>();

            // חלוף על כל ההתאמות ושמירה של המצביע הראשון (group[0])
            foreach (Match match in matches)
            {
                result.Add(match.Groups[0].Value);
            }

            // הסרת ירידות השורה ורווחים מיותרים


            return result;
        }
        public HtmlElement BuildHtmlTree(HashSet<string> htmlStrings)
        {
            List<string> selfClosingTags = htmlHelper.j1.ToList();
            List<string> voidTags = htmlHelper.j2.ToList();
            HtmlElement root = new HtmlElement();
            HtmlElement currentElement = root;

            foreach (string htmlString in htmlStrings)
            {
                string tagString = htmlString.Trim();

                if (tagString == "html/")
                {
                    // Reached the end of HTML
                    break;
                }
                if (tagString.StartsWith("/"))
                {
                    // Closing tag - go up one level in the tree
                    currentElement = currentElement.Parent;
                }
                else if (voidTags.Contains(tagString) || tagString.EndsWith("/"))
                {
                    // Self-closing tag or void tag - add it to the current element
                    HtmlElement voidTag = new HtmlElement { Name = tagString };
                    currentElement.Children.Add(voidTag);
                }
                else
                {
                    // Regular tag - create a new element and add it to the current element
                    HtmlElement newElement = ParseTag(tagString);
                    newElement.Parent = currentElement;

                    if (currentElement.Name != null)
                    {
                        currentElement.Children.Add(newElement);
                    }

                    else
                    {
                        // If it's the first element, add it to the root
                        root = newElement;
                    }


                    // If the tag is not self-closing, update the current element
                    if (!selfClosingTags.Contains(newElement.Name))
                    {
                        currentElement = newElement;
                    }
                }
            }

            return root;
        }

        private HtmlElement ParseTag(string tagString)
        {
            HtmlElement tag = new HtmlElement();
            tag.Name = tagString.TrimStart('<').TrimEnd('>').Split(' ')[0];

            var attributeMatches = Regex.Matches(tagString, "([^\\s]*?)=\"(.*?)\"");

            foreach (Match match in attributeMatches)
            {
                string attributeName = match.Groups[1].Value;
                string attributeValue = match.Groups[2].Success ? match.Groups[2].Value :
                                        match.Groups[3].Success ? match.Groups[3].Value : null;

                if (attributeName.ToLower() == "class" && !string.IsNullOrWhiteSpace(attributeValue))
                {
                    tag.Classes.AddRange(attributeValue.Split(' '));
                }

                tag.Attributes.Add($"{attributeName}=\"{attributeValue}\"");
                if (attributeValue == null)
                {
                    tag.InnerHtml = attributeValue;
                }
            }
            if (tag.Attributes != null)
            {
                var idAttribute = tag.Attributes.FirstOrDefault(attr => attr.StartsWith("id="));
                tag.Id = idAttribute != null ? idAttribute.Split('=').Length > 1 ? idAttribute.Split('=')[1].Trim('"') : null : null;
                /* tag.InnerHtml = GetInnerHtml(tagString)*/
                ;
            }



            return tag;
        }

    }
}
