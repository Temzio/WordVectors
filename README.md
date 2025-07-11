# WordVectors

WordVectors is a C# library designed to load, query, and perform vector arithmetic on pre-trained Word2Vec models. It provides functionalities to find similar words and solve analogy problems based on the continuous bag-of-words (CBOW) or skip-gram architectures.

## Features

- **Load Word2Vec Models**: Efficiently load binary Word2Vec models (compatible with models trained by the original C Word2Vec tool).
    
- **Vector Retrieval**: Get the numerical vector representation for any word in the vocabulary.
    
- **Similarity Search**: Find the most similar words to a given word based on cosine similarity.
    
- **Analogy Solving**: Perform vector analogies (e.g., "king - man + woman = queen") to discover semantic relationships between words.
    
- **Basic Vector Math**: Includes utilities for dot product, normalization, addition, and subtraction of vectors.
    

## Installation

You can install the WordVectors library via NuGet Package Manager.

### NuGet Package Manager

```
dotnet add package WordVectors --version 0.2.2
```

For more details, visit the [WordVectors NuGet page](https://www.nuget.org/packages/WordVectors/ "null").

### From Source

1. Clone or download the repository containing the `Word2VecModel.cs` and `VectorMath.cs` files.
    
2. Add these files to your C# project in Visual Studio or your preferred IDE.
    

## Usage

### Loading a Word2Vec Model

To get started, you need a pre-trained Word2Vec binary model file (e.g., `vectors.bin`). You can download pre-trained models from various sources, such as Google's Word2Vec project or Stanford's GloVe project (note: GloVe models might need conversion to Word2Vec binary format if not already in that format).

```
using WordVectors;
using System;
using System.Collections.Generic;

public class Program
{
    public static void Main(string[] args)
    {
        string modelPath = "path/to/your/vectors.bin"; // Replace with the actual path to your model file

        try
        {
            Console.WriteLine("Loading Word2Vec model...");
            Word2VecModel model = Word2VecModel.Load(modelPath);
            Console.WriteLine($"Model loaded. Vocabulary Size: {model.VocabularySize}, Vector Size: {model.VectorSize}");

            // Now you can start querying the model
            QueryModel(model);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine($"Error: Model file not found at {modelPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void QueryModel(Word2VecModel model)
    {
        Console.WriteLine("\n--- Querying the model ---");

        // Get a word vector
        string wordToQuery = "apple";
        float[]? appleVector = model.GetVector(wordToQuery);
        if (appleVector != null)
        {
            Console.WriteLine($"\nVector for '{wordToQuery}' (first 5 dimensions): {string.Join(", ", appleVector.Take(5))}");
        }
        else
        {
            Console.WriteLine($"\n'{wordToQuery}' not found in vocabulary.");
        }

        // Check if a word exists
        Console.WriteLine($"Does 'banana' exist? {model.HasWord("banana")}");
        Console.WriteLine($"Does 'xylophone' exist? {model.HasWord("xylophone")}");

        // Find similar words
        string similarWordQuery = "king";
        Console.WriteLine($"\nWords similar to '{similarWordQuery}':");
        IEnumerable<KeyValuePair<string, float>>? similarWords = model.FindSimilar(similarWordQuery, topN: 5);
        if (similarWords != null)
        {
            foreach (var kvp in similarWords)
            {
                Console.WriteLine($"- {kvp.Key}: {kvp.Value:F4}");
            }
        }
        else
        {
            Console.WriteLine($"'{similarWordQuery}' not found in vocabulary for similarity search.");
        }

        // Solve an analogy
        string man = "man";
        string king = "king";
        string woman = "woman";
        Console.WriteLine($"\nAnalogy: '{man}' is to '{king}' as '{woman}' is to X");
        IEnumerable<KeyValuePair<string, float>>? analogyResults = model.FindAnalogy(man, king, woman, topN: 5);
        if (analogyResults != null)
        {
            foreach (var kvp in analogyResults)
            {
                Console.WriteLine($"- {kvp.Key}: {kvp.Value:F4}");
            }
        }
        else
        {
            Console.WriteLine($"One or more words for the analogy not found in vocabulary.");
        }
    }
}
```

## API Documentation

### `Word2VecModel` Class

Represents a Word2Vec model, providing methods to query for vectors and similarities.

#### Properties

- **`VocabularySize`** (int): Gets the number of words in the vocabulary.
    
- **`VectorSize`** (int): Gets the dimension of the word vectors.
    

#### Static Methods

- **`static Word2VecModel Load(string filePath)`** Loads a Word2Vec model from a binary `.vec` file.
    
    - `filePath`: The path to the binary `.vec` file.
        
    - **Returns**: A new instance of the `Word2VecModel` class loaded with data from the specified file.
        

#### Public Methods

- **`float[]? GetVector(string word)`** Retrieves the vector (float array) associated with a given word.
    
    - `word`: The word for which to retrieve the vector.
        
    - **Returns**: The float array representing the word's vector, or `null` if the word is not found in the vocabulary.
        
- **`bool HasWord(string word)`** Checks if the model contains the specified word in its vocabulary.
    
    - `word`: The word to check for.
        
    - **Returns**: `true` if the word is found in the vocabulary; otherwise, `false`.
        
- **`IEnumerable<KeyValuePair<string, float>>? FindSimilar(string word, int topN = 10)`** Finds words most similar to a given word based on cosine similarity.
    
    - `word`: The word for which to find similar words.
        
    - `topN`: The number of top similar words to return. Defaults to 10.
        
    - **Returns**: An enumerable of key-value pairs, where the key is the similar word and the value is its cosine similarity, ordered in descending order of similarity. Returns `null` if the input word is not found in the model.
        
- **`IEnumerable<KeyValuePair<string, float>>? FindAnalogy(string man, string king, string woman, int topN = 10)`** Solves an analogy problem (e.g., "man is to king as woman is to X") by performing vector arithmetic.
    
    - `man`: The word representing the first concept in the analogy (e.g., "man").
        
    - `king`: The word representing the second concept in the analogy (e.g., "king").
        
    - `woman`: The word representing the third concept in the analogy (e.g., "woman").
        
    - `topN`: The number of top similar words to return as potential answers. Defaults to 10.
        
    - **Returns**: An enumerable of key-value pairs, where the key is a potential answer word and the value is its cosine similarity to the calculated analogy vector, ordered in descending order of similarity. Returns `null` if any of the input words are not found in the model.
        

### `VectorMath` Static Class

Provides static methods for common vector mathematics operations.

#### Static Methods

- **`static float DotProduct(float[] vec1, float[] vec2)`** Calculates the dot product of two vectors.
    
    - `vec1`: The first vector.
        
    - `vec2`: The second vector.
        
    - **Returns**: The dot product of the two vectors.
        
    - **Remarks**: This method assumes that both input vectors have the same length.
        
- **`static void Normalize(float[] vector)`** Normalizes a vector in-place, scaling it to have a magnitude (L2 norm) of 1.
    
    - `vector`: The vector to normalize.
        
- **`static float[] Add(float[] vec1, float[] vec2)`** Adds two vectors element-wise.
    
    - `vec1`: The first vector.
        
    - `vec2`: The second vector.
        
    - **Returns**: A new float array representing the sum of the two vectors.
        
    - **Remarks**: This method assumes that both input vectors have the same length.
        
- **`static float[] Subtract(float[] vec1, float[] vec2)`** Subtracts the second vector from the first vector element-wise.
    
    - `vec1`: The vector from which to subtract.
        
    - `vec2`: The vector to subtract.
        
    - **Returns**: A new float array representing the result of the subtraction.
        
    - **Remarks**: This method assumes that both input vectors have the same length.
        

## Requirements

- .NET 8.0+

## Contributing

If you'd like to contribute to WordVectors, please feel free to fork the repository and submit pull requests.
