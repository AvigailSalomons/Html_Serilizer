﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicode_2
{
    internal class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; } = new List<string>();
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        public Selector()
        {
            Classes = new List<string>();
        }

        public static Selector FromQueryString(string queryString)
        {
            string[] selectors = queryString.Split();
            Selector root = new Selector();
            Selector currentSelector = root;

            foreach (string selectorString in selectors)
            {
                string[] parts = selectorString.Split('#');
                if (parts.Length > 1)
                {
                    currentSelector.Id = parts[1];
                    parts = parts[0].Split('.');
                }
                else
                {
                    parts = selectorString.Split('.');
                }

                if (!string.IsNullOrEmpty(parts[0]))
                {
                    currentSelector.TagName = parts[0];
                }

                for (int i = 1; i < parts.Length; i++)
                {
                    currentSelector.Classes.Add(parts[i]);
                }

                Selector newSelector = new Selector();
                currentSelector.Child = newSelector;
                newSelector.Parent = currentSelector;
                currentSelector = newSelector;
            }

            return root;
        }


        private static bool IsValidHtmlTagName(string tagName)
        {
            // Add your own logic to validate HTML tag names if needed
            // For simplicity, assuming any non-empty string is a valid tag name

            return HtmlHelper.htmlHelper.j1.Contains(tagName) && HtmlHelper.htmlHelper.j2.Contains(tagName);
        }
    }


}
