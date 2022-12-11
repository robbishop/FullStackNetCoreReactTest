import React, { useRef, useState } from "react";
import axios from "axios";
import { Counter } from "./features/counter/Counter";

import logo from "./logo.svg";
import "./App.css";

const DEFAULT_STATS_TEXT = "Stats will appear here.\n.\n.\n.\n.";

function App() {
  const textField = useRef<HTMLTextAreaElement>(null);
  const [text, setText] = useState("");
  const [textStats, setTextStats] = useState(DEFAULT_STATS_TEXT);
  const newTextStats = () => {
    axios
      .post("/api/TextStats", text, {
        headers: { "content-type": "application/json" },
      })
      .then((response) => {
        setTextStats(response.data);
        // console.log(`Stats for "${text}": `, response);
      });
  };
  const textChanged = () => {
    const enteredText = textField.current!.value;
    setText(enteredText);
    if (!enteredText) {
      setTextStats(DEFAULT_STATS_TEXT);
    }
  };
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <h1>Front-end Challenge</h1>
      </header>
      <article className="App-body">
        <section>
          <Counter />
        </section>
        <section className="text-stats">
          <div className="container">
            <h3>Free text</h3>
            <textarea
              style={{ width: "100%" }}
              ref={textField}
              onChange={textChanged}
              rows={8}
            ></textarea>
            <button disabled={!text} onClick={newTextStats}>
              Send
            </button>
            <pre>{textStats}</pre>
          </div>
        </section>
        <section className="file-stats">
          <div className="container">
            <h3>From file</h3>
            <p>Implement here file upload client</p>
          </div>
        </section>
        <section className="client-stats">
          <div className="container">
            <h3>Client Statistics</h3>
            <ul>
              <li>
                Number of free text requests: <code>Unknown</code>
              </li>
              <li>
                Number of file requests: <code>Unknown</code>
              </li>
            </ul>
          </div>
        </section>
      </article>
    </div>
  );
}

export default App;
