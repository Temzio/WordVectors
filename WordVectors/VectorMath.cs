namespace WordVectors
{
    /// <summary>
    /// Provides static methods for common vector mathematics operations.
    /// </summary>
    public static class VectorMath
    {
        /// <summary>
        /// Calculates the dot product of two vectors.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        /// <remarks>
        /// This method assumes that both input vectors have the same length.
        /// </remarks>
        public static float DotProduct(float[] vec1, float[] vec2)
        {
            float dotProduct = 0;
            for (int i = 0; i < vec1.Length; i++)
            {
                dotProduct += vec1[i] * vec2[i];
            }
            return dotProduct;
        }

        /// <summary>
        /// Normalizes a vector in-place, scaling it to have a magnitude (L2 norm) of 1.
        /// </summary>
        /// <param name="vector">The vector to normalize.</param>
        public static void Normalize(float[] vector)
        {
            float magnitude = (float)Math.Sqrt(DotProduct(vector, vector));
            if (magnitude > 0)
            {
                for (int i = 0; i < vector.Length; i++)
                {
                    vector[i] /= magnitude;
                }
            }
        }

        /// <summary>
        /// Adds two vectors element-wise.
        /// </summary>
        /// <param name="vec1">The first vector.</param>
        /// <param name="vec2">The second vector.</param>
        /// <returns>A new float array representing the sum of the two vectors.</returns>
        /// <remarks>
        /// This method assumes that both input vectors have the same length.
        /// </remarks>
        public static float[] Add(float[] vec1, float[] vec2)
        {
            return vec1.Select((v, i) => v + vec2[i]).ToArray();
        }

        /// <summary>
        /// Subtracts the second vector from the first vector element-wise.
        /// </summary>
        /// <param name="vec1">The vector from which to subtract.</param>
        /// <param name="vec2">The vector to subtract.</param>
        /// <returns>A new float array representing the result of the subtraction.</returns>
        /// <remarks>
        /// This method assumes that both input vectors have the same length.
        /// </remarks>
        public static float[] Subtract(float[] vec1, float[] vec2)
        {
            return vec1.Select((v, i) => v - vec2[i]).ToArray();
        }
    }
}