using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace practicode_2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }

        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; } = new List<HtmlElement>();



        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                HtmlElement current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        // פונקציה למציאת כל האבות של אלמנט
        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement current = this;

            while (current.Parent != null)
            {
                current = current.Parent;
                yield return current;
            }
        }
        private bool IfSame(HtmlElement element, Selector selector)
        {
            if (selector.TagName == element.Name) return true;
            return false;
        }

        private void RecursiveSearch(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            if (selector.TagName == null)
            {
                results.Add(element);
                return;
            }

            foreach (var child in element.Children)
            {
                if (IfSame(child, selector))
                {
                    RecursiveSearch(child, selector.Child, results);
                }
            }
        }

        public HashSet<HtmlElement> FindElements(Selector selector)
        {
            HashSet<HtmlElement> results = new HashSet<HtmlElement>();

            foreach (var element in this.Descendants())
            {
                RecursiveSearch(element, selector, results);
            }

            return results;
        }

        private bool Match(Selector selector)
        {
            // כאן את יש להמשיך ולהוסיף בדיקות תואמות להגדרת הסלקטור
            // החלק הזה ישונה בהתאם לדרישות הסלקטור
            // זהו דוגמה פשוטה

            if (!string.IsNullOrEmpty(selector.TagName) && !string.Equals(this.Name, selector.TagName, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(selector.Id) && !string.Equals(this.Id, selector.Id, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (selector.Classes.Any() && !selector.Classes.All(c => this.Classes.Contains(c, StringComparer.OrdinalIgnoreCase)))
            {
                return false;
            }

            return true;
        }
        public override string ToString()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append($"<{Name}");

            if (!string.IsNullOrEmpty(Id))
            {
                sb.Append($" id=\"{Id}\"");
            }

            if (Classes.Any())
            {
                sb.Append($" class=\"{string.Join(" ", Classes)}\"");
            }

            if (Attributes.Any())
            {
                sb.Append($" {string.Join(" ", Attributes)}");
            }

            if (!string.IsNullOrEmpty(InnerHtml))
            {
                sb.Append($">{InnerHtml}</{Name}>");
            }
            else
            {
                sb.Append(" />");
            }

            return sb.ToString();
        }


        public string ToString2()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Name: {Name}");
            sb.AppendLine($"Id: {Id}");

            if (Classes.Any())
            {
                // Order the classes alphabetically and print each one on a new line
                sb.AppendLine($"Classes: {string.Join(Environment.NewLine, Classes.OrderBy(c => c))}");
            }

            if (Attributes.Any())
            {
                sb.AppendLine($"Attributes: {string.Join(", ", Attributes)}");
            }

            if (!string.IsNullOrEmpty(InnerHtml))
            {
                sb.AppendLine($"InnerHtml: {InnerHtml}");
            }

            sb.AppendLine($"Children Count: {Parent}");

            return sb.ToString();
        }
    }
}
