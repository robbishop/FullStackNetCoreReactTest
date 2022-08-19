# FullStackNetCoreReactTest
## Back End

The existing API provides a set of statistics for a given string. It provides Character, Line, Sentence and Paragraph counts for a string.
As a developer you have been asked to develop two features for this existing API

### New Features

#### Add a new endpoint

A new endpoint that will allow the upload of a text file. The file should be read, and the string contents used to provide the statistics.

#### Add a new statistic: Top Ten words

A new statistic should be provided for both API's. This will be a top ten list of words, ordered by their incidence in the text. If there are less that ten individual words, then return just the words that exist. In the event of a tie, then the words are ranked in alphabetical order. The word counts will be case-insensitive ("One" and "one" are the same) and should in return lower case.

### Clean Up

You have also been asked to try to clean up/refactor the codebase if you are able to. Some parts are not using best practices

## Front End

The front-end project contains a very basic layout where to implement a user interface for our users to interact with the API.

The provided project skeleton is based on Create React App, and it comes with basic layout and styles.

CORS is also managed by using Create React Apps' proxy, so you can simply run requests to `/api/...` and they will be properly redirected to the back end. Please update the `proxy` key in `package.json` if you change the port the API is listening to.

The [`axios`](https://github.com/axios/axios) library is configured to make REST calls - there's a working example in `App.tsx`. You're not enforced to use it, though, feel free to use whatever client you feel comfortable with, like `fetch`.

### Function components

Use function components and hooks to implement every client. Feel free to create as many components and custom hooks as you wish, the use of best practices is expected.

### Sections to implement

#### Text input section

Add a basic form with an input text and a button with the following behaviour:

* The submit button is enabled when the input text is not empty
* The submit button triggers a REST call to the `GetTextStats` action
* The response of the API must be rendered in this section

> :exclamation: **Task**

This section is already implemented but it requires a refactor to apply best practices.

#### File upload section

> :exclamation: **Task**

Add a basic form with a file upload component and a button with the following behaviour:

* The submit button is enabled when a file has been selected
* The submit button triggers a REST call to the new endpoint required as part of the back-end challenge.
* The response of the API must be rendered in this section

#### Statistics

> :exclamation: **Task**

Add display area containing the following information:

* Number of requests sent to the text stats endpoint
* Number of requests sent to the file upload stats endpoint

Data must be stored and retrieved **using Redux**.

### Responsive

> :exclamation: **Task**

Please adjust the layout and styles of the provided application to make it responsive:

* Mobile: all sections in one column, each section is 100% width
* Tablet+: all sections in one row, each section is one third of the full width

You can use whatever approach you want:

* Flexbox or CSS Grid
* Custom CSS/SCSS, a CSS library ([TailwindCSS](https://tailwindcss.com/)) or component library ([MUI](https://mui.com/), others)

### Performance

:star: Please consider performance when implementing your components. Pay attention to wasted renders.
