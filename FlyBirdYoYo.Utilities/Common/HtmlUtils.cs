#region Usings

using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

#endregion

namespace FlyBirdYoYo.Utilities
{

    /// <summary>
    /// HtmlUtils is a Utility class that provides Html Utility methods
    /// </summary>
    /// <remarks>
    /// </remarks>
    /// <history>
    ///		[cnurse]	11/16/2004	documented
    /// </history>
    /// -----------------------------------------------------------------------------
    public class HtmlUtils
    {
        private static readonly Regex HtmlDetectionRegex = new Regex("<(.*\\s*)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Clean removes any HTML Tags, Entities (and optionally any punctuation) from
        /// a string
        /// </summary>
        /// <remarks>
        /// Encoded Tags are getting decoded, as they are part of the content!
        /// </remarks>
        /// <param name="HTML">The Html to clean</param>
        /// <param name="RemovePunctuation">A flag indicating whether to remove punctuation</param>
        /// <returns>The cleaned up string</returns>
        /// <history>
        ///		[cnurse]	11/16/2004	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string Clean(string HTML, bool RemovePunctuation)
        {
            //First remove any HTML Tags ("<....>")
            HTML = StripTags(HTML, true);

            //Second replace any HTML entities (&nbsp; &lt; etc) through their char symbol
            HTML = HttpUtility.HtmlDecode(HTML);

            //Thirdly remove any punctuation
            if (RemovePunctuation)
            {
                HTML = StripPunctuation(HTML, true);
            }
            //Finally remove extra whitespace
            HTML = StripWhiteSpace(HTML, true);
            return HTML;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   CleanWithTagInfo removes unspecified HTML Tags, Entities (and optionally any punctuation) from a string.
        /// </summary>
        /// <param name = "html"></param>
        /// <param name="tagsFilter"></param>
        /// <param name = "removePunctuation"></param>
        /// <returns>The cleaned up string</returns>
        /// <remarks>
        /// </remarks>
        /// <history>
        ///   [vnguyen]   09/02/2010   Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string CleanWithTagInfo(string html, string tagsFilter, bool removePunctuation)
        {
            //First remove unspecified HTML Tags ("<....>")
            html = StripUnspecifiedTags(html, tagsFilter, true);

            //Second replace any HTML entities (&nbsp; &lt; etc) through their char symbol
            html = HttpUtility.HtmlDecode(html);

            //Thirdly remove any punctuation
            if (removePunctuation)
            {
                html = StripPunctuation(html, true);
            }

            //Finally remove extra whitespace
            html = StripWhiteSpace(html, true);

            return html;
        }





        /// -----------------------------------------------------------------------------
        /// <summary>
        /// FormatText replaces <br/> tags by LineFeed characters
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="HTML">The HTML content to clean up</param>
        /// <param name="RetainSpace">Whether ratain Space</param>
        /// <returns>The cleaned up string</returns>
        /// <history>
        ///		[cnurse]	12/13/2004	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string FormatText(string HTML, bool RetainSpace)
        {
            //Match all variants of <br> tag (<br>, <BR>, <br/>, including embedded space
            string brMatch = "\\s*<\\s*[bB][rR]\\s*/\\s*>\\s*";
            //Replace Tags by replacement String and return mofified string
            return Regex.Replace(HTML, brMatch, Environment.NewLine);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   Formats String as Html by replacing linefeeds by <br />
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name = "strText">Text to format</param>
        /// <returns>The formatted html</returns>
        /// <history>
        ///   [cnurse]	12/13/2004	Documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string ConvertToHtml(string strText)
        {
            string strHtml = strText;

            if (!string.IsNullOrEmpty(strHtml))
            {
                strHtml = strHtml.Replace("\n", "");
                strHtml = strHtml.Replace("\r", "<br />");
            }

            return strHtml;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   Formats Html as text by removing <br /> tags and replacing by linefeeds
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name = "strHtml">Html to format</param>
        /// <returns>The formatted text</returns>
        /// <history>
        ///   [cnurse]	12/13/2004	Documented and modified to use HtmlUtils methods
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string ConvertToText(string strHtml)
        {
            string strText = strHtml;

            if (!string.IsNullOrEmpty(strText))
            {
                //First remove white space (html does not render white space anyway and it screws up the conversion to text)
                //Replace it by a single space
                strText = StripWhiteSpace(strText, true);

                //Replace all variants of <br> by Linefeeds
                strText = FormatText(strText, false);
            }


            return strText;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Format a domain name including link
        /// </summary>
        /// <param name="Website">The domain name to format</param>
        /// <returns>The formatted domain name</returns>
        /// <history>
        ///		[cnurse]	09/29/2005	moved from Globals
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string FormatWebsite(object Website)
        {
            string formatWebsite = "";
            if (Website != DBNull.Value)
            {
                if (!String.IsNullOrEmpty(Website.ToString().Trim()))
                {
                    if (Website.ToString().IndexOf(".") > -1)
                    {
                        formatWebsite = "<a href=\"" + (Website.ToString().IndexOf("://") > -1 ? "" : "http://") + Website + "\">" + Website + "</a>";
                    }
                    else
                    {
                        formatWebsite = Website.ToString();
                    }
                }
            }
            return formatWebsite;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Shorten returns the first (x) characters of a string
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="txt">The text to reduces</param>
        /// <param name="length">The max number of characters to return</param>
        /// <param name="suffix">An optional suffic to append to the shortened string</param>
        /// <returns>The shortened string</returns>
        /// <history>
        ///		[cnurse]	11/16/2004	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string Shorten(string txt, int length, string suffix)
        {
            string results;
            if (txt.Length > length)
            {
                results = txt.Substring(0, length) + suffix;
            }
            else
            {
                results = txt;
            }
            return results;
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// StripEntities removes the HTML Entities from the content
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="HTML">The HTML content to clean up</param>
        /// <param name="RetainSpace">Indicates whether to replace the Entity by a space (true) or nothing (false)</param>
        /// <returns>The cleaned up string</returns>
        /// <history>
        ///		[cnurse]	11/16/2004	created
        /// </history>
        /// -----------------------------------------------------------------------------
        [Obsolete("This method has been deprecated. Please use System.Web.HtmlUtility.HtmlDecode")]
        public static string StripEntities(string HTML, bool RetainSpace)
        {
            string RepString;
            if (RetainSpace)
            {
                RepString = " ";
            }
            else
            {
                RepString = "";
            }
            //Replace Entities by replacement String and return mofified string
            return Regex.Replace(HTML, "&[^;]*;", RepString);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// StripTags removes the HTML Tags from the content
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="HTML">The HTML content to clean up</param>
        /// <param name="RetainSpace">Indicates whether to replace the Tag by a space (true) or nothing (false)</param>
        /// <returns>The cleaned up string</returns>
        /// <history>
        ///		[cnurse]	11/16/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string StripTags(string HTML, bool RetainSpace)
        {
            //Set up Replacement String
            string RepString;
            if (RetainSpace)
            {
                RepString = " ";
            }
            else
            {
                RepString = "";
            }
            //Replace Tags by replacement String and return mofified string
            return Regex.Replace(HTML, "<[^>]*>", RepString);
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        ///   StripUnspecifiedTags removes the HTML tags from the content -- leaving behind the info 
        ///   for the specified HTML tags.
        /// </summary>
        /// <param name = "html"></param>
        /// <param name="specifiedTags"></param>
        /// <param name = "retainSpace"></param>
        /// <returns>The cleaned up string</returns>
        /// <remarks>
        /// </remarks>
        /// <history>
        ///   [vnguyen]   09/02/2010   Created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string StripUnspecifiedTags(string html, string specifiedTags, bool retainSpace)
        {
            var result = new StringBuilder();

            //Set up Replacement String
            string RepString = null;
            if (retainSpace)
            {
                RepString = " ";
            }
            else
            {
                RepString = "";
            }

            //Stripped HTML
            result.Append(Regex.Replace(html, "<[^>]*>", RepString));

            //Adding Tag info from specified tags
            foreach (Match m in Regex.Matches(html, "(?<=(" + specifiedTags + ")=)\"(?<a>.*?)\""))
            {
                if (m.Value.Length > 0)
                {
                    result.Append(" " + m.Value);
                }
            }

            return result.ToString();
        }


        /// -----------------------------------------------------------------------------
        /// <summary>
        /// StripPunctuation removes the Punctuation from the content
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="HTML">The HTML content to clean up</param>
        /// <param name="RetainSpace">Indicates whether to replace the Punctuation by a space (true) or nothing (false)</param>
        /// <returns>The cleaned up string</returns>
        /// <history>
        ///		[cnurse]	11/16/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string StripPunctuation(string HTML, bool RetainSpace)
        {
            //Create Regular Expression objects
            string punctuationMatch = "[~!#\\$%\\^&*\\(\\)-+=\\{\\[\\}\\]\\|;:\\x22'<,>\\.\\?\\\\\\t\\r\\v\\f\\n]";
            var afterRegEx = new Regex(punctuationMatch + "\\s");
            var beforeRegEx = new Regex("\\s" + punctuationMatch);

            //Define return string
            string retHTML = HTML + " "; //Make sure any punctuation at the end of the String is removed

            //Set up Replacement String
            string RepString;
            if (RetainSpace)
            {
                RepString = " ";
            }
            else
            {
                RepString = "";
            }
            while (beforeRegEx.IsMatch(retHTML))
            {
                retHTML = beforeRegEx.Replace(retHTML, RepString);
            }
            while (afterRegEx.IsMatch(retHTML))
            {
                retHTML = afterRegEx.Replace(retHTML, RepString);
            }
            // Return modified string after trimming leading and ending quotation marks
            return retHTML.Trim('"');
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// StripWhiteSpace removes the WhiteSpace from the content
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="HTML">The HTML content to clean up</param>
        /// <param name="RetainSpace">Indicates whether to replace the WhiteSpace by a space (true) or nothing (false)</param>
        /// <returns>The cleaned up string</returns>
        /// <history>
        ///		[cnurse]	12/13/2004	documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string StripWhiteSpace(string HTML, bool RetainSpace)
        {
            //Set up Replacement String
            string RepString;
            if (RetainSpace)
            {
                RepString = " ";
            }
            else
            {
                RepString = "";
            }

            //Replace Tags by replacement String and return mofified string
            if (!string.IsNullOrEmpty(HTML))
            {
                return Regex.Replace(HTML, "\\s+", RepString);
            }

            return "";
        }

        /// -----------------------------------------------------------------------------
        /// <summary>
        /// StripNonWord removes any Non-Word Character from the content
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="HTML">The HTML content to clean up</param>
        /// <param name="RetainSpace">Indicates whether to replace the Non-Word Character by a space (true) or nothing (false)</param>
        /// <returns>The cleaned up string</returns>
        /// <history>
        ///		[cnurse]	1/28/2005	created
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string StripNonWord(string HTML, bool RetainSpace)
        {
            //Set up Replacement String
            string RepString;
            if (RetainSpace)
            {
                RepString = " ";
            }
            else
            {
                RepString = "";
            }
            if (HTML == null)
            {
            //Replace Tags by replacement String and return modified string
                return HTML;
            }
            else
            {
                return Regex.Replace(HTML, "\\W*", RepString);
            }
        }

        /// <summary>
        ///   Determines wether or not the passed in string contains any HTML tags
        /// </summary>
        /// <param name = "text">Text to be inspected</param>
        /// <returns>True for HTML and False for plain text</returns>
        /// <remarks>
        /// </remarks>
        public static bool IsHtml(string text)
        {
            if ((string.IsNullOrEmpty(text)))
            {
                return false;
            }

            return HtmlDetectionRegex.IsMatch(text);
        }
        

    }
}
