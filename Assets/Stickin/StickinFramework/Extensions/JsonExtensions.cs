
namespace stickin
{
    public static class JsonExtensions
    {
        public static string AddedQuotesMazeGame(string str)
        {
            str = AddedSymbolBeforeSymbol(str, "r", "\"");
            str = AddedSymbolBeforeSymbol(str, "c", "\"");
            str = AddedSymbolBeforeSymbol(str, "s", "\"");
            str = AddedSymbolBeforeSymbol(str, "f", "\"");
            str = AddedSymbolBeforeSymbol(str, "h", "\"");
            str = AddedSymbolBeforeSymbol(str, "w", "\"");
            str = AddedSymbolBeforeSymbol(str, "i", "\"");
            str = AddedSymbolBeforeSymbol(str, "e", "\"");
            str = AddedSymbolBeforeSymbol(str, "t", "\"");

            str = AddedSymbolBeforeSymbol(str, "o", "\"");
            str = AddedSymbolBeforeSymbol(str, "d", "\"");

            str = AddedSymbolBeforeSymbol(str, "x", "\"");
            str = AddedSymbolBeforeSymbol(str, "y", "\"");

            str = AddedSymbolBeforeSymbol(str, ":", "\"");
            // str = AddedSymbolAfterSymbol(str, ":", "\"");

            return str;
        }

        public static string AddedQuotesPipesGame(string str)
        {
            var al = "qwertyuiopasdfghjklzxcvbnm";
            for (var i = 0; i < al.Length; i++)
                str = AddedSymbolBeforeAndAfterSymbol(str, al[i].ToString(), "\"");

            return str;
        }

        private static string AddedSymbolBeforeAndAfterSymbol(string str, string findStr, string insertStr)
        {
            str = AddedSymbolBeforeSymbol(str, findStr, insertStr);
            str = AddedSymbolAfterSymbol(str, findStr, insertStr, null);

            return str;
        }

        public static string AddedSymbolBeforeSymbol(string str, string findStr, string insertStr,
            string ignorePrevSymbol = null)
        {
            return AddedSymbol(str, findStr, insertStr, 0, ignorePrevSymbol, -1);
        }

        public static string AddedSymbolAfterSymbol(string str, string findStr, string insertStr,
            string ignoreNextSymbol = null)
        {
            return AddedSymbol(str, findStr, insertStr, 1, ignoreNextSymbol, 1);
        }

        private static string AddedSymbol(string str, string findStr, string insertStr, int addedIndex,
            string ignoreNextSymbol, int stepNextSymbol)
        {
            var startIndex = 0;
            var maxSteps = str.Length;

            while (maxSteps > 0)
            {
                maxSteps--;

                var index = str.IndexOf(findStr, startIndex);
                if (index >= 0 && index < str.Length)
                {
                    if (ignoreNextSymbol == null || ignoreNextSymbol.Contains(str[index + stepNextSymbol]) == false)
                    {
                        index += addedIndex;
                        str = str.Insert(index, insertStr);
                        startIndex = index + 1 + insertStr.Length;
                    }
                    else
                    {
                        startIndex++;
                    }

                }
                else
                    break;
            }

            return str;
        }
    }
}