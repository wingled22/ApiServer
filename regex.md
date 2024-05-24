
# Comprehensive Guide to Regular Expressions in C#

## Introduction to Regular Expressions

Regular expressions (regex) are powerful tools for pattern matching and text manipulation. In C#, regular expressions are implemented through the `System.Text.RegularExpressions` namespace. They allow you to define search patterns using a formal syntax.

## Basic Syntax

### Literal Characters

Literal characters in a regex pattern match themselves. For example, the pattern `cat` will match the string "cat" in the text.

```csharp
Regex regex = new Regex("cat");
bool isMatch = regex.IsMatch("The cat is sleeping.");
```

### Character Classes

Character classes match any one of a set of characters. Use square brackets `[]` to define a character class.

```csharp
Regex regex = new Regex("[aeiou]");
bool isMatch = regex.IsMatch("hello");
```

### Quantifiers

Quantifiers specify how many times a character or group should be repeated.

- `*`: Zero or more times.
- `+`: One or more times.
- `?`: Zero or one time.
- `{n}`: Exactly `n` times.
- `{n,}`: At least `n` times.
- `{n,m}`: Between `n` and `m` times.

```csharp
Regex regex = new Regex("a+");
bool isMatch = regex.IsMatch("aaabbb");
```

### Anchors

Anchors assert a position in the string rather than matching a character.

- `^`: Matches the start of a line.
- `$`: Matches the end of a line.
- `\b`: Matches a word boundary.

```csharp
Regex regex = new Regex("^start");
bool isMatch = regex.IsMatch("start of the line");
```

### Escape Characters

Use backslashes to escape special characters.

```csharp
Regex regex = new Regex(@"\d+"); // Matches digits
bool isMatch = regex.IsMatch("123");
```

## Advanced Techniques

### Groups and Capturing

Parentheses `()` define a group. Groups can be captured and referenced.

```csharp
Regex regex = new Regex(@"(\d{3})-(\d{3})-(\d{4})");
Match match = regex.Match("Phone: 123-456-7890");
string phoneNumber = match.Groups[0].Value;
```

### Alternation

The pipe `|` character allows for alternation, matching either of two patterns.

```csharp
Regex regex = new Regex(@"cat|dog");
bool isMatch = regex.IsMatch("I have a cat.");
```

### Lookahead and Lookbehind

Lookahead and lookbehind assertions allow you to check if a pattern is followed or preceded by another pattern without consuming the characters.

```csharp
Regex regex = new Regex(@"\b\w+(?=ing\b)");
MatchCollection matches = regex.Matches("Walking and talking are actions.");
```

## Using Regular Expressions in C#

### Creating a Regex Object

Use the `Regex` class to create a regex object.

```csharp
Regex regex = new Regex(pattern);
```

### Matching Strings

Use the `IsMatch()` method to check if a string matches a pattern.

```csharp
bool isMatch = regex.IsMatch(input);
```

### Finding Matches

Use the `Match()` method to find the first match in a string.

```csharp
Match match = regex.Match(input);
```

### Finding All Matches

Use the `Matches()` method to find all matches in a string.

```csharp
MatchCollection matches = regex.Matches(input);
```

### Replacing Matches

Use the `Replace()` method to replace matches in a string.

```csharp
string result = regex.Replace(input, replacement);
```

## Conclusion

Regular expressions are a powerful tool for string manipulation and pattern matching in C#. By mastering regex syntax and techniques, you can effectively search, validate, and transform text data in your applications.

This guide provides a solid foundation for using regular expressions in C#. Experiment with different patterns and techniques to become proficient in regex usage.