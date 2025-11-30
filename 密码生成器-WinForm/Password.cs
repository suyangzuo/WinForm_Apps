namespace 密码生成器_WinForm
{
    class Password
    {
        [Flags]
        public enum PwdType
        {
            //Decimal     //Binary
            None = 0,     //0000 0000
            Letters = 1,  //0000 0001
            Digits = 2,   //0000 0010
            Symbols = 4,  //0000 0100
            LettersAndDigits = Letters | Digits,
            LettersAndSymbols = Letters | Symbols,
            All = Letters | Digits | Symbols
        }

        public static string GetPassword(int length, PwdType pwdType)
        {
            // Build per-category pools
            var rnd = new Random(Guid.NewGuid().GetHashCode());

            var letters = new List<char>();
            for (char c = 'A'; c <= 'Z'; c++) letters.Add(c);
            for (char c = 'a'; c <= 'z'; c++) letters.Add(c);

            var digits = new List<char>();
            for (char c = '0'; c <= '9'; c++) digits.Add(c);

            var symbols = new List<char>();
            void AddRange(int s, int e)
            {
                for (int x = s; x <= e; x++) symbols.Add((char)x);
            }
            AddRange(33, 47);
            AddRange(58, 64);
            AddRange(91, 96);
            AddRange(123, 126);

            var selectedPools = new List<char[]>();
            if ((pwdType & PwdType.Letters) != 0) selectedPools.Add(letters.ToArray());
            if ((pwdType & PwdType.Digits) != 0) selectedPools.Add(digits.ToArray());
            if ((pwdType & PwdType.Symbols) != 0) selectedPools.Add(symbols.ToArray());

            // If no type selected, return empty string
            if (selectedPools.Count == 0) return string.Empty;

            // Ensure length is at least number of required categories
            if (length < selectedPools.Count) length = selectedPools.Count;

            var result = new char[length];
            var availableIndices = new List<int>(length);
            for (int i = 0; i < length; i++) availableIndices.Add(i);

            // Place at least one char from each selected category into random positions
            foreach (var pool in selectedPools)
            {
                int posIndex = rnd.Next(availableIndices.Count);
                int pos = availableIndices[posIndex];
                result[pos] = pool[rnd.Next(pool.Length)];
                availableIndices.RemoveAt(posIndex);
            }

            // Build combined allowed pool
            var combined = new List<char>();
            foreach (var pool in selectedPools) combined.AddRange(pool);

            // Fill remaining positions randomly from combined pool
            for (int i = 0; i < availableIndices.Count; i++)
            {
                int pos = availableIndices[i];
                result[pos] = combined[rnd.Next(combined.Count)];
            }

            return new string(result);
        }
    }
}
