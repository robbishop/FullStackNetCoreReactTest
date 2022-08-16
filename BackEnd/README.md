# Core-Web-Api-Test

The existing API provides a set of statistics for a given string. It provides Character, Line, Sentence and Paragraph counts for a string.
As a developer you have been asked to develop two features for this existing API

## New Features

### Add a new endpoint

A new endpoint that will allow the upload of a text file. The file should be read, and the string contents used to provide the statistics.

### Add a new statistic: Top Ten words

A new statistic should be provided for both API's. This will be a top ten list of words, ordered by their incidence in the text. If there are less that ten individual words, then return just the words that exist. In the event of a tie, then the words are ranked in alphabetical order. The word counts will be case-insensitive ("One" and "one" are the same) and should in return lower case.

## Clean Up

You have also been asked to try to clean up/refactor the codebase if you are able to. Some parts are not using best practices

