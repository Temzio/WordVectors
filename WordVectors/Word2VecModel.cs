using System.Text;

namespace WordVectors
{
    /// <summary>
    /// Represents a Word2Vec model, providing methods to query for vectors and similarities.
    /// </summary>
    public class Word2VecModel
    {
        private readonly Dictionary<string, float[]> _vectors;

        /// <summary>
        /// Gets the number of words in the vocabulary.
        /// </summary>
        public int VocabularySize { get; }

        /// <summary>
        /// Gets the dimension of the word vectors.
        /// </summary>
        public int VectorSize { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Word2VecModel"/> class.
        /// </summary>
        /// <param name="vectors">A dictionary mapping words to their corresponding float arrays (vectors).</param>
        /// <param name="vectorSize">The dimension of the word vectors.</param>
        private Word2VecModel(Dictionary<string, float[]> vectors, int vectorSize)
        {
            _vectors = vectors;
            VectorSize = vectorSize;
            VocabularySize = vectors.Count;
        }

        /// <summary>
        /// Loads a Word2Vec model from a binary .vec file.
        /// </summary>
        /// <param name="filePath">The path to the binary .vec file.</param>
        /// <returns>A new instance of the <see cref="Word2VecModel"/> class loaded with data from the specified file.</returns>
        public static Word2VecModel Load(string filePath)
        {
            using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using var reader = new BinaryReader(fileStream);

            string header = ReadHeader(reader);
            var parts = header.Split(' ');
            int vocabularySize = int.Parse(parts[0]);
            int vectorSize = int.Parse(parts[1]);
            int vectorByteSize = vectorSize * sizeof(float);

            var vectors = new Dictionary<string, float[]>(vocabularySize);

            for (int i = 0; i < vocabularySize; i++)
            {
                string word = ReadWord(reader);
                if (string.IsNullOrEmpty(word)) continue;

                byte[] vectorBytes = reader.ReadBytes(vectorByteSize);
                var vector = new float[vectorSize];
                Buffer.BlockCopy(vectorBytes, 0, vector, 0, vectorByteSize);
                vectors[word] = vector;

                if (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    byte nextByte = reader.ReadByte();
                    if (nextByte != (byte)'\n')
                    {
                        reader.BaseStream.Position--;
                    }
                }
            }
            return new Word2VecModel(vectors, vectorSize);
        }

        /// <summary>
        /// Reads a string from the binary reader until a specified delimiter byte is encountered.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
        /// <param name="delimiter">The byte that signifies the end of the string.</param>
        /// <returns>The string read from the stream, decoded as UTF-8.</returns>
        private static string ReadStringUntilDelimiter(BinaryReader reader, byte delimiter)
        {
            var bytes = new List<byte>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                byte b = reader.ReadByte();
                if (b == delimiter) break;
                bytes.Add(b);
            }
            return Encoding.UTF8.GetString(bytes.ToArray());
        }

        /// <summary>
        /// Reads the header string from the binary reader, which is terminated by a newline character.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
        /// <returns>The header string.</returns>
        private static string ReadHeader(BinaryReader reader)
        {
            return ReadStringUntilDelimiter(reader, (byte)'\n');
        }

        /// <summary>
        /// Reads a word string from the binary reader, which is terminated by a space character.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> to read from.</param>
        /// <returns>The word string.</returns>
        private static string ReadWord(BinaryReader reader)
        {
            return ReadStringUntilDelimiter(reader, (byte)' ');
        }

        /// <summary>
        /// Retrieves the vector (float array) associated with a given word.
        /// </summary>
        /// <param name="word">The word for which to retrieve the vector.</param>
        /// <returns>The float array representing the word's vector, or null if the word is not found in the vocabulary.</returns>
        public float[]? GetVector(string word)
        {
            _vectors.TryGetValue(word, out var vector);
            return vector;
        }

        /// <summary>
        /// Checks if the model contains the specified word in its vocabulary.
        /// </summary>
        /// <param name="word">The word to check for.</param>
        /// <returns>True if the word is found in the vocabulary; otherwise, false.</returns>
        public bool HasWord(string word)
        {
            return _vectors.ContainsKey(word);
        }

        /// <summary>
        /// Finds words most similar to a given word based on cosine similarity.
        /// </summary>
        /// <param name="word">The word for which to find similar words.</param>
        /// <param name="topN">The number of top similar words to return. Defaults to 10.</param>
        /// <returns>An enumerable of key-value pairs, where the key is the similar word and the value is its cosine similarity,
        /// ordered in descending order of similarity. Returns null if the input word is not found in the model.</returns>
        public IEnumerable<KeyValuePair<string, float>>? FindSimilar(string word, int topN = 10)
        {
            var vector = GetVector(word);
            if (vector == null) return null;
            return FindSimilar(vector, topN, new HashSet<string> { word });
        }

        /// <summary>
        /// Solves an analogy problem (e.g., "man is to king as woman is to X") by performing vector arithmetic.
        /// </summary>
        /// <param name="man">The vector representing the first word in the analogy (e.g., "man").</param>
        /// <param name="king">The vector representing the second word in the analogy (e.g., "king").</param>
        /// <param name="woman">The vector representing the third word in the analogy (e.g., "woman").</param>
        /// <param name="topN">The number of top similar words to return as potential answers. Defaults to 10.</param>
        /// <returns>An enumerable of key-value pairs, where the key is a potential answer word and the value is its cosine similarity
        /// to the calculated analogy vector, ordered in descending order of similarity. Returns null if any of the input words are not found in the model.</returns>
        public IEnumerable<KeyValuePair<string, float>>? FindAnalogy(string man, string king, string woman, int topN = 10)
        {
            var vMan = GetVector(man);
            var vKing = GetVector(king);
            var vWoman = GetVector(woman);

            if (vMan == null || vKing == null || vWoman == null) return null;

            float[] resultVector = VectorMath.Add(VectorMath.Subtract(vKing, vMan), vWoman);
            VectorMath.Normalize(resultVector);

            var wordsToExclude = new HashSet<string> { man, king, woman };
            return FindSimilar(resultVector, topN, wordsToExclude);
        }

        /// <summary>
        /// Finds words most similar to a given target vector based on cosine similarity, excluding a specified set of words.
        /// </summary>
        /// <param name="targetVector">The float array representing the target vector.</param>
        /// <param name="topN">The number of top similar words to return.</param>
        /// <param name="wordsToExclude">A set of words to exclude from the similarity search results.</param>
        /// <returns>An enumerable of key-value pairs, where the key is the similar word and the value is its cosine similarity,
        /// ordered in descending order of similarity.</returns>
        private IEnumerable<KeyValuePair<string, float>> FindSimilar(float[] targetVector, int topN, ISet<string> wordsToExclude)
        {
            return _vectors
                .Where(kvp => !wordsToExclude.Contains(kvp.Key))
                .Select(kvp => new KeyValuePair<string, float>(
                    kvp.Key,
                    VectorMath.DotProduct(targetVector, kvp.Value)
                ))
                .OrderByDescending(kvp => kvp.Value)
                .Take(topN);
        }
    }
}